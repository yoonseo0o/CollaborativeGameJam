using System.Collections;
using UnityEngine;

public class SunSystem : MonoBehaviour
{
    public int DeactivePieceCount;
    public Transform pieceTrf { get; private set; }
    public bool IsTime { get; private set; }
    private int defenseSeconds = 1;
    [Header("해의 시간 - 몬스터 스폰 변화 배율")] 
    [SerializeField] private float intervalMag;
    [SerializeField] private int amountMag;
    [Header("해의 조각 활성화 상태 공격 ")]
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
        Debug.Log("해의 시간 시작");
        GameManager.Instance.SetDifficulty(GameManager.Instance.Difficulty + 1);
        IsTime = true;
        pieceTrf= piece;
        GameManager.Instance.MonsterManager.ReTargetingAll();
        MonsterSpawner monsterSpawner = GameManager.Instance.MonsterSpawner;
        monsterSpawner.SetSpawnOption(monsterSpawner.spawnInterval * intervalMag,
            monsterSpawner.spawnAmount*amountMag);
        yield return new WaitForSeconds(defenseSeconds);
        Debug.Log("해의 시간 디펜스 성공!");
        // 10 m 즉사 시키기 
        piece.GetComponent<PieceOfTheSun>().Kill10M();
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
        DeactivePieceCount--;
        if(pieceTrf != null )
        {
            pieceTrf.GetComponent<PieceOfTheSun>().Active();
            UpdatePieceImg();
        }
        else
        {
            Debug.Log("해의 조각 활성화 하려고 하는데 null이에요");
        }
        if(DeactivePieceCount <= 0)
        {
            GameManager.Instance.GameClear();
        }
    }
    private void UpdatePieceImg()
    {
        Debug.Log("랜턴 위에 있는 해의 조각 이미지 업데이트해야돼요 - 미구현");
    }
    private void AttackActivePiece()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15f, LayerMask.GetMask("Monster"));
        Debug.Log($"해의 조각 활성화 상태 공격@!!@ {hitColliders.Length}"); 
        foreach (var hit in hitColliders)
        {
            Debug.Log(hit.GetComponent<Entity>()==null ? $"{hit.name}왜없냐" : $"{hit.name}있지");
            hit.GetComponent<Entity>()?.Attacked(damageAmountOfAPiece * (4 - DeactivePieceCount));
        }  

    } 
}
