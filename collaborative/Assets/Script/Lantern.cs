using System.Collections;
using UnityEngine;

public class Lantern : MonoBehaviour, Entity
{
    private int hp=25;
    private int range;
    private int intensity;
    [SerializeField] private int playerHealTime;

    void Entity.Attacked(int damageAmount)
    {
        Debug.Log("아야 : 렌턴");
        hp -= damageAmount;
        if (hp <= 0)
        {
            ((Entity)this).Dead();
        }
    }
    void Entity.Dead()
    {
        GameManager.Instance.GameOver("lantern");
    }
    public void interaction()
    {
        // 강화창 출력
    }
    private void RangeStrengthen()
    {
        // 범위 강화
    }
    private void IntensityStrengthen()
    {
        // 밝기 강화
    }
    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
            Debug.Log("player enter");
            other.GetComponent<Player>().InvokeRepeating("LanternHeal", playerHealTime, playerHealTime);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player exit");
            other.GetComponent<Player>().CancelInvoke("LanternHeal");
        }
    }
}
