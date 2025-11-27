using UnityEngine;


/// <summary>
/// Gère le contrôle et l'état de Pacman
/// Contrôles au clavier, rotation visuelle, et animation de mort
/// </summary>
public class Pacman : MonoBehaviour
{

    private AnimatedSprite deathSequence;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private Movement movement;


    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            movement.DefinirLaDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            movement.DefinirLaDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            movement.DefinirLaDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            movement.DefinirLaDirection(Vector2.right);
        }

        // Rotation du packman afin qu'il regarde le direction
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetDeLEtat()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;

        if (deathSequence != null) 
        {
            deathSequence.enabled = false;
        }

        movement.ResetDeLEtat();
        gameObject.SetActive(true);
    }
}
