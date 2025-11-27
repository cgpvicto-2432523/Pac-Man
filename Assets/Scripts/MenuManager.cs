using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère le menu principal et la navigation vers le jeu
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Charge la scène Level1 quand on clique sur Play
    /// </summary>
    public void StartJeu()
    {
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// Quitte le jeu 
    /// </summary>
    public void Quitter()
    {
        Application.Quit();
    }
}