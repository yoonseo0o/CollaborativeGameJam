using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PieceOfTheSun : MonoBehaviour, Interaction,Entity
{ 
    private bool IsActive; // �̹� Ȱ��ȭ ��... ��������
    [SerializeField]private GameObject light;
    private int hp=50;
    private int maxHp=50;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Coroutine co;
    private void Start()
    {
        if (GameManager.Instance == null)
            Debug.LogWarning("GameManager.Instance�� null�̿��� ");
        if (GameManager.Instance.SunSystem == null)
            Debug.LogWarning("�� �ý����� null�̿��� ");  
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
        Debug.Log("���� ���� Ȱ��ȭ �õ�");
        co= StartCoroutine(GameManager.Instance.SunSystem.TimeOfTheSun(transform)); 
    }
    /*private IEnumerator TurnOn()
        {
            if (IsActive)
                yield return null ;
            // ���� ���� ������... 
            // 60�� ���� ���� ������.. (�󸶳� ����..?)��������, ���� ������ �÷��̾ ����..
            MonsterSpawner monsterSpawner = GameManager.Instance.MonsterSpawner;
            float interval = monsterSpawner.spawnInterval;
            int amount = monsterSpawner.spawnAmount;
            monsterSpawner.SetSpawnOption(interval * 0.2f, amount);

            // 60�� ī��Ʈ 
            // �ı����� �ʾ��� ��
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
            // �ı� �ƴٸ� ���� ���·�
            else
            {
                // �� ����
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
            Debug.LogError("���� �ð��� �ƴѵ� ��� �����°ž�");
            return false;
        }
        Debug.Log("���� ���� ���� �Դ� ��...");
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
        Debug.Log("���� ���� �ı� - Ȱ��ȭ ����");
        if(co !=null) StopCoroutine(co);
        GameManager.Instance.SunSystem.TimeOutOfTheSun();
        RestorationOriginalState();
        //GameManager.Instance.SunSystem.ActivePiece();
    }
    void RestorationOriginalState()
    {
        Debug.Log("���� ���� ���� ����");
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
