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

    public float radius = 10f;
    [Range(0, 360)]
    public float angle;
    
    private bool playerInVisionRange;
    private bool playerInAttackRange;
    private bool oldplayerInVisionRange;
    private bool oldplayerInAttackRange;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask ObstacleLayer;

    public bool canSeePlayer = true;

    public Transform guardEyes;

    //Points route to follow
    [SerializeField] private Transform[] points;
    private int totalPoints;
    public int nextPoint;

    //Animations
    private Animator guardAnim;
    private bool guard_Walking, guard_Running, guard_Attack;

    private bool canMove = true;
    private bool chasing = false;

    private Animator myCamAnim;
    private bool camPrueba = true;

    private float Timer;

    //Audio
    public AudioClip[] GuardSFX; //Walking, Running, Idle, Attacking
    private AudioSource guardAudioSource;
    private bool isPlaying = false;

    //Scripts
    private PlayerController PlayerControllerScript;
    private GameManager GameManagerScript;
    private AudioSource gameManagerAudioSource;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        guardAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        guardAnim = GetComponent<Animator>();
        //Localize the player
        player = GameObject.Find("Player").transform;
        PlayerControllerScript = FindObjectOfType<PlayerController>();
        GameManagerScript = FindObjectOfType<GameManager>();
        gameManagerAudioSource = GameObject.Find("GameManager").GetComponent<AudioSource>();

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

        Vector3 directionToPlayer = (player.position - transform.position).normalized; //Direction to be taken into account based on the player's position

        if (Vector3.Angle(transform.forward, directionToPlayer) < angle / 2) //Only if the player is within the angle of vision will he try to catch it.
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position); //Distance between the player and the guard

            if (Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, ObstacleLayer))
            {
                canSeePlayer = false;
            }
            else
            {
                canSeePlayer = true;
                Debug.DrawLine(pos, player.position, Color.green);
            }
        }

        if (canSeePlayer == false) //If he does not see us, he will be able to patrol, regardless of whether the player is nearby or not.
        {
            if ((!playerInVisionRange && !playerInAttackRange) || (playerInVisionRange && !playerInAttackRange))
            {
                agent.speed = 3f;
                FollowPatrolRoute(); //The agent will walk through the scenario
            }
        }
        else
        {
            if (!playerInVisionRange && !playerInAttackRange)
            {
                agent.speed = 3f;
                FollowPatrolRoute(); //The agent will walk through the scenario
            }

            if (playerInVisionRange && !playerInAttackRange)
            {
                agent.speed = 15f;
                Chase(); //The agent will chase the player
                GuardSound(1, 132.049f);
            }

            if (playerInVisionRange && playerInAttackRange)
            {
                Attack(); //The agent will make an uppercut to the player to finally stop the game
            }
        }

        if(guard_Walking == true)
        {
            GuardSound(0, 5.081f);
        }
        if (guard_Walking == false && guard_Running == false && guard_Attack == false)
        {
            GuardSound(3, Timer);
        }

        if (GameManagerScript.pause == true)
        {
            guardAnim.enabled = false;
            agent.isStopped = true; //To stop the guard from doing anything
        }
        else
        {
            guardAnim.enabled = true;
            agent.isStopped = false;
        }

        DrawAngle();

        if(GameManagerScript.pause == true || GameManagerScript.gameOver == true || GameManagerScript.win == true)
        {
            guardAudioSource.Pause();
        }
        if(GameManagerScript.pause == false)
        {
            guardAudioSource.UnPause();
        }
        if(GameManagerScript.SFXToggle.isOn == false)
        {
            guardAudioSource.Pause();
        }
    }

    //Animations
    private void LateUpdate()
    {
        guardAnim.SetBool("Guard_Walking", guard_Walking);
        guardAnim.SetBool("Guard_Running", guard_Running);
        guardAnim.SetBool("Guard_Attack", guard_Attack);
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        playerInVisionRange = Physics.CheckSphere(pos, visionRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(pos, attackRange, playerLayer);

        if (!oldplayerInVisionRange && playerInVisionRange && !playerInAttackRange)
        {
            //OnEnter System
            isPlaying = false;
        }       

        if (!oldplayerInAttackRange && playerInAttackRange && playerInVisionRange)
        {
            //OnEnter System
            isPlaying = false;
            GuardSound(2, 0.602f);
        }

        if ((oldplayerInVisionRange && !playerInVisionRange) || (oldplayerInAttackRange && !playerInAttackRange))
        {
            //OnExit System
            isPlaying = false;
        }

        oldplayerInVisionRange = playerInVisionRange;
        oldplayerInAttackRange = playerInAttackRange;
    }

    public void DisablePlaying()
    {
        isPlaying = false;
    }
    private IEnumerator Repeat_Sound(int value, float SFXDuration)
    {   
        if(GameManagerScript.pause == false)
        {
            guardAudioSource.Stop();
            isPlaying = true;
            guardAudioSource.PlayOneShot(GuardSFX[value]);
            yield return new WaitForSeconds(SFXDuration);
            isPlaying = false;
        }       
    }

    private void GuardSound(int value, float SFXDuration)
    {
        if (isPlaying == false)
        {
            StartCoroutine(Repeat_Sound(value, SFXDuration));
        }       
    }

    private void FollowPatrolRoute()
    {
        guard_Running = false;
        guard_Attack = false;

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

                if (guard_Attack == false)
                {
                    StartCoroutine(Idle_Cooldown());
                }
            }

            if(chasing == true) // To return to the path as we set the destination in the coroutine
            {
                chasing = false;
                nextPoint++;
                transform.LookAt(points[nextPoint].position);
                agent.SetDestination(points[nextPoint].position);
            }
        }
    }

    private IEnumerator Idle_Cooldown()
    {
        //To look to the direction the agent is following
        transform.LookAt(points[nextPoint].position);
        Debug.Log("not chasing");

        Timer = Random.Range(3f, 8f);
        int RandIndx = Random.Range(0, 2);

        if (RandIndx == 0)
        {
            guard_Walking = true;
            canMove = true;
        }
        if (RandIndx == 1)
        {
            guard_Walking = false;
            isPlaying = false;
            yield return new WaitForSeconds(Timer);
            canMove = true;
        }

        agent.SetDestination(points[nextPoint].position);
    }

    private void Chase()
    {
        chasing = true;
        guard_Running = true;
        guard_Attack = false;
        guard_Walking = false;

        //The agent will leave the route and inmediately follow the player to try to stop it
        agent.SetDestination(player.position);
        transform.LookAt(player.transform.GetChild(0));
    }

    private void Attack()
    {
        if(PlayerControllerScript.hasBeenAttacked == false)
        {
            PlayerControllerScript.hasBeenAttacked = true;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f); //The guard faces ther player
            transform.LookAt(player.transform.GetChild(0));

            agent.stoppingDistance = 6.5f;
            agent.velocity = new Vector3(0, 0f, 0f);
            agent.isStopped = true;

            agent.SetDestination(myCam.transform.position); //Distance the guard has to respect with the player

            guard_Attack = true;
            guard_Running = false;
            guard_Walking = false;

            //We make sure that if we have just jumped over the camera no matter how much it is disabled (GAMEover function) it is still looking at the guard at his eye level.     
            myCam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            if (camPrueba == true)
            {
                myCam.transform.LookAt(guardEyes); //The player looks at the guard
                camPrueba = false;
            }
            myCamAnim.enabled = true; //The guard make an uppercut to the player and the camera moves with the punch       
            StartCoroutine(GameManagerScript.GameOver());
        }
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

    private void DrawAngle()
    {
        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        //Draws a line for each angle Limit
        Debug.DrawLine(transform.position, transform.position + viewAngle01 * radius, Color.yellow);
        Debug.DrawLine(transform.position, transform.position + viewAngle02 * radius, Color.yellow);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
}
