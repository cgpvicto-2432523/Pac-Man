using UnityEngine;

/// <summary>
/// Comportement du fantôme en mode patrouille (Scatter)
/// Le fantôme se déplace aléatoirement dans le labyrinthe
/// </summary>
public class GhostScatter : GhostBehavior
{
    /// <summary>
    /// Quand le mode Scatter se termine, activer le mode Chase (poursuite)
    /// </summary>
    private void OnDisable()
    {
        ghost.chase.Enable();
    }

    /// <summary>
    /// À chaque intersection, choisir une direction aléatoire
    /// en évitant de faire démi tour
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Ne rien faire si pas à une intersection, script désactivé, ou fantôme vulnérable
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            // Choisir un index aléatoire parmi les directions disponibles
            int index = Random.Range(0, node.availableDirections.Count);

            // Éviter de faire demi-tour 
            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -ghost.movement.direction)
            {
                index++;  // Passer à la direction suivante

                // Si on dépasse le tableau, revenir au début
                if (index >= node.availableDirections.Count) {
                    index = 0;
                }
            }

            ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }

}
