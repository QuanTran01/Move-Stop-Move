using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public Animator anim;

    private NavMeshAgent agent;
    private Transform player;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
            agent.destination = player.position;
        else
            anim.SetTrigger("Win");
    }
}
