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
        // 강화창 출력
        canvas.SetActive(true);
    }
    public void RangeStrengthen(float value)
    {
        // 범위 강화
        Debug.Log("범위 강화");
        range = value;
        rangeTrf.localScale = Vector3.one * range;
    }
    public void IntensityStrengthen()
    {
        // 밝기 강화
        Debug.Log("밝기 강화");
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
