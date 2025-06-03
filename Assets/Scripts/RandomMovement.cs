using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range = 10f;
    public Transform centrePoint;

    private float stuckCheckDelay = 1.0f;
    private Vector3 lastPosition;
    private float checkTimer;
    private float stuckThreshold = 0.1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Ajustes de evitação de obstáculos
        agent.avoidancePriority = Random.Range(30, 70);
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        lastPosition = transform.position;
    }

    void Update()
    {
        checkTimer += Time.deltaTime;

        // Verifica se está parado, indicando possível travamento
        if (checkTimer >= stuckCheckDelay)
        {
            float movedDistance = Vector3.Distance(transform.position, lastPosition);

            if (movedDistance < stuckThreshold)
            {
                // Provavelmente está preso, tenta novo destino
                SetNewDestination();
            }

            lastPosition = transform.position;
            checkTimer = 0f;
        }

        // Se chegou ao destino, define um novo ponto
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetNewDestination();
        }
    }

    void SetNewDestination()
    {
        Vector3 point;
        if (RandomPoint(centrePoint.position, range, out point))
        {
            agent.SetDestination(point);
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++) // tenta até 30 vezes
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            randomPoint.y = center.y; // mantém no mesmo nível do chão

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 3f, NavMesh.AllAreas))
            {
                if (Vector3.Distance(hit.position, transform.position) > 1.5f)
                {
                    // Verifica se o caminho é navegável
                    NavMeshPath path = new NavMeshPath();
                    if (agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                    {
                        result = hit.position;
                        return true;
                    }
                }
                
            }
        }

        result = Vector3.zero;
        return false;
    }
}
