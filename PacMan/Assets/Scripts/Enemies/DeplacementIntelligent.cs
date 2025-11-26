using UnityEngine;

public class DeplacementIntelligent : MonoBehaviour
{

    public float vitesseNormale = 3f;
    public float vitesseVulnerable = 1.5f;
    public Sprite spriteNormal;
    public Sprite spriteVulnerable;

    private float vitesseActuelle;
    private Vector2 direction;
    public bool estVulnerable = false;

    public float distanceDetectionMur = 0.6f; 
    public float tempsAvantChangement = 8f;
    private float timerChangementDirection;
    private bool peutChangerDirection = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Initialisation des composants
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Démarrage - Choisir direction initiale
    /// </summary>
    private void Start()
    {
        vitesseActuelle = vitesseNormale;

        if (spriteNormal != null)
        {
            spriteRenderer.sprite = spriteNormal;
        }

        // Trouver une direction libre au départ
        TrouverDirectionLibre();

        // Initialiser le timer
        timerChangementDirection = tempsAvantChangement;
    }

    /// <summary>
    /// Update - Gestion du timer et détection murs
    /// </summary>
    private void Update()
    {
        // Détecter si un mur est devant
        if (MurDevant())
        {
            // Mur détecté! Trouver une nouvelle direction immédiatement
            TrouverDirectionLibre();
            // Réinitialiser le timer
            timerChangementDirection = tempsAvantChangement;
        }
        else
        {
            // Pas de mur, décrémenter le timer
            timerChangementDirection -= Time.deltaTime;

            // Timer écoulé? Changer de direction aléatoirement
            if (timerChangementDirection <= 0)
            {
                TrouverDirectionLibre();
                timerChangementDirection = tempsAvantChangement; // Reset timer
            }
        }
    }

    /// <summary>
    /// Déplacement du fantôme
    /// </summary>
    private void FixedUpdate()
    {
        Vector2 deplacement = rb.position + direction * vitesseActuelle * Time.fixedDeltaTime;
        rb.MovePosition(deplacement);
    }

    /// <summary>
    /// Détecte si un mur est devant le fantôme
    /// </summary>
    private bool MurDevant()
    {
        // Raycast dans la direction actuelle
        RaycastHit2D hit = Physics2D.Raycast(
            rb.position,
            direction,
            distanceDetectionMur,
            LayerMask.GetMask("mur")
        );

        // Si hit.collider existe, il y a un mur
        return hit.collider != null;
    }

    /// <summary>
    /// Trouve une direction libre (sans mur) de façon aléatoire
    /// </summary>
    private void TrouverDirectionLibre()
    {
        Vector2[] directionsDisponibles = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        // Mélanger les directions pour avoir un ordre aléatoire
        ShuffleArray(directionsDisponibles);

        // Essayer chaque direction jusqu'à trouver une direction libre
        foreach (Vector2 dir in directionsDisponibles)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                rb.position,
                dir,
                distanceDetectionMur,
                LayerMask.GetMask("mur")
            );

            // Pas de mur dans cette direction?
            if (hit.collider == null)
            {
                direction = dir; // Choisir cette direction
                Debug.Log(gameObject.name + " nouvelle direction: " + dir);
                return; // Sortir de la fonction
            }
        }

        // Si aucune direction libre (coincé dans un coin), garder direction actuelle
        Debug.LogWarning(gameObject.name + " coincé! Garde direction actuelle.");
    }

    /// <summary>
    /// Mélange un tableau de façon aléatoire (Fisher-Yates shuffle)
    /// Source: https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
    /// </summary>
    private void ShuffleArray(Vector2[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            // Échanger
            Vector2 temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Activer/désactiver la vulnérabilité du fantôme
    /// </summary>
    public void ModifierLaVulnerabilite(bool vulnerable)
    {
        estVulnerable = vulnerable;

        if (vulnerable)
        {
            vitesseActuelle = vitesseVulnerable;
            if (spriteVulnerable != null)
            {
                spriteRenderer.sprite = spriteVulnerable;
            }
        }
        else
        {
            vitesseActuelle = vitesseNormale;
            if (spriteNormal != null)
            {
                spriteRenderer.sprite = spriteNormal;
            }
        }
    }

    /// <summary>
    /// Méthode de gestion des collisions entre les fantômes et les joueurs
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (estVulnerable)
            {
                // Le joueur mange le fantôme
                GameManager.Instance.AjouterLeScore(200);
                Destroy(gameObject);
            }
            else
            {
                // Le fantôme tue le joueur
                GameManager.Instance.PerdreUneVie();
            }
        }
    }

    /// <summary>
    /// Debug visuel - Dessiner la direction du raycast
    /// </summary>
    private void OnDrawGizmos()
    {
        if (rb != null && direction != Vector2.zero)
        {
            // Dessiner une ligne rouge dans la direction du fantôme
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rb.position, direction * distanceDetectionMur);
        }
    }
}
