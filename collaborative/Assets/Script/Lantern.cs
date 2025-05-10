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
        Debug.Log("�ƾ� : ����");
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
        // ��ȭâ ���
    }
    private void RangeStrengthen()
    {
        // ���� ��ȭ
    }
    private void IntensityStrengthen()
    {
        // ��� ��ȭ
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
