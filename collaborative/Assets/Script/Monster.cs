using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    [Header("property")]
    private float hp;//?
    private float damageAmount;
    [SerializeField] private float speed;
    NavMeshAgent agent;
    [SerializeField] private Transform target; 

    void Awake()
    {
        agent.speed = speed;
        agent = GetComponent<NavMeshAgent>();
        if (target == null)
            target = GameObject.Find("Player").transform;
    }
     
    void Update()
    {
        moveToTarget();
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
    private void Attack(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<Player>().Attacked(damageAmount);
        else if (other.CompareTag("keep"))
        {
            // 지키는거 tag 확인 후,
            // 지키는 거 클래스의 공격당하는 함수 실행
        }
        else
        {
            Debug.Log($"{name}이 {other.name}과 닿았지만 tag({other.tag})를 알 수 없음");
        }
    }
    private void Attacked(float damageAmount)
    {
        if (IsDead())
            return;
        hp -= damageAmount;

    }
    private bool IsDead()
    {
        return hp < 0;
    }
}