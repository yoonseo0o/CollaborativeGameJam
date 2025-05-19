using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class PieceOfTheSun : MonoBehaviour, Interaction, Entity
{
    private bool IsActive; // 이미 활성화 한... 조각인지
    [SerializeField] private GameObject light;
    private int hp = 50;
    private int maxHp = 50;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Coroutine co;
    private int InteractionAvailableTime = 60;
    private bool IsInteractionAvailable;
    [Header("연출")]
    private float DistanceY=3f; 
    private Coroutine moveCo;
    private void Start()
    {
        if (GameManager.Instance == null)
            Debug.LogWarning("GameManager.Instance이 null이에요 ");
        if (GameManager.Instance.SunSystem == null)
            Debug.LogWarning("해 시스템이 null이에요 ");
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
            Debug.Log("상호작용 불가능 - 사유 : 활성화 실패으로부터 1분 미만의 시간이 흐름");
        }
    }
    private void TurnOn()
    {
        if (IsActive)
            return;
        Debug.Log("해의 조각 활성화 시도");
        co = StartCoroutine(GameManager.Instance.SunSystem.TimeOfTheSun(transform));
        // 연출
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
            Debug.LogError("해의 시간이 아닌데 어떻게 때리는거야");
            return false;
        }
        Debug.Log("해의 조각 피해 입는 중...");
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
        Debug.Log("해의 조각 파괴 - 활성화 실패");
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
        Debug.Log("해의 조각 원상 복구");
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
        Debug.Log("10M 범위 내 적 즉사 시키기");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Monster")); 
        foreach (var hit in hitColliders)
        { 
            hit.GetComponent<Entity>()?.Attacked(999999);
        }
    }
}
