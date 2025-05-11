using UnityEngine;

public class StreetLamp : Structure
{
    [Header("ability")]
    [SerializeField] private ParticleSystem lightEffect;
    public override void Ability()
    {
        lightEffect.Play();
    }
}
