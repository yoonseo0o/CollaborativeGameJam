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
    private int powerConsumptionAmount = 1; // ���¼Һ�

    private StreetLampData data;
    private float attackInterval = 0.25f;
    private List<Entity> monsters;
    private bool IsDischarge; // �����ƴ���
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
            Debug.Log("���� ���̾� �ƴϾ�");
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
                Debug.Log($"{m}�� null -> deadMonsters.add");
                deadMonsters.Add(m);
                continue;
            }

            if (m.Attacked(brightness))
            {
                Debug.Log($"{myId} : �׿���!");
                deadMonsters.Add(m);
                // deadMonsters �� ... Remove�� ���ؼ� stack overflow �ɸ��� ����. heap�̱��� 

            }
            PowerConsumption(powerConsumptionAmount);
        }
        foreach (var m in deadMonsters)
        {
            monsters.Remove(m);
            Debug.Log($"{m} monsters���� ���� ��. ");
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
    /// �߱�
    /// </summary>
    void luminescence(bool IsOn)
    {
        // IsOn��� IsDischarge���൵ �ɰ� ���� 
        lightEffect.SetActive(IsOn);
        if (!IsOn && IsAttack)
        {
            IsAttack = false;
            CancelInvoke("Attack");
        }
    }
    /// <summary>
    /// ���� �Һ�
    /// </summary>
    /// <param name="value"></param>
    void PowerConsumption(int value)
    {
        Debug.Log($"{myId} : ���� �Һ� | ���� ���� : {power}");
        power -= value;
        if (power <= 0)
        {
            Debug.Log("����..");
            IsDischarge = true;
            luminescence(false);
        }
    }
    // ���� (������ ��ȣ�ۿ�)
    // ������ �ʿ��� 1�ʵ��� ���� ������ �̰� ȣ��. 
    public void Charging()
    {
        Debug.Log("����!!");
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
        Debug.Log($"���� : {transform.localScale.x}");
    }
    public void SetBrightnessLevel(int level)
    {
        brightness = data.streetLampRangeData[level].value;
        Debug.Log($"��� : {brightness}");
    }
    public void SetPowerLevel(int level)
    {
        chargingLimit = data.streetLampPowerData[level].chargingLimit;
        chargingValuePerSec = data.streetLampPowerData[level].chargingValuePerSec;

        Debug.Log($"���� ���� �ѵ� : {chargingLimit}");
    }
}
