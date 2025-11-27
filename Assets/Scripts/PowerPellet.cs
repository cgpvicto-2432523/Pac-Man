using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f;

    public override void Eat()
    {
        GameManager.Instance.PowerPelletEaten(this);
    }
}
