using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float vitesse = 5f;
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

        if (direction != Vector2.zero)
        {
            directionActuelle = direction.normalized;
            TournerSelonDirection(directionActuelle);
        }
    }

    /// <summary>
    /// Modification de la physique du joueur
    /// </summary>
    private void FixedUpdate()
    {
        Vector2 deplacement = rb.position + directionActuelle * vitesse * Time.fixedDeltaTime;
        rb.MovePosition(deplacement);
    }

    /// <summary>
    /// Oriente Pac-Man selon la direction.
    /// source: https://docs.unity3d.com/ScriptReference/Quaternion.Euler.html pour la méthode Quaternion.Euler()
    /// </summary>
    private void TournerSelonDirection(Vector2 direction)
    {
        if (direction == Vector2.up)
            transform.rotation = Quaternion.Euler(0, 0, 90);

        else if (direction == Vector2.down)
            transform.rotation = Quaternion.Euler(0, 0, -90);

        else if (direction == Vector2.left)
            transform.rotation = Quaternion.Euler(0, 180, 0);  

        else if (direction == Vector2.right)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    /// <summary>
    /// Détection de collision avec les fantômes
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {

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
