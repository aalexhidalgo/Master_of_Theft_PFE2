using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    public float visionRange = 20f;
    public float attackRange = 10f;

    private bool playerInVisionRange;
    private bool playerInAttackRange;

    [SerializeField] private LayerMask playerLayer;

    public Transform guardEyes;

    //Points route to follow
    [SerializeField] private Transform[] points;
    private int totalPoints;
    public int nextPoint;

    //Animations
    private Animator guardAnim;
    private bool guard_Walking, guard_Running, guard_Attack;

    private bool canMove = true;

    //Scripts
    private GameManager GameManagerScript;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        guardAnim = GetComponent<Animator>();
        //Localize the player
        player = GameObject.Find("Player").transform;
        GameManagerScript = FindObjectOfType<GameManager>();

        //It teleports the agent on the first point (0) and set the total of points to follow
        transform.position = points[0].position;
        totalPoints = points.Length;
        nextPoint = 1;
        agent.SetDestination(points[nextPoint].position);
    }

    void Update()
    {
        //Vision and attack fields of the agent
        Vector3 pos = transform.position;
        playerInVisionRange = Physics.CheckSphere(pos, visionRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(pos, attackRange, playerLayer);

        if (!playerInVisionRange && !playerInAttackRange)
        {
            FollowPatrolRoute(); //The agent will walk through the scenario
        }

        if (playerInVisionRange && !playerInAttackRange)
        {
            Chase(); //The agent will chase the player
        }

        if (playerInVisionRange && playerInAttackRange)
        {
            Attack(); //The agent will make an uppercut to the player to finally stop the game
        }

    }

    //Animations
    private void LateUpdate()
    {       
        guardAnim.SetBool("Guard_Walking", guard_Walking);
        guardAnim.SetBool("Guard_Running", guard_Running);
        guardAnim.SetBool("Guard_Attack", guard_Attack);
    }
    private void FollowPatrolRoute()
    {
        guard_Running = false;

        if (canMove == true)
        {
            guard_Walking = true;

            if (Vector3.Distance(transform.position, points[nextPoint].position) < 0.7f)
            {
                canMove = false;
                nextPoint++;

                //If the agent completes the route, it will go again to the first point (0)
                if (nextPoint == totalPoints)
                {
                    nextPoint = 0;
                }

                StartCoroutine(Idle_Cooldown());                
            }           
        }        
    }

    private IEnumerator Idle_Cooldown()
    {
        //To look to the direction the agent is following
        transform.LookAt(points[nextPoint].position);

        int RandIndx = Random.Range(0, 2);
        if (RandIndx == 0)
        {
            guard_Walking = true;
            canMove = true;           
            
        }
        if (RandIndx == 1)
        {
            guard_Walking = false;
            //agent.SetDestination(transform.position);
            yield return new WaitForSeconds(3f);
            canMove = true;
        }
        
        agent.SetDestination(points[nextPoint].position);
    }

    private void Chase()
    {
        guard_Walking = false;
        guard_Running = true;

        //The agent will leave the route and inmediately follow the player to try to stop it
        agent.SetDestination(player.position);
        guardEyes.transform.LookAt(player);
    }

    private void Attack()
    {
        agent.SetDestination(player.position);
        guard_Running = false;
        guard_Attack = true;
        GameManagerScript.GameOver = true;
    }

    //To see the Gizmos in the editor
    private void OnDrawGizmos()
    {
        //Vision range of the agent
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        //Attack range of the agent
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
