using System.Collections;
using UnityEngine;

public class Lantern : MonoBehaviour, Entity, Interaction
{
    private int hp=25;
    private int range;
    private int intensity;
    [SerializeField] private int playerHealTime;
    [SerializeField] private GameObject canvas;

    void Awake()
    {
        canvas.SetActive(false);
    }
    void Entity.Attacked(int damageAmount)
    { 
        hp -= damageAmount;
        GameManager.Instance.UIManager.UpdateLanternHP(hp);
        if (hp <= 0)
        {
            ((Entity)this).Dead();
        }
    }
    void Entity.Dead()
    {
        GameManager.Instance.GameOver("lantern");
    }
    void Interaction.interaction()
    {
        // ��ȭâ ���
        canvas.SetActive(true);
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
            //Debug.Log("player enter");
            other.GetComponent<Player>().InvokeRepeating("LanternHeal", playerHealTime, playerHealTime);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("player exit");
            other.GetComponent<Player>().CancelInvoke("LanternHeal");
        }
    }
}
