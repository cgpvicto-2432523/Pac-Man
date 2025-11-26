using UnityEngine;

public class pellet : MonoBehaviour
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
    /// 
    /// </summary>
    /// <param name="collision"></param>

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
                GameManager.Instance.ActiverVulnerabilite();
            }

            // Détruire le pellet
            Destroy(gameObject);
        }
    }


}
