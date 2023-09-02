using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private bool arrived;

    public float detection;
    EnemyFOV enemyFOV;
    Vector3 playerPos;

    float count = 5;
    float catchCount = 3;
    public float waitCount = 1;
    public float reactTime = 0;

    private EnemyAnimation enemyAnimation;
    void Start()
    {
        checkPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        enemyFOV = GetComponent<EnemyFOV>();
        enemyAnimation = GetComponent<EnemyAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectMeter();
        CatchPlayer();
        Debug.Log(reactTime);
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

    private bool checkDestination()
    {
        if (Vector3.Distance(transform.position, targetDestination) < 2.5f)
        {
            return true;
        }
        return false;
    }

    //Patrol State
    private void PatrolState()
    {
        /*if (returning)
        {
            returning = false;
            UpdateDestination();
            goToDestination(10);
        }

        if (Vector3.Distance(transform.position, targetDestination) < 1.5f)
        {
            waypointCircle();
            UpdateDestination();
            goToDestination(10);
            returning = false;
        }*/

        if (!checkDestination())
        {
            UpdateDestination();
            goToDestination();
            waitCount = 10;

        }
        else
        {
            waitCount -= Time.deltaTime;
        }

        if (waitCount < 0)
        {
            waypointCircle();
            UpdateDestination();
            goToDestination();
        }
    }


    private void UpdateDestination()
    {
        agent.isStopped = false;
        targetDestination = wayPoints[waypointIndex].position;
    }
    private void goToDestination()
    {
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
        CheckLocation();
    }

    private void RotateToPoint(Vector3 point)
    {
        var targetRotation = Quaternion.LookRotation(point - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }
    private void CheckLocation()
    {
        reactTime -= Time.deltaTime;
        RotateToPoint(checkPosition);
        if (reactTime < 0)
        {
            targetDestination = checkPosition;
            agent.ResetPath();
            agent.SetDestination(targetDestination);
        }

        if (!checkDestination() && reactTime<0)
        {
            reactTime = 5;
        }

        if (checkDestination())
        {
            agent.isStopped = true;
            reactTime -= Time.deltaTime;
            if (reactTime < 0 )
            {
                state = 0;
                UpdateDestination();
                goToDestination();
            }
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


    private void CatchPlayer()
    {
        float distance = Vector3.Distance(transform.position, playerPos);

        if (distance > 5 || state < 2) catchCount = 3;

        catchCount -= Time.deltaTime;

        if (catchCount < 0)
        {
        }
    }
}
