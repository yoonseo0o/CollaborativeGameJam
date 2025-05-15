using System.Collections.Generic;
using UnityEngine;

public class StreetLamp : Structure/*,Interaction*/
{
    [Header("ability")]
    [SerializeField] private Transform light;
    private ParticleSystem lightEffect;

    [Header("Property")]
    private int power;
    private int brightness;
    public int chargingLimit;
    public int chargingValuePerSec;
    private int powerConsumptionAmount=1; // ���¼Һ�

    private StreetLampData data;
    private float attackInterval=0.25f;
    private List<Entity> monsters;
    private bool IsDischarge; // �����ƴ���
    private bool IsAttack;
    private void Awake()
    {
        lightEffect = GetComponent<ParticleSystem>();
        IsDischarge = false;
        IsAttack = false;
        data = GameManager.Instance.StreetLampManager.data; 
        monsters = new List<Entity>();
    }
    private void OnDestroy()
    {
        if(IsAttack)
        {
            CancelInvoke("Attack");
        }
        GameManager.Instance.StreetLampManager.streetLamps.Remove(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        monsters.Add(other.GetComponent<Entity>());
        if (!IsDischarge&&!IsAttack)
        { 
            IsAttack = true;
            InvokeRepeating("Attack", attackInterval, attackInterval);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        monsters.Remove(other.GetComponent<Entity>());
        if (monsters.Count<=0)
        {
            IsAttack = false;
            CancelInvoke("Attack");
        }
    }
    public override void Ability()
    {
        if( lightEffect != null ) { 
        lightEffect.Play();}
    }
    /*void Interaction.interaction()
    { 

    }*/
    void Attack()
    {
        Debug.Log("����!!");
        List<Entity> deadMonsters=new List<Entity>();
        foreach (Entity m in monsters)
        {
            if(m==null) continue;

            if(m.Attacked(brightness))
                deadMonsters.Add(m);
        }
        foreach (var m in deadMonsters)
            monsters.Remove(m);
        PowerConsumption(powerConsumptionAmount);
    }
    /// <summary>
    /// �߱�
    /// </summary>
    void luminescence(bool IsOn)
    {
        // IsOn��� IsDischarge���൵ �ɰ� ����
        // �߱� ������Ʈ setactive (IsOn)
        if(!IsOn&& IsAttack)
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
        Debug.Log("���� �Һ�");
        power -= value;
        if(power <= 0 )
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
        if( power > chargingLimit)
            power = chargingLimit;

        IsDischarge = false;
        luminescence(true);
    }
    // upgrade
    public void SetRangeLevel(int level)
    { 
        int range =  data.streetLampRangeData[level].value;
        light.transform.localScale = new Vector3(range, light.transform.localScale.y, range);
        Debug.Log($"���� : {transform.localScale.x}");
    }
    public void SetBrightnessLevel(int level)
    {  
        brightness =  data.streetLampRangeData[level].value;
        Debug.Log($"��� : {brightness}");
    }
    public void SetPowerLevel(int level)
    {
        chargingLimit =  data.streetLampPowerData[level].chargingLimit;
        chargingValuePerSec  = data.streetLampPowerData[level].chargingValuePerSec;
         
        Debug.Log($"���� ���� �ѵ� : {chargingLimit}");
    }
}
