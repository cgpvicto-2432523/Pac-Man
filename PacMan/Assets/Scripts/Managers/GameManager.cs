using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Implementation de la logique de singleton appris lors de mon cours de prog2
    public static GameManager Instance;

    private int score = 0;
    public int vie = 3;
    public int niveau = 1;
    public float tempsVulnerabilite = 8f;

    public float tempsNiveau1 = 60f;
    public float tempsNiveau2 = 120f;

    private float temps; //Temps restant
    private int totalDeGains = 0; //Il s'agit du nombre de point blanc que pacman doit manger
    private int gainCollecte = 0;
    private float compteurTempsVulnérabilite;

    /// <summary>
    /// Initialisation du Singleton
    /// </summary>
    private void Awake()
    {
        // S'assurer qu'il n'y a qu'un seul GameManager, en utilisant notre singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CommencerLeJeu();
    }

    // Update is called once per frame
    void Update()
    {
        //Mise a jour du temps (le diminué)
        temps = temps - Time.deltaTime;

        if (temps <= 0)
        {
            PartieTerminer();
        }

        TempsDeVulnerabilite();
    }

    /// <summary>
    /// lancer le niveau 1 du jeu
    /// </summary>
    public void CommencerLeJeu()
    {
        //Nombre de pelettes collectionné a 0
        gainCollecte = 0;

        totalDeGains = GameObject.FindGameObjectsWithTag("pellet").Length;

        //Definition du temps en fonction du niveau dans le jeu
        if (niveau == 1)
        {
            temps = tempsNiveau1;
            SceneManager.LoadScene("Level1");
        }
        else if(niveau == 2)
        {
            temps = tempsNiveau2;
            SceneManager.LoadScene("Level2");
        }
        else
        {
            temps = 30f;
        }
        //Teste
        Debug.Log("Niveau " + niveau + " - Temps: " + temps + "s - Pellets: " + totalDeGains);
    }

    /// <summary>
    /// Ajouter des points
    /// </summary>
    /// <param name="points"></param>
    public void AjouterLeScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
    }


    /// <summary>
    /// Collectionner les "pelette"
    /// </summary>
    public void CollecterPellet()
    {
        gainCollecte++;
        Debug.Log ("Pellets collecés: "+  gainCollecte + "/" + totalDeGains);

        if (gainCollecte >= totalDeGains)
        {
            NiveauComplete();
        }
    }

    /// <summary>
    /// Perdre des points de vie
    /// </summary>
    public void PerdreUneVie()
    {
        vie--;
        if (vie <= 0)
        {
            PartieTerminer();
        }
    }


    /// <summary>
    /// Niveau terminé
    /// </summary>
    private void NiveauComplete()
    {
        Debug.Log("=== NIVEAU " + niveau + " TERMINÉ! ===");
        niveau++;

        if (niveau > 2)
        {
            Victoire();
        }
        else
        {
            Debug.Log("Passage au niveau 2!");
            // Plus tard: SceneManager.LoadScene("Level2");
        }
    }

    /// <summary>
    /// Victoire totale
    /// </summary>
    private void Victoire()
    {
        if (niveau == 1)
            niveau++;
        else
            niveau = 1;
    }

    /// <summary>
    /// Jeu perdu
    /// </summary>
    private void PartieTerminer()
    {
        Debug.Log("=== GAME OVER! Score final: " + score + " ===");
        // Plus tard: SceneManager.LoadScene("GameOver");
    }

    /// <summary>
    /// Obtenir le temps restant (pour l'UI)
    /// </summary>
    public float ObtenirLeTemps()
    {
        return temps;
    }


    /// <summary>
    /// Rendre les fantomes vulnérables
    /// </summary>

    public void ActiverVulnerabilite()
    {
        GameObject[] lesFantomes = GameObject.FindGameObjectsWithTag("enemies");

        foreach (GameObject f in lesFantomes)
        {
            DeplacementIntelligent fantome = f.GetComponent<DeplacementIntelligent>();
            if (fantome != null)
                fantome.ModifierLaVulnerabilite(true);
        }

        compteurTempsVulnérabilite = tempsVulnerabilite;
    }

    /// <summary>
    /// Gestion du temps de vulnerabilité des énémies
    /// </summary>
    public void TempsDeVulnerabilite()
    {
        if (compteurTempsVulnérabilite > 0)
        {
            compteurTempsVulnérabilite -= Time.deltaTime;

            if (compteurTempsVulnérabilite <= 0)
            {
                ResetVulnerabilite();
            }
        }
    }


    /// <summary>
    /// Quand le timer atteint zéro les fantomes redeviennent invulnérables
    /// </summary>
    private void ResetVulnerabilite()
    {
        GameObject[] lesFantomes = GameObject.FindGameObjectsWithTag("enemies");

        foreach (GameObject f in lesFantomes)
        {
            DeplacementIntelligent fantome = f.GetComponent<DeplacementIntelligent>();
            if (fantome != null)
                fantome.ModifierLaVulnerabilite(false);
        }

        compteurTempsVulnérabilite = 0;
    }
}

