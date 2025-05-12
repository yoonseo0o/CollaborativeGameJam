using System.Collections;
using UnityEngine;

public class Lantern : MonoBehaviour, Entity, Interaction
{
    private int hp=25;
    private float range;
    private int intensity;
    [SerializeField] private int playerHealTime;
    [SerializeField] private GameObject canvas;

    [SerializeField] private Transform rangeTrf;

    void Awake()
    {
        canvas.SetActive(false);
        range = rangeTrf.localScale.x; 
    }
    bool Entity.Attacked(int damageAmount)
    { 
        hp -= damageAmount;
        GameManager.Instance.UIManager.UpdateLanternHP(hp);
        if (hp <= 0)
        {
            ((Entity)this).Dead();
            return true;
        }
        else 
            return false;
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
    public void RangeStrengthen(float value)
    {
        // ���� ��ȭ
        Debug.Log("���� ��ȭ");
        range = value;
        rangeTrf.localScale = Vector3.one * range;
    }
    public void IntensityStrengthen()
    {
        // ��� ��ȭ
        Debug.Log("��� ��ȭ");
    }
    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
            //Debug.Log("player enter");
            other.GetComponent<Player>().InvokeRepeating("ModifyHealth", playerHealTime, playerHealTime);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("player exit");
            other.GetComponent<Player>().CancelInvoke("ModifyHealth");
        }
    }
}
