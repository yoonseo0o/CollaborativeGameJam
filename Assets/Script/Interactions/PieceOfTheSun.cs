using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PieceOfTheSun : MonoBehaviour, Interaction,Entity
{ 
    private bool IsActive; // 이미 활성화 한... 조각인지
    [SerializeField]private GameObject light;
    private int hp=50;
    private int maxHp=50;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Coroutine co;
    private void Start()
    {
        if (GameManager.Instance == null)
            Debug.LogWarning("GameManager.Instance이 null이에요 ");
        if (GameManager.Instance.SunSystem == null)
            Debug.LogWarning("해 시스템이 null이에요 ");  
        GameManager.Instance.SunSystem.ActivePieceCount++;

        IsActive = false;
        light.SetActive(true);
        hp=maxHp;
        
    }
    void Interaction.interaction()
    {
        TurnOn();
    }
    private void TurnOn()
    { 
        if (IsActive) 
            return;
        Debug.Log("해의 조각 활성화 시도");
        co= StartCoroutine(GameManager.Instance.SunSystem.TimeOfTheSun(transform)); 
    }
    /*private IEnumerator TurnOn()
        {
            if (IsActive)
                yield return null ;
            // 빛을 내기 시작함... 
            // 60초 동안 많은 적들이.. (얼마나 많이..?)몰려오며, 해의 조각과 플레이어를 공격..
            MonsterSpawner monsterSpawner = GameManager.Instance.MonsterSpawner;
            float interval = monsterSpawner.spawnInterval;
            int amount = monsterSpawner.spawnAmount;
            monsterSpawner.SetSpawnOption(interval * 0.2f, amount);

            // 60초 카운트 
            // 파괴되지 않았을 시
            yield return StartCoroutine(Defense60Time());
            if ( )
            {
                IsActive = true;
                light.SetActive(false);
                remainFlagCount--;
                if (remainFlagCount <= 0)
                {
                    GameManager.Instance.GameClear();
                }
            }
            // 파괴 됐다면 원래 상태로
            else
            {
                // 빛 끄기
                hp = maxHp; 
                monsterSpawner.SetSpawnOption(interval, amount);
            }
        }
        private IEnumerator Defense60Time()
        {
            bool IsDestruction = false;
            int time = 60;
            while (!IsDestruction&&time-->0)
            {

                yield return new WaitForSeconds(1f);
            }
        }*/ 
    bool Entity.Attacked(int damageAmount)
    {
        if (!GameManager.Instance.SunSystem.IsTime)
        {
            Debug.LogError("해의 시간이 아닌데 어떻게 때리는거야");
            return false;
        }
        Debug.Log("해의 조각 피해 입는 중...");
        if (IsActive) return false;
        hp -= damageAmount;
        if(hp < 0)
        {
            ((Entity)this).Dead();
            return true;
        }
        return false;
    }
    void Entity.Dead()
    {
        if(!GameManager.Instance.SunSystem.IsTime) return;
        Debug.Log("해의 조각 파괴 - 활성화 실패");
        if(co !=null) StopCoroutine(co);
        GameManager.Instance.SunSystem.TimeOutOfTheSun();
        RestorationOriginalState();
        //GameManager.Instance.SunSystem.ActivePiece();
    }
    void RestorationOriginalState()
    {
        Debug.Log("해의 조각 원상 복구");
        hp = maxHp;
        IsActive = false;
        light.SetActive(true);
    }
    public void Active()
    {
        light.SetActive(false);
        IsActive = true;
    }
}
