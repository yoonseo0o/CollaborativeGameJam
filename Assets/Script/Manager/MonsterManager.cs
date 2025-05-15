using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{ 
    private List<Monster> monsters;

    private void Awake()
    {
        monsters = new List<Monster>();
    }
    public void AddMonster(Monster monster)
    {
        monsters .Add(monster);
    }
    public void Remove(Monster monster)
    {
        monsters.Remove(monster);
    }
    public void ReTargetingAll()
    {
        foreach (Monster monster in monsters)
        {
            monster.TargetSelection();
        }
    }
}
