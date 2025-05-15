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
            Debug.Log($"최대 레벨 도달");
            return;
        }
        // 손전등 데이터의 필요 코스트가 없다면 
        if (data.streetLampRangeData[rangeLevel].pureCost >
            GameManager.Instance.PureSystem.pure)
        {
            Debug.Log($"필요 동심 부족 {data.streetLampRangeData[rangeLevel].pureCost - GameManager.Instance.PureSystem.pure} 동심이 더 필요합니다");
            return;
        }
        else
        {
            GameManager.Instance.PureSystem.LosePure(data.streetLampRangeData[rangeLevel].pureCost);
            rangeLevel++;
            Debug.Log($"범위 강화 {rangeLevel}레벨");
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
            Debug.Log($"최대 레벨 도달");
            return;
        }
        // 손전등 데이터의 필요 코스트가 없다면 
        if (data.streetLampBrightnessData[brightnessLevel].pureCost >
            GameManager.Instance.PureSystem.pure)
        {
            Debug.Log($"필요 동심 부족 {data.streetLampBrightnessData[brightnessLevel].pureCost - GameManager.Instance.PureSystem.pure} 동심이 더 필요합니다");
            return;
        }
        else
        {
            GameManager.Instance.PureSystem.LosePure(data.streetLampBrightnessData[brightnessLevel].pureCost);
            brightnessLevel++;
            Debug.Log($"범위 강화 {brightnessLevel}레벨");
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
            Debug.Log($"최대 레벨 도달");
            return;
        }
        // 손전등 데이터의 필요 코스트가 없다면 
        if (data.streetLampPowerData[powerLevel].pureCost >
            GameManager.Instance.PureSystem.pure)
        {
            Debug.Log($"필요 동심 부족 {data.streetLampPowerData[powerLevel].pureCost - GameManager.Instance.PureSystem.pure} 동심이 더 필요합니다");
            return;
        }
        else
        {
            GameManager.Instance.PureSystem.LosePure(data.streetLampPowerData[powerLevel].pureCost);
            powerLevel++;
            Debug.Log($"범위 강화 {powerLevel}레벨");
            foreach (var lamp in streetLamps)
            {
                lamp.SetPowerLevel(powerLevel);

            }
            GameManager.Instance.lanternTrf.GetComponent<Lantern>().CanvasSetActive(false);

        }
    }

}
