using UnityEngine;

/// <summary>
/// Représente une pelette mangeable par Pacman
/// </summary>
public class Pellet : MonoBehaviour
{
    public int points = 10;

    /// <summary>
    /// Appelé quand Pacman mange la pelette
    /// </summary>
    public virtual void Eat()
    {
        GameManager.Instance.PelletEaten(this);
    }

    /// <summary>
    /// Détecte quand Pacman touche la pelette
    /// </summary>
    /// <param name="other">L'objet qui touche la pastille</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman")) {
            Eat();
        }
    }

}
