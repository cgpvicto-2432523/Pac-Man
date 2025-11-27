using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Garde en mémoire si on a déjà une musique qui joue
    private static MusicManager instance;

    void Awake()
    {
        Debug.Log("MusicManager instancié dans la scène : " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Check si y'a déjà une musique dans le jeu
        if (instance != null && instance != this)
        {
            // Y'en a déjà une, donc on détruit celle-ci pour pas avoir 2 musiques
            Destroy(gameObject);
            return;
        }

        // Sinon c'est la première, alors on la garde
        instance = this;

        // Dit à Unity de pas supprimer cet objet quand on change de scène
        DontDestroyOnLoad(gameObject);
    }
}