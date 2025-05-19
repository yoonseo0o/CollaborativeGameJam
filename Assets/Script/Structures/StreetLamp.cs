using System.Collections.Generic;
using UnityEngine;

public class StreetLamp : Structure/*,Interaction*/
{
    [Header("ability")]
    [SerializeField] private Transform light;
    [SerializeField] private GameObject lightEffect;

    [Header("Property")]
    private int power;
    private int brightness;
    public int chargingLimit;
    public int chargingValuePerSec;
    private int powerConsumptionAmount = 1; // 전력소비량

    private StreetLampData data;
    private float attackInterval = 0.25f;
    private List<Entity> monsters;
    private bool IsDischarge; // 방전됐는지
    private bool IsAttack;

    private int monsterLayer = 6;
    static int id;
    int myId;
    private void Awake()
    {
        IsDischarge = false;
        IsAttack = false;
        data = GameManager.Instance.StreetLampManager.data;
        monsters = new List<Entity>();
        SetRangeLevel(GameManager.Instance.StreetLampManager.rangeLevel);
        SetBrightnessLevel(GameManager.Instance.StreetLampManager.brightnessLevel);
        SetPowerLevel(GameManager.Instance.StreetLampManager.powerLevel);
        power = chargingLimit;
        luminescence(true);
        myId = ++id;
    }
    private void OnDestroy()
    {
        if (IsAttack)
        {
            CancelInvoke("Attack");
        }
        GameManager.Instance.StreetLampManager.streetLamps.Remove(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != monsterLayer)
        {
            return;
        }
        Debug.Log($"{myId} : enter " + other.name);
        monsters.Add(other.GetComponent<Entity>());
        if (!IsDischarge && !IsAttack)
        {
            IsAttack = true;
            InvokeRepeating("Attack", attackInterval, attackInterval);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name + ":" + other.gameObject.layer + "==" + monsterLayer + "?" + (other.gameObject.layer == monsterLayer));
        if (other.gameObject.layer != monsterLayer)
        {
            Debug.Log("같은 레이엉 아니야");
            return;
        }
        Debug.Log($"{myId} : exit " + (other.name));
        monsters.Remove(other.GetComponent<Entity>());
        if (monsters.Count <= 0)
        {
            IsAttack = false;
            CancelInvoke("Attack");
        }
    }
    public override void Ability()
    {

    }
    /*void Interaction.interaction()
    { 

    }*/
    void Attack()
    {
        Debug.Log($"{myId} : monster count {monsters.Count}");
        List<Entity> deadMonsters = new List<Entity>();
        foreach (Entity m in monsters)
        {
            if (m == null)
            {
                Debug.Log($"{m}이 null -> deadMonsters.add");
                deadMonsters.Add(m);
                continue;
            }

            if (m.Attacked(brightness))
            {
                Debug.Log($"{myId} : 죽였다!");
                deadMonsters.Add(m);
                // deadMonsters 는 ... Remove를 안해서 stack overflow 걸릴거 같음. heap이구나 

            }
            PowerConsumption(powerConsumptionAmount);
        }
        foreach (var m in deadMonsters)
        {
            monsters.Remove(m);
            Debug.Log($"{m} monsters에서 삭제 중. ");
        }
        deadMonsters.Clear();
        if (monsters.Count <= 0)
        {
            Debug.Log($"{myId} : monster count {monsters.Count}");
            IsAttack = false;
            CancelInvoke("Attack");
        }
    }
    /// <summary>
    /// 발광
    /// </summary>
    void luminescence(bool IsOn)
    {
        // IsOn대신 IsDischarge해줘도 될거 같고 
        lightEffect.SetActive(IsOn);
        if (!IsOn && IsAttack)
        {
            IsAttack = false;
            CancelInvoke("Attack");
        }
    }
    /// <summary>
    /// 전력 소비
    /// </summary>
    /// <param name="value"></param>
    void PowerConsumption(int value)
    {
        Debug.Log($"{myId} : 전력 소비 | 남은 전력 : {power}");
        power -= value;
        if (power <= 0)
        {
            Debug.Log("방전..");
            IsDischarge = true;
            luminescence(false);
        }
    }
    // 충전 (손전등 상호작용)
    // 손전등 쪽에서 1초동안 보고 있으면 이거 호출. 
    public void Charging()
    {
        Debug.Log("충전!!");
        power += chargingValuePerSec;
        if (power > chargingLimit)
            power = chargingLimit;

        IsDischarge = false;
        luminescence(true);
    }
    // upgrade
    public void SetRangeLevel(int level)
    {
        int range = data.streetLampRangeData[level].value;
        light.transform.localScale = new Vector3(range, light.transform.localScale.y, range);
        Debug.Log($"범위 : {transform.localScale.x}");
    }
    public void SetBrightnessLevel(int level)
    {
        brightness = data.streetLampRangeData[level].value;
        Debug.Log($"밝기 : {brightness}");
    }
    public void SetPowerLevel(int level)
    {
        chargingLimit = data.streetLampPowerData[level].chargingLimit;
        chargingValuePerSec = data.streetLampPowerData[level].chargingValuePerSec;

        Debug.Log($"전력 충전 한도 : {chargingLimit}");
    }
}
