using UnityEngine;


public class GhostEyes : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    // Composant qui affiche le sprite des yeux
    private SpriteRenderer spriteRenderer;

    // Référence au script de mouvement du fantôme 
    private Movement movement;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<Movement>(); ///Mouvement du fantome
    }

    /// <summary>
    /// PErmet ai yeix du fantome de regarder dans la direction où il va. 
    /// utile pour l'annimation
    /// </summary>
    private void Update()
    {
        if (movement.direction == Vector2.up) {
            spriteRenderer.sprite = up;
        }
        else if (movement.direction == Vector2.down) {
            spriteRenderer.sprite = down;
        }
        else if (movement.direction == Vector2.left) {
            spriteRenderer.sprite = left;
        }
        else if (movement.direction == Vector2.right) {
            spriteRenderer.sprite = right;
        }
    }

}
