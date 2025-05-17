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
        agent.avoidancePriority = Random.Range(30, 70); // valores entre 0 e 99
        lastPosition = transform.position;
    }

    void Update()
    {
        checkTimer += Time.deltaTime;

        // Verifica se está parado mesmo com destino definido
        if (checkTimer >= stuckCheckDelay)
        {
            float movedDistance = Vector3.Distance(transform.position, lastPosition);

            if (movedDistance < stuckThreshold)
            {
                // Provavelmente preso, tenta novo destino
                SetNewDestination();
            }

            lastPosition = transform.position;
            checkTimer = 0f;
        }

        // Se chegou ao destino, define novo ponto
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
        for (int i = 0; i < 1; i++) // tenta até 30 vezes
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.5f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }
}
