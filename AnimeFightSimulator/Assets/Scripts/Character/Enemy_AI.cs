using System.Collections;
using System.Collections.Generic;
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
    public int damage = 10;             // Damage to deal to the player
    public float damageInterval = 1.0f;      // Time between damages
    private float nextDamageTime = 0;        // Next time damage can be applied
    public int damageDistance = 5;
    public int pathSegments = 10;       // Number of segments for path deviations
    public float deviationMagnitude = 1.0f; // Magnitude of each deviation

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        nextZigzagTime = Time.time + zigzagFrequency;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (distance >= safeDistance)
        {
            Vector3 destination = player.position + (transform.position - player.position).normalized * safeDistance;
            if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
            {
                MoveInSegmentsTowards(destination);
            }
        }
        else
        {
            ImplementZigzagMovement();
        }

        FacePlayer();
        CheckAndDealDamage(distance);
    }

    void MoveInSegmentsTowards(Vector3 destination)
    {
        Vector3 currentPos = transform.position;
        List<Vector3> pathPoints = new List<Vector3>();
        float segmentLength = Vector3.Distance(currentPos, destination) / pathSegments;

        for (int i = 1; i <= pathSegments; i++)
        {
            Vector3 segmentEnd = Vector3.Lerp(currentPos, destination, (float)i / pathSegments);
            Vector3 deviation = Random.insideUnitSphere * deviationMagnitude;
            deviation.y = 0; // Ensure there's no vertical deviation
            pathPoints.Add(segmentEnd + deviation);
        }

        StartCoroutine(FollowPath(pathPoints));
    }

    IEnumerator FollowPath(List<Vector3> pathPoints)
    {
        foreach (var point in pathPoints)
        {
            agent.SetDestination(point);
            while (Vector3.Distance(transform.position, point) > agent.stoppingDistance)
            {
                yield return null;
            }
        }
    }

    void ImplementZigzagMovement()
    {
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
    }

    void FacePlayer()
    {
        Vector3 lookPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookPosition);
    }

    void CheckAndDealDamage(float distance)
    {
        if (distance < safeDistance + damageDistance && Time.time >= nextDamageTime)
        {
            nextDamageTime = Time.time + damageInterval;
            DealDamage(damage);
        }
    }

    void DealDamage(int amount)
    {
        // Assuming the player has a method to receive damage
        player.GetComponent<Character_Health>().reduceHealth(amount);
    }
}
