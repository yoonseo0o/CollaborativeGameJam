using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Flashlight : MonoBehaviour
{
    [Header("property")]
    [SerializeField] private bool IsOn;
    [Header(" ")]
    [SerializeField] private float brightness;
    [SerializeField] private float distance;  
    //[SerializeField] private float range;  
    [Header(" ")] 
    [SerializeField] private float attackDelay=0.2f;
    private List<Transform> monsters;  
    private void Awake()
    {
        IsOn = gameObject.activeSelf;
        distance = Vector3.Distance(Camera.main.transform.position,
            transform.GetChild(0).position);
        monsters = new List<Transform>();
        //range = transform.localScale.x/2;
    }

    public void Turn( )
    {
        IsOn = !IsOn;
        if (IsOn)
        {
            gameObject.SetActive(true);
            InvokeRepeating("Attack", attackDelay, attackDelay);
        }
        else
        {
            CancelInvoke("Attack");
            gameObject.SetActive(false);
        }
    }
    public void TurnOn(bool b)
    {
        IsOn = b;
        if (b)
        {
            gameObject.SetActive(true);
            InvokeRepeating("Attack", attackDelay, attackDelay);
        }
        else
        {
            CancelInvoke("Attack");
            gameObject.SetActive(false);
        }
    }
    private void Attack()
    {
        if (!IsOn)
            return;
        Vector3 thisPos = Camera.main.transform.position;
            //Debug.Log(monsters.Count);
            List<Transform> deadMonsters = new List<Transform>();
        foreach (var m in monsters)
        { 
            float distanceToTarget = Vector3.Distance(transform.position, m.transform.position);
            float ratio = Mathf.Clamp01((distance - distanceToTarget) / distance);
            int damageAmount = Mathf.RoundToInt(ratio * brightness);

            // 공격 후, 죽었으면 삭제
            if (m.GetComponent<Entity>().Attacked(damageAmount))
                deadMonsters.Add(m);
        }
        foreach (var m in deadMonsters)
            monsters.Remove(m); 
    }
    public void RangeIncrease()
    {

        transform.localScale += new Vector3(1,0,1);
        Debug.Log($"범위 증가 : {transform.localScale.x}");
    }
    public void BrightnessIncrease()
    {
        brightness++;
        Debug.Log($"밝기 증가 : {brightness}");
    }
    private void OnTriggerEnter(Collider other)
    {
        monsters.Add(other.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        monsters.Remove(other.transform);
    }
}
