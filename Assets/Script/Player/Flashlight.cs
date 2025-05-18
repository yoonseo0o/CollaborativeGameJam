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
    [SerializeField] private float range;  
    [Header(" ")] 
    [SerializeField] private float attackDelay=0.2f;
    private List<Transform> monsters;

    [SerializeField] private GameObject light;
    private int rangeLevel;
    private int brightnessLevel;
    private int distanceLevel;
    private void Awake()
    {
        IsOn = gameObject.activeSelf;
        
        monsters = new List<Transform>();
            
        SetBrightnessLevel(0);
        SetDistanceLevel(0);
        SetRangeLevel(0); 
    }
    public void Raise()
    {
        gameObject.SetActive(true);
        TurnOn(false);
    }
    public void Lower()
    {
        TurnOn(false); 
        gameObject.SetActive(false); 
    }
    public void Turn()
    {
        IsOn = !IsOn;
        TurnOn(IsOn); 
    }
    public void TurnOn(bool b)
    {
        IsOn = b;
        if (b)
        {
            //gameObject.SetActive(true);
            light.SetActive(true);
            InvokeRepeating("Attack", attackDelay, attackDelay);
        }
        else
        {
            CancelInvoke("Attack");
            light.SetActive(false);
            //gameObject.SetActive(false);
        }
    }
    private void Attack()
    {
        if (!IsOn)
            return;
        Vector3 thisPos = Camera.main.transform.position; 
        List<Transform> deadMonsters = new List<Transform>();
        foreach (var m in monsters)
        { 
            if( m ==null ) continue;
            Debug.Log(m.name);
            float distanceToTarget = Vector3.Distance(transform.position, m.transform.position);
            float ratio = Mathf.Clamp01((distance - distanceToTarget) / distance);
            int damageAmount = Mathf.RoundToInt(ratio * brightness);

            // 공격 후, 죽었으면 삭제
            if (m.GetComponent<Entity>().Attacked(damageAmount))
                deadMonsters.Add(m);
        }
        foreach (var m in deadMonsters)
            monsters.Remove(m);
        deadMonsters.Clear();
    }
    public void SetRangeLevel(int level)
    {
        rangeLevel = level;
        range = GameManager.Instance.flashData.rangeDatas[rangeLevel].value;
        light.transform.localScale = new Vector3(range, light.transform.localScale.y, range);
        Debug.Log($"범위 : {transform.localScale.x}");
    }
    public void SetBrightnessLevel(int level)
    {
        brightnessLevel = level;
        brightness = GameManager.Instance.flashData.brightnessDatas[brightnessLevel].value; 
        Debug.Log($"밝기 : {brightness}");
    }
    public void SetDistanceLevel(int level)
    {
        distanceLevel = level;
        distance = GameManager.Instance.flashData.distanceDatas[distanceLevel].value;
        light.transform.localPosition = new Vector3(0, 0, (distance - 0.5f) / 2 + 0.5f);
        light.transform.localScale = new Vector3(light.transform.localScale.x, (distance - 0.5f) / 2, light.transform.localScale.z);

        Debug.Log($"사거리 : {distance}");
    }
    public void UpgradeRangeLevel()
    {
        if(rangeLevel>= GameManager.Instance.flashData.rangeDatas.Length-1)
        {
            Debug.Log($"최대 레벨 도달");
            return;
        }
        // 손전등 데이터의 필요 코스트가 없다면 
        if (GameManager.Instance.flashData.rangeDatas[rangeLevel].pureCost >
            GameManager.Instance.PureSystem.pure)
        {
            Debug.Log($"필요 동심 부족 {GameManager.Instance.flashData.rangeDatas[rangeLevel].pureCost-GameManager.Instance.PureSystem.pure} 동심이 더 필요합니다");
            return;
        }
        else
        {
            GameManager.Instance.PureSystem.LosePure(GameManager.Instance.flashData.rangeDatas[rangeLevel].pureCost);
            SetRangeLevel(++rangeLevel);
            GameManager.Instance.lanternTrf.GetComponent<Lantern>().CanvasSetActive(false);

        }

    }
    public void UpgradeBrightnessLevel()
    {

        if (brightnessLevel >= GameManager.Instance.flashData.brightnessDatas.Length - 1)
        {
            Debug.Log($"최대 레벨 도달");
            return;
        }
        // 손전등 데이터의 필요 코스트가 없다면 
        if (GameManager.Instance.flashData.brightnessDatas[brightnessLevel].pureCost >
            GameManager.Instance.PureSystem.pure)
            return;
        else
        {
            GameManager.Instance.PureSystem.LosePure(GameManager.Instance.flashData.brightnessDatas[brightnessLevel].pureCost);
            SetBrightnessLevel(++brightnessLevel);
            GameManager.Instance.lanternTrf.GetComponent<Lantern>().CanvasSetActive(false);

        }
    }
    public void UpgradeDistanceLevel()
    {

        if (distanceLevel >= GameManager.Instance.flashData.distanceDatas.Length - 1)
        {
            Debug.Log($"최대 레벨 도달");
            return;
        }
        // 손전등 데이터의 필요 코스트가 없다면 
        if (GameManager.Instance.flashData.distanceDatas[distanceLevel].pureCost >
            GameManager.Instance.PureSystem.pure)
            return;
        else
        {
            GameManager.Instance.PureSystem.LosePure(GameManager.Instance.flashData.distanceDatas[distanceLevel].pureCost);
            SetDistanceLevel(++distanceLevel);
            GameManager.Instance.lanternTrf.GetComponent<Lantern>().CanvasSetActive(false);

        }
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
