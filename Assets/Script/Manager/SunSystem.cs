using System.Collections;
using UnityEngine;

public class SunSystem : MonoBehaviour
{
    public int DeactivePieceCount;
    public Transform pieceTrf { get; private set; }
    public bool IsTime { get; private set; }
    private int defenseSeconds = 1;
    [Header("���� �ð� - ���� ���� ��ȭ ����")] 
    [SerializeField] private float intervalMag;
    [SerializeField] private int amountMag;
    [Header("���� ���� Ȱ��ȭ ���� ���� ")]
    [SerializeField] private float interval=0.25f;
    [SerializeField] private int damageAmountOfAPiece=1; 
    void Awake()
    {
        IsTime = false;

    }
    private void Start()
    {
        InvokeRepeating("AttackActivePiece", interval, interval);
        
    }
    private void OnDestroy()
    {
        CancelInvoke("AttackActivePiece");
    }
    public IEnumerator TimeOfTheSun(Transform piece)
    {
        if (IsTime) yield break;
        Debug.Log("���� �ð� ����");
        GameManager.Instance.SetDifficulty(GameManager.Instance.Difficulty + 1);
        IsTime = true;
        pieceTrf= piece;
        GameManager.Instance.MonsterManager.ReTargetingAll();
        MonsterSpawner monsterSpawner = GameManager.Instance.MonsterSpawner;
        monsterSpawner.SetSpawnOption(monsterSpawner.spawnInterval * intervalMag,
            monsterSpawner.spawnAmount*amountMag);
        yield return new WaitForSeconds(defenseSeconds);
        Debug.Log("���� �ð� ���潺 ����!");
        // 10 m ��� ��Ű�� 
        piece.GetComponent<PieceOfTheSun>().Kill10M();
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
        DeactivePieceCount--;
        if(pieceTrf != null )
        {
            pieceTrf.GetComponent<PieceOfTheSun>().Active();
            UpdatePieceImg();
        }
        else
        {
            Debug.Log("���� ���� Ȱ��ȭ �Ϸ��� �ϴµ� null�̿���");
        }
        if(DeactivePieceCount <= 0)
        {
            GameManager.Instance.GameClear();
        }
    }
    private void UpdatePieceImg()
    {
        Debug.Log("���� ���� �ִ� ���� ���� �̹��� ������Ʈ�ؾߵſ� - �̱���");
    }
    private void AttackActivePiece()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15f, LayerMask.GetMask("Monster"));
        Debug.Log($"���� ���� Ȱ��ȭ ���� ����@!!@ {hitColliders.Length}"); 
        foreach (var hit in hitColliders)
        {
            Debug.Log(hit.GetComponent<Entity>()==null ? $"{hit.name}�־���" : $"{hit.name}����");
            hit.GetComponent<Entity>()?.Attacked(damageAmountOfAPiece * (4 - DeactivePieceCount));
        }  

    } 
}
