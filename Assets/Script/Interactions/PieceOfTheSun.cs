using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class PieceOfTheSun : MonoBehaviour, Interaction, Entity
{
    private bool IsActive; // �̹� Ȱ��ȭ ��... ��������
    [SerializeField] private GameObject light;
    private int hp = 50;
    private int maxHp = 50;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Coroutine co;
    private int InteractionAvailableTime = 60;
    private bool IsInteractionAvailable;
    [Header("����")]
    private float DistanceY=3f; 
    private Coroutine moveCo;
    private void Start()
    {
        if (GameManager.Instance == null)
            Debug.LogWarning("GameManager.Instance�� null�̿��� ");
        if (GameManager.Instance.SunSystem == null)
            Debug.LogWarning("�� �ý����� null�̿��� ");
        GameManager.Instance.SunSystem.DeactivePieceCount++;

        IsActive = false;
        light.SetActive(true);
        hp = maxHp;
        IsInteractionAvailable = true;


    }
    void Interaction.interaction()
    {
        if (IsInteractionAvailable)
            TurnOn();
        else
        {
            Debug.Log("��ȣ�ۿ� �Ұ��� - ���� : Ȱ��ȭ �������κ��� 1�� �̸��� �ð��� �帧");
        }
    }
    private void TurnOn()
    {
        if (IsActive)
            return;
        Debug.Log("���� ���� Ȱ��ȭ �õ�");
        co = StartCoroutine(GameManager.Instance.SunSystem.TimeOfTheSun(transform));
        // ����
        if (moveCo != null) StopCoroutine(moveCo);
        moveCo=StartCoroutine(moveToPos(transform.position+ Vector3.up* DistanceY,1f));
    } 
    private IEnumerator moveToPos(Vector3 pos,float speed)
    {
        while (Vector3.Magnitude(transform.position - pos) > 0.5f)
        {
            Debug.Log("moveToPos");
            transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = pos;
        moveCo = null;
    }
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
        if (hp < 0)
        {
            ((Entity)this).Dead();
            return true;
        }
        return false;
    }
    void Entity.Dead()
    {
        if (!GameManager.Instance.SunSystem.IsTime) return;
        Debug.Log("���� ���� �ı� - Ȱ��ȭ ����");
        if (co != null) StopCoroutine(co);
        GameManager.Instance.SunSystem.TimeOutOfTheSun();
        RestorationOriginalState();
        
        GameManager.Instance.SetDifficulty(GameManager.Instance.Difficulty-1);
        IsInteractionAvailable = false;
        Invoke("SetInteractionAvailable", InteractionAvailableTime);
        //GameManager.Instance.SunSystem.ActivePiece();
    }
    void SetInteractionAvailable()
    {

        IsInteractionAvailable = true;
    }
    void RestorationOriginalState()
    {
        Debug.Log("���� ���� ���� ����");
        hp = maxHp;
        if (moveCo != null) StopCoroutine(moveCo);
        moveCo = StartCoroutine(moveToPos(transform.position - Vector3.up * DistanceY, 1f));
        IsActive = false;
        light.SetActive(true);
    }
    public void Active()
    {
        if (moveCo != null) StopCoroutine(moveCo);
        moveCo = StartCoroutine(moveToPos(GameManager.Instance.SunSystem.transform.position,6));
        light.SetActive(false);
        IsActive = true;
    }
    public void Kill10M()
    {
        Debug.Log("10M ���� �� �� ��� ��Ű��");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Monster")); 
        foreach (var hit in hitColliders)
        { 
            hit.GetComponent<Entity>()?.Attacked(999999);
        }
    }
}
