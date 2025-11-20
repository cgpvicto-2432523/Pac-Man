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

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        vitesseActuelle = vitesseNormale;

        if (spriteNormal != null)
        {
            spriteRenderer.sprite = spriteNormal;
        }

        ChoisirNouvelleDirection();
        Invoke("ChoisirNouvelleDirection", 2f);
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
    /// Methode pour choisir une direction aléatoire du fantôme dans le terrain
    /// </summary>
    private void ChoisirNouvelleDirection()
    {
        int random = Random.Range(0, 4);

        if (random == 0)
            direction = Vector2.up;
        else if (random == 1)
            direction = Vector2.down;
        else if (random == 2)
            direction = Vector2.left;
        else
            direction = Vector2.right;
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
            spriteRenderer.sprite = spriteVulnerable;
        }
        else
        {
            vitesseActuelle = vitesseNormale;
            spriteRenderer.sprite = spriteNormal;
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
                GameManager.Instance.AjouterLeScore(5);
                Destroy(gameObject);
            }
            else
            {
                // Le fantôme tue diminue la vie du joueur
                GameManager.Instance.PerdreUneVie();
            }

        }
    }
}
