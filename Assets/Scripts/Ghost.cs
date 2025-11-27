using UnityEngine;

/// <summary>
/// Le fantome a la base
/// </summary>
public class Ghost : MonoBehaviour
{

    /// <summary>
    /// DEfinition des différents comportemnts d'unfantome
    /// </summary>
    public Movement movement { get; private set; } // en mouvement
    public GhostHome home { get; private set; } //rester dans la base (carré centrale)
    public GhostScatter scatter { get; private set; } //patrouillé vers un coin
    public GhostChase chase { get; private set; } //poursuivre le pacman
    public GhostFrightened frightened { get; private set; } //fantome vulnérable
    public GhostBehavior initialBehavior;//comportement de départ du fantome
    public Transform target; //le pacman
    public int points = 200; //points par fantome mangé

    /// <summary>
    ///  Récuperer le nécessaire au démarrage
    /// </summary>
    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    /// <summary>
    /// Au début, on initialise l'état du fantome
    /// </summary>
    private void Start()
    {
        ResetDeLEtat();
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetDeLEtat()
    {
        gameObject.SetActive(true); // S'assurer que le fantôme est actif
        movement.ResetDeLEtat(); // Réinitialiser le mouvement

        //desactiver tous les comportements additionnels (vulnérable, en chasse)
        frightened.Disable();
        chase.Disable();

        //mode par défaut, se deplacer vers un coin
        scatter.Enable();

        //si le comportement initiale n'est pas home, on le desactive
        if (home != initialBehavior) {
            home.Disable();
        }

        // Activer le comportement initial configuré
        if (initialBehavior != null) {
            initialBehavior.Enable();
        }
    }


    /// <summary>
    /// Change la position du fantôme 
    /// </summary>
    /// <param name="position"></param>
    public void DefinirLaDirection(Vector3 position)
    {
        position.z = transform.position.z;
        transform.position = position;
    }


    /// <summary>
    /// collidion avec pacman
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled) {
                GameManager.Instance.FantomeMange(this); //Si le fantome n'est pas vulnérable pacman perd une vie
            } else {
                GameManager.Instance.PacmanMange(); //si il était en mode vulnérable pacman se régale
            }
        }
    }

}
