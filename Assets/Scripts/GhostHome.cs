using System.Collections;
using UnityEngine;

/// <summary>
/// Comportement du fantôme dans sa base (maison)
/// Gère le rebond dans la base et la sortie animée
/// </summary>
public class GhostHome : GhostBehavior
{
    public Transform inside; //point de départ de sortie
    public Transform outside; //point d'arrivée de sortie

    /// <summary>
    /// Quand le comportement s'active, arrêter toutes les animations en cours
    /// </summary>
    private void OnEnable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Quand le comportement se désactive, lancer l'animation de sortie de la base
    /// </summary>
    private void OnDisable()
    {
        // Vérifier que l'objet est actif pour éviter erreur si détruit
        if (gameObject.activeInHierarchy) {
            StartCoroutine(ExitTransition());
        }
    }


    /// <summary>
    /// Fait rebondir le fantôme dans la base quand il touche un mur
    /// Crée l'effet de va-et-vient dans la maison
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si en mode Home et touche un obstacle, inverser la direction
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            ghost.movement.SetDirection(-ghost.movement.direction);
        }
    }

    /// <summary>
    /// Animation de sortie de la base en 2 étapes
    /// 1. Aller au point "inside" (entrée de la base)
    /// 2. Sortir au point "outside" (sortie de la base)
    /// </summary>
    private IEnumerator ExitTransition()
    {
        // Désactiver le mouvement normal pendant l'animation manuelle
        ghost.movement.SetDirection(Vector2.up, true);
        ghost.movement.rb.isKinematic = true;   // Désactiver la physique
        ghost.movement.enabled = false;          // Désactiver le script de mouvement

        Vector3 position = transform.position;   // Position actuelle
        float duration = 0.5f;                   // Durée de chaque animation (0.5 secondes)
        float elapsed = 0f;                      // Temps écoulé

        // ÉTAPE 1: Animer vers le point d'entrée "inside"
        while (elapsed < duration)
        {
            // Interpolation linéaire entre position actuelle et "inside"
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;  // Attendre la prochaine frame
        }

        elapsed = 0f;  // Réinitialiser le timer

        // ÉTAPE 2: Animer la sortie de "inside" vers "outside"
        while (elapsed < duration)
        {
            // Interpolation linéaire entre "inside" et "outside"
            ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;  // Attendre la prochaine frame
        }

        // ÉTAPE 3: Sortie terminée, réactiver le mouvement normal
        // Choisir aléatoirement gauche ou droite
        ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
        ghost.movement.rb.isKinematic = false;   // Réactiver la physique
        ghost.movement.enabled = true;           // Réactiver le mouvement
    }
}


