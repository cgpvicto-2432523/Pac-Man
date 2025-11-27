using UnityEngine;

/// <summary>
/// fantome en mode chaseur
/// </summary>
public class GhostChase : GhostBehavior
{
    private void OnDisable()
    {
        ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Ne rien faire tant que le fantôme est vulnérable
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero; //contiendra la meilleur direction
            float minDistance = float.MaxValue; //variable qui contiendra la distance minimale, intialement initialise avec un gros nombre

            // Trouver la direction disponible qui rapproche le plus de Pacman
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // Si la distance dans cette direction est inférieure à la distance
                // minimale actuelle, alors cette direction devient la nouvelle plus proche
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            ghost.movement.DefinirLaDirection(direction);
        }
    }

}
