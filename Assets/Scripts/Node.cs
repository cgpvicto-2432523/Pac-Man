using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Représente une intersection dans le labyrinthe
/// Détecte automatiquement les directions disponibles (sans mur) au démarrage
/// Utilisé par les fantômes pour choisir leur direction aux carrefours
/// </summary>
public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer; //layer du mur
    public List<Vector2> availableDirections = new(); //Liste des directions disponobles

    /// <summary>
    /// Au démarrage, vérifie toutes les directions pour détecter lesquelles sont libres
    /// </summary>
    private void Start()
    {
        availableDirections.Clear(); //vide la liste de direction

        // Tester chaque direction cardinale
        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    /// <summary>
    /// Vérifie si une direction est libre (sans mur)
    /// Si libre, l'ajoute à la liste des directions disponibles
    /// </summary>
    /// <param name="direction">Direction à tester</param>
    private void CheckAvailableDirection(Vector2 direction)
    {
        // Lance un BoxCast pour détecter un obstacle
        // - Position: position du Node (intersection)
        // - Taille: 0.5×0.5
        // - Angle: 0°
        // - Direction: direction à tester
        // - Distance: 1 unité
        // - Layer: seulement les obstacles
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);

        // If no collider is hit then there is no obstacle in that direction
        if (hit.collider == null) {
            availableDirections.Add(direction);
        }
    }

}
