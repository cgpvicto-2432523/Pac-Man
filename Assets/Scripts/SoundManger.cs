using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip sonCollecte;
    public AudioClip sonDefaite;
    public AudioClip MangeFantomes;

    private AudioSource audioSource;

    void Awake()
    {
        // Garde une seule instance du SoundManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Fonction pour jouer le son de collecte
    public void JouerSonCollecte()
    {
        audioSource.PlayOneShot(sonCollecte);
    }

    public void JouerSonMange()
    {
        audioSource.PlayOneShot(sonCollecte);
    }
    // Fonction pour jouer le son de défaite
    public void JouerSonDefaite()
    {
        audioSource.PlayOneShot(sonDefaite);
    }
}