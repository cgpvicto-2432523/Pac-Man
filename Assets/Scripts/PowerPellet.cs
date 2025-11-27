using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f;

    public override void Manger()
    {
        GameManager.Instance.MangerPowerPelette(this);
    }
}
