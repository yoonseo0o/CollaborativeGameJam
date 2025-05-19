using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements; 
public enum MonsterType { walk,creep }; 
public class Monster : MonoBehaviour, Entity
{
    [Header("instance property")]
    [SerializeField] private int hp=10;//?
    [SerializeField] private float speed;
    [SerializeField] private int damageAmount=1;
    [SerializeField] private int pureValue=2;
    [SerializeField] private MonsterType type;

    [Header("attack property")]
    [SerializeField] private float attackCoolingTime=2f;
    [SerializeField] private float attackDistance=1f;
    private bool IsAttack;

    NavMeshAgent agent;
    [Header("target")]
    [SerializeField] private Transform target;
    private List<Transform> structuresInRange;
    private Transform lanternInRange;
    private Transform playerInRange;


    [SerializeField ] private Animator animator;
    [SerializeField] private Collider collider;
    Coroutine stanbyAttackCo;
    bool IsDead;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        structuresInRange = new List<Transform>(); 
        if (animator == null)
            Debug.LogError("animator is null");
        IsAttack = false;
        IsDead = false;
    }
    void Start()
    { 
        if (target == null)
            target = GameManager.Instance.playerTrf;

        TargetSelection();
        stanbyAttackCo=StartCoroutine(StandbyAttack());

    }
    void OnDestroy()
    {
        if(stanbyAttackCo != null)
            StopCoroutine(stanbyAttackCo);
    }
    void Update()
    {
        moveToTarget();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lantern"))
        { 
            lanternInRange = other.transform;
            TargetSelection();
        }
        else if (other.CompareTag("Player"))
        {
            playerInRange = other.transform;
            TargetSelection();
        }
        else if (other.CompareTag("Structure"))
        {
            structuresInRange.Add(other.transform);
            TargetSelection();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lantern"))
        {
            lanternInRange = null;
            TargetSelection();
        }
        else if (other.CompareTag("Player"))
        {
            playerInRange = null;
            TargetSelection();
        }
        else if (other.CompareTag("Structure"))
        { 
            structuresInRange.Remove(other.transform);
            TargetSelection();
        }
    }
    bool Entity.Attacked(int damageAmount)
    {
        if (IsDead) { Debug.Log($"죽어있는뎅 : {hp}"); return true; }
        hp -= damageAmount; 
        if (hp<=0)
        {
            ((Entity)this).Dead(); 
            IsDead = true;
            return true;
        }
        else { return false; }
    }
    void Entity.Dead()
    {
        collider.enabled = false;
        GameManager.Instance.PureSystem.GetPure(pureValue);
        GameManager.Instance.MonsterManager.Remove(this);
        Destroy(gameObject);
    }
    public void TargetSelection()
    {
        // 해의 조각 시간이라면 
        if (GameManager.Instance.SunSystem.IsTime)
        {
            if (GameManager.Instance.SunSystem.pieceTrf==null)
            {
                Debug.LogError("해의 조각이 null");
                target = GameManager.Instance.playerTrf;
                return;
            }
            // 플레이어와 해의 조각 중 가까운 곳으로 타겟팅
            if (Vector3.Distance(transform.position, GameManager.Instance.SunSystem.pieceTrf.position) <
                           Vector3.Distance(transform.position, GameManager.Instance.playerTrf.position))
                target = GameManager.Instance.SunSystem.pieceTrf;
            else
                target = GameManager.Instance.playerTrf;
            return;
        }
        switch (type)
        {
            case MonsterType.walk:
                {

                    if (lanternInRange != null) { target = lanternInRange; }
                    else if (structuresInRange.Count > 0)
                    {
                        if (structuresInRange[0] == null)
                        {
                            target = GameManager.Instance.lanternTrf;
                            return;
                        }
                        Transform minDistanceStr = structuresInRange[0];
                        float minSqrDistance = (transform.position - minDistanceStr.position).sqrMagnitude;

                        List<Transform> deadStructures = new List<Transform>();
                        foreach (var str in structuresInRange)
                        {
                            if (str == null)
                            { 
                                deadStructures.Add(str);
                                continue;
                            }
                            float sqrDistance = (transform.position - str.position).sqrMagnitude;
                            if (sqrDistance < minSqrDistance)
                            {
                                minSqrDistance = sqrDistance;
                                minDistanceStr = str;
                            }
                        }
                        foreach (var m in deadStructures)
                            structuresInRange.Remove(m);

                        target = minDistanceStr;
                    }
                    else if (playerInRange != null) { target = playerInRange; }
                    else
                    {
                        if (Vector3.Distance(transform.position, GameManager.Instance.lanternTrf.position) <
                            Vector3.Distance(transform.position, GameManager.Instance.playerTrf.position))
                            target = GameManager.Instance.lanternTrf;
                        else
                            target = GameManager.Instance.playerTrf;
                    }

                }
                break;

            case MonsterType.creep:
                {
                    if (playerInRange != null) { target = playerInRange; }
                    else if (structuresInRange.Count > 0)
                    {
                        if (structuresInRange[0] == null)
                        {
                            target = GameManager.Instance.playerTrf;
                            return;
                        }
                        Transform minDistanceStr = structuresInRange[0];
                        float minSqrDistance = (transform.position - minDistanceStr.position).sqrMagnitude;

                        foreach (var str in structuresInRange)
                        {
                            if (str == null)
                            {
                                structuresInRange.Remove(str);
                                continue;
                            }
                            float sqrDistance = (transform.position - str.position).sqrMagnitude;
                            if (sqrDistance < minSqrDistance)
                            {
                                minSqrDistance = sqrDistance;
                                minDistanceStr = str;
                            }
                        }

                        target = minDistanceStr;
                    }
                    else
                    {
                        target = GameManager.Instance.playerTrf;
                    }
                }
                break;
        }

    }
    private void moveToTarget()
    {
        if (IsAttack) {  
            agent.SetDestination(transform.position); 
            return; }
        if (target == null)
        { 
            agent.SetDestination(transform.position);
        }
        else if (agent.destination != target.transform.position)
        { 
            agent.SetDestination(target.transform.position);
        }
        else
        { 
            agent.SetDestination(transform.position);
        }
    }
    private IEnumerator StandbyAttack()
    {
        while(true)
        { 
            if (target == null)
            {
                stanbyAttackCo = null; 
                TargetSelection();
                yield break;
            }
            // 타겟이랑 거리 체크
            if (Vector3.Distance(transform.position, target.position) > attackDistance)
            { 
                // 공격 범위 안에 들어올때까지 대기
                while (target != null&&Vector3.Distance(transform.position, target.position) > attackDistance )
                { 
                    yield return null; 
                }
            }
            // 공격 범위 안이면 공격 
            if (target != null)
            {
                IsAttack = true;
                if (!Attack())
                {
                    IsAttack = false;
                    continue;
                }
            }
            else continue;
            // 쿨타임 돌라가
            yield return new WaitForSeconds(attackCoolingTime);
            IsAttack=false;
        }
    }
    private bool Attack()
    {
        if(target==null)
        { 
            TargetSelection();
            return false;
        }
        if (target.GetComponent<Entity>()!=null)
        {  
            if ((target.GetComponent<Entity>() is PieceOfTheSun)&&
                    !GameManager.Instance.SunSystem.IsTime)
            { 
                TargetSelection();
                return false;
            }
            animator.SetTrigger("IsAttack");
            target.GetComponent<Entity>().Attacked(damageAmount);
            return true;
        }
        return false;
    }  
}