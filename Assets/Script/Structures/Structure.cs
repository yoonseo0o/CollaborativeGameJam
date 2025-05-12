using UnityEngine;

public abstract class Structure : MonoBehaviour, Entity
{
    [Header("Property")]
    public int pureCost;
    [SerializeField] private int hp;

    [Header("UI content")]
    public string name;
    public string description;
    //public Sprite img;
    public abstract void Ability();
    bool Entity.Attacked(int damageAmount)
    {
        Debug.Log($"공격받는 중 {damageAmount}");
        hp -=damageAmount;
        if (hp < 0) { 
            ((Entity)this).Dead();
            return true;
        }
        else 
            return false;
    }
    void Entity.Dead() 
    {
        Destroy(gameObject);
    }
    
}
