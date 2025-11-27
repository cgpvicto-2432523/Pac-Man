using UnityEngine;

/// <summary>
/// Gere le deplacement du pacman et des fantomes dans le jeu, 
/// genre les obstacles sur leur route
/// </summary>
public class Movement : MonoBehaviour
{
    public float speed = 8f; //vitesse du pacman et des fantomes
    public float speedMultiplier = 1f; // Multiplicateur de vitesse (utilisé pour ralentir/accélérer temporairement)
    public Vector2 initialDirection; // Direction initiale au démarrage
    public LayerMask obstacleLayer; // Layer des obstacles (murs) à détecter

    public Rigidbody2D rb { get; private set; }
    public Vector2 direction { get; private set; } //direction actuelle
    public Vector2 nextDirection { get; private set; } // Prochaine direction en attente (si bloquée par un mur)
    public Vector3 startingPosition { get; private set; } // Position de départ pour réinitialisation

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }


    /// <summary>
    /// Initialise l'état au début du jeu
    /// </summary>
    private void Start()
    {
        ResetState();
    }

    /// <summary>
    /// Réinitialise le mouvement à son état de départ
    /// Utilisé au début du jeu ou après une mort
    /// </summary>
    public void ResetState()
    {
        speedMultiplier = 1f;                    // Vitesse normale
        direction = initialDirection;             // Direction initiale
        nextDirection = Vector2.zero;             // Pas de direction en attente
        transform.position = startingPosition;    // Retour à la position de départ
        rb.isKinematic = false;                   // Physique activée
        enabled = true;                           // Script activé
    }

    /// <summary>
    /// appliquer la direction en attente à chaque frame
    /// </summary>
    private void Update()
    {
        // Si une direction est en attente, essayer de l'appliquer
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        Vector2 translation = speed * speedMultiplier * Time.fixedDeltaTime * direction;

        rb.MovePosition(position + translation);
    }

    /// <summary>
    /// Change la direction de déplacement
    /// Si un obstacle bloque, met la direction en attente
    /// </summary>
    /// <param name="direction">Nouvelle direction souhaitée</param>
    /// <param name="forced">Si true, ignore la détection d'obstacles</param>
    public void SetDirection(Vector2 direction, bool forced = false)
    {
        // Changer la direction seulement si pas d'obstacle OU si forcé
        if (forced || !Occupied(direction))
        {
            this.direction = direction;       // Appliquer la direction
            nextDirection = Vector2.zero;     // Vider la file d'attente
        }
        else
        {
            // Direction bloquée, la mettre en attente
            nextDirection = direction;
        }
    }


    /// <summary>
    /// Vérifie s'il y a un obstacle dans une direction donnée
    /// Utilise un BoxCast pour détecter les murs
    /// </summary>
    /// <param name="direction">Direction à vérifier</param>
    /// <returns>True si un obstacle est présent, False sinon</returns>
    public bool Occupied(Vector2 direction)
    {
            // If no collider is hit then there is no obstacle in that direction
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
            return hit.collider != null;
        

    }
}
