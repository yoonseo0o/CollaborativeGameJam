using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour, Entity
{
    [Header("property")]
    private int hp=10;//?
    private int damageAmount=1;
    [SerializeField] private float speed;
    private float attackCoolingTime=2f;
    private float attackDistance=1f;
    NavMeshAgent agent;
    [SerializeField] private Transform target;

    private List<Transform> structuresInRange;
    private Transform lanternInRange;
    private Transform playerInRange;

    Coroutine stanbyAttackCo;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        structuresInRange = new List<Transform>();
    }
    void Start()
    {
        if (target == null)
            target = GameManager.Instance.lanternTrf;

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
    void Entity.Attacked(int damageAmount)
    { 
        hp -= damageAmount;
        if (hp<=0)
            ((Entity)this).Dead();
    }
    void Entity.Dead()
    {
        Destroy(gameObject);
    }
    private void TargetSelection()
    {
        if (lanternInRange != null) { target = lanternInRange; }
        else if (playerInRange != null) { target = playerInRange; }
        else if (structuresInRange.Count > 0)
        { 
            Transform minDistanceStr = structuresInRange[0];
            float minSqrDistance = (transform.position - minDistanceStr.position).sqrMagnitude;

            foreach (var str in structuresInRange)
            {
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
            target = GameManager.Instance.lanternTrf;
        } 
    }
    private void moveToTarget()
    {
        if (agent.destination != target.transform.position)
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
                Debug.LogError($"{name}의 target이 null임");
                stanbyAttackCo = null;
                yield break;
            }
            // 타겟이랑 거리 체크
            if (Vector3.Distance(transform.position, target.position) > attackDistance)
            { 
                // 공격 범위 안에 들어올때까지 대기
                while (Vector3.Distance(transform.position, target.position) > attackDistance)
                { 
                    yield return null; 
                } 
            }
            // 공격 범위 안이면 공격 
            Attack(); 
            // 쿨타임 돌라가
            yield return new WaitForSecondsRealtime(attackCoolingTime);  
        }
    }
    private void Attack()
    {
        if (target.CompareTag("Lantern")|| target.CompareTag("Player"))
            target.GetComponent<Entity>().Attacked(damageAmount);
    }  
}