using UnityEngine;
using static UnityEngine.UI.Image;

public class Flashlight : MonoBehaviour
{
    public bool IsActive;
    [SerializeField] private float spotRange;
    [SerializeField] private float spotAngle;

    [SerializeField] private int maxDamageAmount; 
    [SerializeField] private float attackCoolingTime;

    private int monsterLayer;

    private void Awake()
    {
        IsActive = false;
        monsterLayer = 1<<6; 
    }
    public void Init(int damageAmount, float coolTime=0.2f )
    {
        this.maxDamageAmount = damageAmount;
        this.attackCoolingTime = coolTime;
    }
    private void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, spotRange, monsterLayer);

        foreach (var hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            if (angle < spotAngle / 2f)
            { 
                float distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);
                float ratio = Mathf.Clamp01((spotRange - distanceToTarget) / spotRange);
                int thisDamageAmount = Mathf.RoundToInt(ratio * maxDamageAmount);


                Debug.Log($"{(spotRange - distanceToTarget)}/{spotRange }*{maxDamageAmount} = {thisDamageAmount}");
                hit.GetComponent<Entity>().Attacked(thisDamageAmount);
            }
        }
    }
    public void TurnOn(bool IsOn)
    {
        IsActive = IsOn;
        if (IsActive)
        {
            InvokeRepeating("Attack", attackCoolingTime, attackCoolingTime);

        }
        else
        {
            CancelInvoke("Attack");
        }

    }
}
