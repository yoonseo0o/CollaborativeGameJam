using UnityEngine;

public abstract class Structure : MonoBehaviour, Entity
{
    [Header("Property")]
    [SerializeField] private int pureCost;
    [SerializeField] private int hp; 
    public abstract void Ability();
    void Entity.Attacked(int damageAmount)
    {
        Debug.Log($"공격받는 중 {damageAmount}");
        hp -=damageAmount;
        if(hp < 0) ((Entity)this).Dead();
    }
    void Entity.Dead() 
    {
        Destroy(gameObject);
    }
    
}
