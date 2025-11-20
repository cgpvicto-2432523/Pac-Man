using UnityEngine;

public class Pellet : MonoBehaviour
{
 
    public int points = 2; //Combien de point, le pacman gagne quand il consomme le pellet
    public bool estUnBooster = false; // S'agit-il d'un booster

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Détection de collision avec le joueur
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifier si c'est le joueur qui touche le pellet
        if (collision.CompareTag("Player"))
        {
            // Ajouter les points
            GameManager.Instance.AjouterLeScore(points);

            // Incrémenter le compteur de pellets collectés
            GameManager.Instance.CollecterPellet();

            // Si c'est un booster, activer le mode vulnérable des fantômes
            if (estUnBooster)
            {
                ActiverModeBooster();
            }

            // Détruire le pellet
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// Active le mode où les fantômes deviennent vulnérables
    /// </summary>
    private void ActiverModeBooster()
    {
        Debug.Log("POWER PELLET ACTIVÉ!");

        // Trouver tous les fantômes et les rendre vulnérables
        // (On fera ça plus tard quand les fantômes existeront)
    }
}
