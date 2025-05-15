using System.Collections;
using UnityEngine;

public class SunSystem : MonoBehaviour
{
    public int ActivePieceCount;
    public Transform pieceTrf { get; private set; }
    public bool IsTime { get; private set; }
    private int defenseSeconds = 10;
    [Header("해의 시간 - 몬스터 스폰 변화 배율")] 
    [SerializeField] private float intervalMag;
    [SerializeField] private int amountMag;
    void Awake()
    {
        IsTime = false;
    }
    
    public IEnumerator TimeOfTheSun(Transform piece)
    {
        if (IsTime) yield break;
        Debug.Log("해의 시간 시작");
        IsTime = true;
        pieceTrf= piece;
        GameManager.Instance.MonsterManager.ReTargetingAll();
        MonsterSpawner monsterSpawner = GameManager.Instance.MonsterSpawner;
        monsterSpawner.SetSpawnOption(monsterSpawner.spawnInterval * intervalMag,
            monsterSpawner.spawnAmount*amountMag);
        yield return new WaitForSeconds(defenseSeconds);
        Debug.Log("해의 시간 디펜스 성공!");
        ActivePiece(); 
        TimeOutOfTheSun();
    }
    public void TimeOutOfTheSun()
    {
        if (!IsTime) return;
        Debug.Log("해의 시간 종료");
        MonsterSpawner monsterSpawner = GameManager.Instance.MonsterSpawner;
        monsterSpawner.SetSpawnOption(monsterSpawner.spawnInterval / intervalMag,
            monsterSpawner.spawnAmount / amountMag);
        IsTime = false;
        pieceTrf = null;
        GameManager.Instance.MonsterManager.ReTargetingAll();
    }
    public void ActivePiece()
    {
        Debug.Log("해의 조각 활성화!@@!!@!@");
        ActivePieceCount--;
        if(pieceTrf != null )
        {
            pieceTrf.GetComponent<PieceOfTheSun>().Active();
        }
        else
        {
            Debug.Log("해의 조각 활성화 하려고 하는데 null이에요");
        }
        if(ActivePieceCount <= 0)
        {
            GameManager.Instance.GameClear();
        }
    }
}
