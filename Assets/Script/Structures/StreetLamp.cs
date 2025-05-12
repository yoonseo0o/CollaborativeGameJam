using UnityEngine;

public class StreetLamp : Structure
{
    [Header("ability")]
    private ParticleSystem lightEffect;
    private void Awake()
    {
        lightEffect = GetComponent<ParticleSystem>();
    }
    public override void Ability()
    {
        if( lightEffect != null ) { 
        lightEffect.Play();}
    }
}
