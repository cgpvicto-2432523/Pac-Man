using UnityEngine;

/// <summary>
/// Comportement du fantome en mode vulnérable
/// </summary>
public class GhostFrightened : GhostBehavior
{
    // Sprites du fantôme
    public SpriteRenderer body;   // Corps normal du fantôme
    public SpriteRenderer eyes;   // Yeux du fantôme
    public SpriteRenderer blue;   // Sprite de vulnérabilité
    public SpriteRenderer white;  

    private bool eaten; //verifie si le fantome a été mangé

    /// <summary>
    /// Active le mode vulnérable du fantome
    /// </summary>
    /// <param name="duration"></param>
    public override void Enable(float duration)
    {
        base.Enable(duration);

        //cache le corps et les yeux normaux
        body.enabled = false;
        eyes.enabled = false;

        //Afficher le sprite bleu de vulnérabilité
        blue.enabled = true;
        white.enabled = false;

        //Clignotement des yeux a la moitié du temps de vulnérabilités
        Invoke("Flash", duration / 2f);
    }

    /// <summary>
    /// Désactive le mode vulnérable
    /// </summary>
    public override void Disable()
    {
        base.Disable(); // Désactive le comportement

        // Remettre l'apparence normale
        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    /// <summary>
    /// fantome mangé par pacman
    /// </summary>
    private void Eaten()
    {
        eaten = true;
        ghost.SetPosition(ghost.home.inside.position); // remetre le fantome dans la maison
        ghost.home.Enable(duration);//il y restera un moment

        // Afficher seulement les yeux 
        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    /// <summary>
    /// Fait clignoter le fantôme en blanc pour avertir que le mode va finir
    /// </summary>
    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    /// <summary>
    /// Appelé quand le script s'active
    /// Initialise l'animation et ralentit le fantôme
    /// </summary>
    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Restart(); //dérarre l'animation de vulnérabilités
        ghost.movement.speedMultiplier = 0.5f; //diminue la vitesse de deplacemnt du fantome
        eaten = false;
    }

    /// <summary>
    /// revenir a la normale
    /// </summary>
    private void OnDisable()
    {
        ghost.movement.speedMultiplier = 1f;
        eaten = false;
    }

    /// <summary>
    /// À chaque intersection, choisit la direction qui éloigne le plus de Pacman
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue; // Distance maximale (on veut la plus grande)

            // Trouver la direction qui éloigne LE PLUS de Pacman
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

    /// <summary>
    /// Gère la collision avec Pacman
    /// Si en mode vulnérable, le fantôme est mangé
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled) {
                Eaten();
            }
        }
    }

}
