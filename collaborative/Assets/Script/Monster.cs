using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] private Transform target; 
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target == null)
            target = GameObject.Find("Player").transform;
    }
     
    void Update()
    {
        if(agent.destination != target.transform.position)
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }
}
