using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton pattern pour un accès facile
    public static AudioManager Instance;

    
    public AudioClip SonPieces;
    public AudioClip SonBooster;
    public AudioClip SonMort;

    private AudioSource audioSource; // L'AudioSource principal pour les SFX

    void Awake()
    {
        // Assure qu'il n'y ait qu'une seule instance de AudioManager
        if (Instance == null)
        {
            Instance = this;
            // Optionnel: Ne pas détruire l'AudioManager si vous changez de scène
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }

        // Récupère l'AudioSource pour les SFX (le premier que vous avez ajouté)
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        // Utilise PlayOneShot pour jouer un clip sans interrompre le son en cours
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}