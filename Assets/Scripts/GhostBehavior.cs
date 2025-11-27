using UnityEngine;

/// <summary>
/// classe de gestion des comportements des fantomes
/// </summary>
public abstract class GhostBehavior : MonoBehaviour
{
    public Ghost ghost { get; private set; } //le fantome
    public float duration; //duree de son comportement

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        ghost = GetComponent<Ghost>();
    }

    /// <summary>
    /// Active le comportement avec sa durée par défaut
    /// </summary>
    public void Enable()
    {
        Enable(duration);
    }

    /// <summary>
    /// Active le comportement pour une durée spécifique
    /// </summary>
    /// <param name="duration"></param>
    public virtual void Enable(float duration)
    {
        enabled = true;

        CancelInvoke(); //Anuler tous les timers précédents
        Invoke("Disable", duration);
    }

    /// <summary>
    /// Désactive le comportement
    /// </summary>
    public virtual void Disable()
    {
        enabled = false;

        CancelInvoke();  // Annule le timer de désactivation automatique
    }

}
