using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public InputActionAsset inputActions;
    private InputAction mvmtAction;
    private Rigidbody2D rb;
    private Vector2 direction= Vector2.zero;
    private Vector2 directionActuelle = Vector2.right; // permet de garantir un déplacement même lorsque la touche de déplacement est lachée
                                                        //Elle sauvegardera la direction
    private void Awake()
    {
        mvmtAction = inputActions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = mvmtAction.ReadValue<Vector2>();

        // Changer la direction actuelle seulement si on appuie sur une touche
        if (direction != Vector2.zero)
        {
            directionActuelle= direction.normalized;
            Debug.Log("Direction: " + directionActuelle); //tester le fonctionnement des directions
        }
    }

    /// <summary>
    /// Modification de la physique du joueur
    /// </summary>
    private void FixedUpdate()
    {
        Vector2 deplacement = rb.position + directionActuelle * speed * Time.fixedDeltaTime;
        rb.MovePosition(deplacement);
    }

    /// <summary>
    /// Détection de collision avec les collectibles
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Géré par les scripts Pellet.cs et GhostAI.cs
        // On laisse vide pour l'instant
    }

    /// <summary>
    /// Activer les inputs quand l'objet est activé
    /// </summary>
    private void OnEnable()
    {
        mvmtAction.Enable();
    }

    /// <summary>
    /// Désactiver les inputs quand l'objet est désactivé
    /// </summary>
    private void OnDisable()
    {
        mvmtAction.Disable();
    }
}
