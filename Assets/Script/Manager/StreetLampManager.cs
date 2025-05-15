using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StreetLampManager : MonoBehaviour
{
    public List<StreetLamp> streetLamps;
    public StreetLampData data;
    public int brightnessLevel { get; private set; }
    public int rangeLevel { get; private set; }
    public int powerLevel { get; private set; }

    public void UpgradeRangeLevel()
    {
        if (rangeLevel >= data.streetLampRangeData.Length - 1)
        {
            Debug.Log($"�ִ� ���� ����");
            return;
        }
        // ������ �������� �ʿ� �ڽ�Ʈ�� ���ٸ� 
        if (data.streetLampRangeData[rangeLevel].pureCost >
            GameManager.Instance.PureSystem.pure)
        {
            Debug.Log($"�ʿ� ���� ���� {data.streetLampRangeData[rangeLevel].pureCost - GameManager.Instance.PureSystem.pure} ������ �� �ʿ��մϴ�");
            return;
        }
        else
        {
            GameManager.Instance.PureSystem.LosePure(data.streetLampRangeData[rangeLevel].pureCost);
            rangeLevel++;
            Debug.Log($"���� ��ȭ {rangeLevel}����");
            foreach (var lamp in streetLamps)
            {
                lamp.SetRangeLevel(rangeLevel);

            } 
            GameManager.Instance.lanternTrf.GetComponent<Lantern>().CanvasSetActive(false);

        }
    }
    public void UpgradeBrightnessLevel()
    {
        if (brightnessLevel >= data.streetLampBrightnessData.Length - 1)
        {
            Debug.Log($"�ִ� ���� ����");
            return;
        }
        // ������ �������� �ʿ� �ڽ�Ʈ�� ���ٸ� 
        if (data.streetLampBrightnessData[brightnessLevel].pureCost >
            GameManager.Instance.PureSystem.pure)
        {
            Debug.Log($"�ʿ� ���� ���� {data.streetLampBrightnessData[brightnessLevel].pureCost - GameManager.Instance.PureSystem.pure} ������ �� �ʿ��մϴ�");
            return;
        }
        else
        {
            GameManager.Instance.PureSystem.LosePure(data.streetLampBrightnessData[brightnessLevel].pureCost);
            brightnessLevel++;
            Debug.Log($"���� ��ȭ {brightnessLevel}����");
            foreach (var lamp in streetLamps)
            {
                lamp.SetBrightnessLevel(brightnessLevel);

            }
            GameManager.Instance.lanternTrf.GetComponent<Lantern>().CanvasSetActive(false);

        }
    }
    public void UpgradePowerLevel()
    {
        if (powerLevel >= data.streetLampPowerData.Length - 1)
        {
            Debug.Log($"�ִ� ���� ����");
            return;
        }
        // ������ �������� �ʿ� �ڽ�Ʈ�� ���ٸ� 
        if (data.streetLampPowerData[powerLevel].pureCost >
            GameManager.Instance.PureSystem.pure)
        {
            Debug.Log($"�ʿ� ���� ���� {data.streetLampPowerData[powerLevel].pureCost - GameManager.Instance.PureSystem.pure} ������ �� �ʿ��մϴ�");
            return;
        }
        else
        {
            GameManager.Instance.PureSystem.LosePure(data.streetLampPowerData[powerLevel].pureCost);
            powerLevel++;
            Debug.Log($"���� ��ȭ {powerLevel}����");
            foreach (var lamp in streetLamps)
            {
                lamp.SetPowerLevel(powerLevel);

            }
            GameManager.Instance.lanternTrf.GetComponent<Lantern>().CanvasSetActive(false);

        }
    }

}
