using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.AI;

public class AIExample : MonoBehaviour
{
    public enum WanderType { Random,Waypoints};

    public FirstPersonController fpsc;
    public WanderType wanderType=WanderType.Random;
    public float fov = 120f;
    public float viewDistance = 10f;
    public float wanderRadius = 7f;
    public float health = 100f;
    public Transform[] waypoints;


    private bool isAware = false;
    private NavMeshAgent agent;
    private new Renderer renderer;
    private Vector3 wanderPoint;
    private int waypointindex=0;
    

   

    // Start is called before the first frame update
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
        wanderPoint = RandomWanderPoint();
    }

    // Update is called once per frame
    public void Update()
    {
        if(isAware)
        {
            agent.SetDestination(fpsc.transform.position);
            renderer.material.color = Color.red;
        }
        else
        {
            Wander();
            SearchForPlayer();
            
            renderer.material.color = Color.blue;
        }
        if (Vector3.Distance(fpsc.transform.position, transform.position) > viewDistance)
        {
            isAware = false;
            Wander();
            // agent.SetDestination(wanderPoint);
        }
        if(health<=0)
        {
            Destroy(transform.gameObject);
        }
    }
    public void SearchForPlayer()
    {
        if(Vector3.Angle(Vector3.forward,transform.InverseTransformPoint(fpsc.transform.position))<fov/2f)
        {
            if(Vector3.Distance(fpsc.transform.position,transform.position)<=viewDistance)
            {
                RaycastHit hit;
                if(Physics.Linecast(transform.position,fpsc.transform.position,out hit))
                {
                    if(hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }
                }
            }
            
        }
    }
    public void OnAware()
    {
        isAware = true;
    }
    public Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint,out navHit,wanderRadius,-1);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    } 
    public void Wander()
    {
        if(wanderType==WanderType.Random)
        {
            if (Vector3.Distance(transform.position, wanderPoint) < 3f)
            {
                wanderPoint = RandomWanderPoint();
            }
            else
            {
                agent.SetDestination(wanderPoint);
            }
        }
        else
        {
            //waypoint wandering
            if(Vector3.Distance(waypoints[waypointindex].position,transform.position)<2f)
            {
                waypointindex = Random.Range(0, waypoints.Length);
               // waypointindex++;
            }
            else
            {
                agent.SetDestination(waypoints[waypointindex].position);
            }

        }
    }
}
