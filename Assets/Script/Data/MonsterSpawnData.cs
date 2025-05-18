using System;
using UnityEngine;

[Serializable]
public class SpawnData
{
    public int id;
    public int difficulty;
    public int monster1_1Amount;
    public int monster1_2Amount;
    public int monster2_1Amount;
    public int monster2_2Amount;
    public int monster3_1Amount;
    public int monster3_2Amount;
}

public class MonsterSpawnData : MonoBehaviour
{ 
    public SpawnData[] spawnData;
}
