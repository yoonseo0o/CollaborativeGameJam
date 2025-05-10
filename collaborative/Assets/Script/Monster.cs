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
            // ��Ű�°� tag Ȯ�� ��,
            // ��Ű�� �� Ŭ������ ���ݴ��ϴ� �Լ� ����
        }
        else
        {
            Debug.Log($"{name}�� {other.name}�� ������� tag({other.tag})�� �� �� ����");
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