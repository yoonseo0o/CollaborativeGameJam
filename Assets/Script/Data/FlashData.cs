using UnityEngine;
using System;
using UnityEditor; 
[Serializable]
public class BrightnessData
{
    public int level;
    public int pureCost;
    public int value;
}
[Serializable]
public class DistanceData
{
    public int level;
    public int pureCost;
    public int value;
}
[Serializable]
public class RangeData
{
    public int level;
    public int pureCost;
    public float value;
}

public class FlashData : MonoBehaviour
{
    public BrightnessData[] brightnessDatas;
    public DistanceData[] distanceDatas;
    public RangeData[] rangeDatas;
}