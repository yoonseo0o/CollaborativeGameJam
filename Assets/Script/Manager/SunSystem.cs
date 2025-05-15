using System.Collections;
using UnityEngine;

public class SunSystem : MonoBehaviour
{
    public int ActivePieceCount;
    public Transform pieceTrf { get; private set; }
    public bool IsTime { get; private set; }
    private int defenseSeconds = 10;
    [Header("���� �ð� - ���� ���� ��ȭ ����")] 
    [SerializeField] private float intervalMag;
    [SerializeField] private int amountMag;
    void Awake()
    {
        IsTime = false;
    }
    
    public IEnumerator TimeOfTheSun(Transform piece)
    {
        if (IsTime) yield break;
        Debug.Log("���� �ð� ����");
        IsTime = true;
        pieceTrf= piece;
        GameManager.Instance.MonsterManager.ReTargetingAll();
        MonsterSpawner monsterSpawner = GameManager.Instance.MonsterSpawner;
        monsterSpawner.SetSpawnOption(monsterSpawner.spawnInterval * intervalMag,
            monsterSpawner.spawnAmount*amountMag);
        yield return new WaitForSeconds(defenseSeconds);
        Debug.Log("���� �ð� ���潺 ����!");
        ActivePiece(); 
        TimeOutOfTheSun();
    }
    public void TimeOutOfTheSun()
    {
        if (!IsTime) return;
        Debug.Log("���� �ð� ����");
        MonsterSpawner monsterSpawner = GameManager.Instance.MonsterSpawner;
        monsterSpawner.SetSpawnOption(monsterSpawner.spawnInterval / intervalMag,
            monsterSpawner.spawnAmount / amountMag);
        IsTime = false;
        pieceTrf = null;
        GameManager.Instance.MonsterManager.ReTargetingAll();
    }
    public void ActivePiece()
    {
        Debug.Log("���� ���� Ȱ��ȭ!@@!!@!@");
        ActivePieceCount--;
        if(pieceTrf != null )
        {
            pieceTrf.GetComponent<PieceOfTheSun>().Active();
        }
        else
        {
            Debug.Log("���� ���� Ȱ��ȭ �Ϸ��� �ϴµ� null�̿���");
        }
        if(ActivePieceCount <= 0)
        {
            GameManager.Instance.GameClear();
        }
    }
}
