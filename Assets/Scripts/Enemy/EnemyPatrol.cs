using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] wayPoints;
    public int waypointIndex = 0;

    public float timeBetweenMove;
    public float countDown;

    private Vector3 targetDestination;
    private Vector3 currentDestination;
    public Vector3 checkPosition;

    public Slider detectionMeter;

    public int state = 0;

    private bool returning = true;

    public float detection;
    EnemyFOV enemyFOV;
    Vector3 playerPos;

    float count = 5;
    void Start()
    {
        checkPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        enemyFOV = GetComponent<EnemyFOV>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectMeter();


        switch (state)
        {
            case 0:
                PatrolState();
                break;

            case 1:
                CheckingState();
                break;

            case 2:
                PursuitState();
                break;
        }

    }

    private void DetectMeter()
    {
        if (enemyFOV.seePlayer)
        {
            detection += (50 / enemyFOV.distance) * Time.deltaTime;
        }
        else
        {
            detection -= (80 / enemyFOV.distance) * Time.deltaTime;
        }

        if (detection > 10)
        {
            detection = 10;
            state = 2;
        }

        if (detection < 0)
        {
            detection = 0;
        }

        detectionMeter.gameObject.SetActive(detection > 0);
        detectionMeter.value = detection;
    }

    //Patrol State
    private void PatrolState()
    {
        if (returning)
        {
            returning = false;
            UpdateDestination();
            StartCoroutine(goToDestination(0));
        }

        if (Vector3.Distance(transform.position, targetDestination) < 1.5f)
        {
            waypointCircle();
            UpdateDestination();
            StartCoroutine(goToDestination(timeBetweenMove));
            returning = false;
        }
    }


    private void UpdateDestination()
    {
        agent.isStopped = false;
        targetDestination = wayPoints[waypointIndex].position;
    }
    private IEnumerator goToDestination(float time)
    {
        yield return new WaitForSeconds(time);
        agent.SetDestination(targetDestination);
    }
    private void waypointCircle()
    {
        waypointIndex++;
        if (waypointIndex == wayPoints.Length)
        {
            waypointIndex = 0;
        }
    }


    //Checking State
    private void CheckingState()
    {
        agent.isStopped = true;

        StartCoroutine(CheckLocation());
    }

    private IEnumerator CheckLocation()
    {
        yield return new WaitForSeconds(3f);
        agent.ResetPath();
        agent.SetDestination(checkPosition);
        if (Vector3.Distance(transform.position, checkPosition) < 4)
        {
            agent.isStopped = true;
            yield return new WaitForSeconds(5f);
            state = 0;
            UpdateDestination();
            StartCoroutine(goToDestination(0));

        }
    }

    //Pursuit State
    private void PursuitState()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        
        if (enemyFOV.seePlayer)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            agent.SetDestination(playerPos);
            count = 5;
        }

        else if (!enemyFOV.seePlayer) 
        {
            count -= Time.deltaTime;
        }

        if (count < 0)
        {
            count = 0;
            Debug.Log("Go search state");
        }
        Debug.Log(count);

    }


    //Search State
    private void SearchState()
    {

    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }


    private void GoRandom()
    {
    }
}
