using UnityEngine;
using UnityEngine.AI;

public class SmartFollower : MonoBehaviour
{
    public Transform player;          // Player to follow
    private NavMeshAgent agent;       // NavMeshAgent reference
    public float safeDistance = 5.0f; // Minimum safe distance from the player
    public float zigzagFrequency = 2.0f; // How often to zigzag (in seconds)
    public int zigzagAngle = 30;
    public int ZigzagTimeRandomness = 0;
    private float nextZigzagTime;
    private int zigzagDirection = 1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        nextZigzagTime = Time.time + zigzagFrequency;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 direction = (transform.position - player.position).normalized;
        
        if (distance > safeDistance)
        {
            // Move directly towards the player if outside the safe distance
            agent.SetDestination(player.position + direction * safeDistance);
        }
        else
        {
            // Implement zigzagging when too close to the player
            if (Time.time >= nextZigzagTime)
            {
                zigzagDirection *= -1; // Change direction
                nextZigzagTime = Time.time + zigzagFrequency + Random.Range(0, ZigzagTimeRandomness);
                float zigzagAmount = safeDistance - Random.Range(0, 2); // Distance to zigzag side to side
     
                
                Vector3 zigzagTarget = player.position + 
                                       Quaternion.Euler(0, zigzagAngle * zigzagDirection, 0) * 
                                       (player.forward.normalized * zigzagAmount);
                agent.SetDestination(zigzagTarget);
                
            }



            // Make the AI face the player while moving
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }
    
}