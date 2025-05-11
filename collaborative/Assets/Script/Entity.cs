using UnityEngine;

public interface Entity
{
    public abstract void Attacked(int damageAmount);

    public abstract void Dead();
}