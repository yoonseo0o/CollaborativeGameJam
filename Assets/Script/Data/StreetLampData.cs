using UnityEngine;
using System;
using UnityEditor;
[Serializable]
public class StreetLampBrightnessData
{
    public int level;
    public int pureCost;
    public int value;
}
[Serializable]
public class StreetLampRangeData
{
    public int level;
    public int pureCost;
    public int value;
}
[Serializable]
public class StreetLampPowerData
{
    public int level;
    public int pureCost;
    public int chargingLimit;
    public int chargingValuePerSec;
}

public class StreetLampData : MonoBehaviour
{
    public StreetLampBrightnessData[] streetLampBrightnessData;
    public StreetLampRangeData[] streetLampRangeData;
    public StreetLampPowerData[] streetLampPowerData;
}
