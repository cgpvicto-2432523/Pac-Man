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
        NewGame();
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        round = 1;
        NewRound();
    }

    /// <summary>
    /// Nouveau round
    /// </summary>
    private void NewRound()
    {
        gameOverText.enabled = false;

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }


        if (round > 1)
        {
            IncreaseDifficulty();
        }

        ResetState();
    }


    /// <summary>
    /// Augmente la vitesse en fonction du niveau
    /// </summary>
    private void IncreaseDifficulty()
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
    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    private void GameOver()
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

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();

        SetLives(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            GameOver();
        }
    }

    /// <summary>
    /// un fantome mangé
    /// </summary>
    /// <param name="ghost"></param>
    public void GhostEaten(Ghost ghost)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.JouerSonMange();
        }
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);

        ghostMultiplier++;
    }

    /// <summary>
    /// Un pellet normal mangé
    /// </summary>
    /// <param name="pellet"></param>
    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(score + pellet.points);
        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            round++;
            Invoke(nameof(NewRound), 3f);
        }
    }

    /// <summary>
    /// Un powerpellet mangé
    /// </summary>
    /// <param name="pellet"></param>
    public void PowerPelletEaten(PowerPellet pellet)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.JouerSonCollecte();
        }
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }


    /// <summary>
    /// Verifier si il reste au moins une pelette
    /// </summary>
    /// <returns></returns>
    private bool HasRemainingPellets()
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
    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }
}