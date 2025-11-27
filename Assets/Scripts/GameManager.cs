using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public Text gameOverText;
    public Text scoreText;
    public Text livesText;
    public int round = 1;

    public int score = 0;
    public int lives = 3;

    private int ghostMultiplier = 1;

    public float ghostSpeedIncrease = 0.5f;
    public float pacmanSpeedIncrease = 0.3f;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        NouveauJeu();
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            NouveauJeu();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void NouveauJeu()
    {
        AJustementDuScore(0);
        AjustementDesVies(3);
        round = 1;
        NouveauNiveau();
    }

    /// <summary>
    /// Nouveau round
    /// </summary>
    private void NouveauNiveau()
    {
        gameOverText.enabled = false;

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }


        if (round > 1)
        {
            AugmenterLaDifficulter();
        }

        ResetDeLEtat();
    }


    /// <summary>
    /// Augmente la vitesse en fonction du niveau
    /// </summary>
    private void AugmenterLaDifficulter()
    {

        // Augmenter vitesse des fantômes
        foreach (Ghost ghost in ghosts)
        {
            ghost.movement.speed += ghostSpeedIncrease;
        }

        // Augmenter vitesse de Pacman
        pacman.GetComponent<Movement>().speed += pacmanSpeedIncrease;
    }


    /// <summary>
    ///  restate a l'état de départ des pacmans et fantomes.
    /// </summary>
    private void ResetDeLEtat()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetDeLEtat();
        }

        pacman.ResetDeLEtat();
    }

    /// <summary>
    /// 
    /// </summary>
    private void FinDePartie()
    {
        gameOverText.enabled = true;
        if (SoundManager.instance != null)
        {
            SoundManager.instance.JouerSonDefaite();
        }
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lives"></param>
    private void AjustementDesVies(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="score"></param>
    private void AJustementDuScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    /// <summary>
    /// 
    /// </summary>
    public void PacmanMange()
    {
        AjustementDesVies(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(ResetDeLEtat), 3f);
        }
        else
        {
            FinDePartie();
        }
    }

    /// <summary>
    /// un fantome mangé
    /// </summary>
    /// <param name="ghost"></param>
    public void FantomeMange(Ghost ghost)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.JouerSonMange();
        }
        int points = ghost.points * ghostMultiplier;
        AJustementDuScore(score + points);

        ghostMultiplier++;
    }

    /// <summary>
    /// Un pellet normal mangé
    /// </summary>
    /// <param name="pellet"></param>
    public void PelletNormalManger(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        AJustementDuScore(score + pellet.points);
        if (!VerifierExistancePelette())
        {
            pacman.gameObject.SetActive(false);
            round++;
            Invoke(nameof(NouveauNiveau), 3f);
        }
    }

    /// <summary>
    /// Un powerpellet mangé
    /// </summary>
    /// <param name="pellet"></param>
    public void MangerPowerPelette(PowerPellet pellet)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.JouerSonCollecte();
        }
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletNormalManger(pellet);
        CancelInvoke(nameof(ReinitialiserLeMultiplicateurDeVitesse));
        Invoke(nameof(ReinitialiserLeMultiplicateurDeVitesse), pellet.duration);
    }


    /// <summary>
    /// Verifier si il reste au moins une pelette
    /// </summary>
    /// <returns></returns>
    private bool VerifierExistancePelette()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// Reset le multiplicateur de vitesse
    /// </summary>
    private void ReinitialiserLeMultiplicateurDeVitesse()
    {
        ghostMultiplier = 1;
    }
}