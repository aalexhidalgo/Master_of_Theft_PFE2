using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private GameObject myCam;

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

    private Animator myCamAnim;
    private bool camPrueba = true;

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

        myCam = GameObject.Find("Main Camera");
        myCamAnim = GameObject.Find("CameraHolder").GetComponent<Animator>();
    }

    void Update()
    {
        //Vision and attack fields of the agent
        Vector3 pos = transform.position;
        playerInVisionRange = Physics.CheckSphere(pos, visionRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(pos, attackRange, playerLayer);

        if (!playerInVisionRange && !playerInAttackRange)
        {
            agent.speed = 3f;
            FollowPatrolRoute(); //The agent will walk through the scenario
        }

        if (playerInVisionRange && !playerInAttackRange)
        {
            agent.speed = 10f;
            Chase(); //The agent will chase the player
        }

        if (playerInVisionRange && playerInAttackRange)
        {           
            Attack();//The agent will make an uppercut to the player to finally stop the game
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

                if(guard_Attack == false)
                {
                    StartCoroutine(Idle_Cooldown());
                }
               
            }           
        }        
    }

    private IEnumerator Idle_Cooldown()
    {
        //To look to the direction the agent is following
        transform.LookAt(points[nextPoint].position);

        float Timer = Random.Range(3f, 8f);
        int RandIndx = Random.Range(0, 2);
        if (RandIndx == 0)
        {
            guard_Walking = true;
            canMove = true;           
            
        }
        if (RandIndx == 1)
        {
            guard_Walking = false;
            yield return new WaitForSeconds(Timer);
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
        transform.LookAt(player.transform.GetChild(0));
    }

    private void Attack()
    {       
        transform.rotation = Quaternion.Euler(0f, 180f, 0f); //The guard faces ther player
        transform.LookAt(player.transform.GetChild(0));       

        Vector3 playerOffset = new Vector3(-0.75f, 0f, 6f);
        agent.SetDestination(player.position + playerOffset); //Distance the guard has to respect with the player

        guard_Running = false;
        guard_Walking = false;
        guard_Attack = true;

        //Nos aseguramos de que si justo hemos saltado la cámara por mucho que se inhabilite (función GAMEover) siga mirando al guardia a la altura de sus ojos     
        myCam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        if (camPrueba == true)
        {
            myCam.transform.LookAt(guardEyes); //The player looks at the guard
            camPrueba = false;
        }
        myCamAnim.enabled = true; //The guard make an uppercut to the player and the camera moves with the punch
        GameManagerScript.GameOver();
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
