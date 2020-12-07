using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAITest : MonoBehaviour
{
    public NavMeshAgent enemy; 
    
    public LayerMask groundIndicator, playerIndicator;
    public float attackTimeDelta;
    //public float walkTimeDelta;
    public float visionRange, attackRange;
    public float targetHealth; 
    //enemy health
    
    public bool playerWithinVision, playerWithinAttackRange; 
    //for the enemy to decide whether to chase or attack
    bool attackExecuted; 
    //stops spamming attacks

    bool walkExecuted;

    public int attackCondition; 
    //identifies which animation is used for the attack
    public float enemyRange = 15f;
    //This is how far the enemy can see

    Transform target; 
    //Player reference

    NavMeshAgent agent;
    //NavMeshAgent reference
    public Animator animator;

    bool enemyDead;

    public float distance;
    
    
    [SerializeField] private playerHealth health;
    private void Start()
    {
        // Initialises the player object locally + enemy
        enemy = GetComponent<NavMeshAgent>();

        // Initialise the animation controller for the enemy
        animator=GetComponent<Animator>();

        target = PlayerManager.instance.player.transform;
        // this is a script that keeps track of where the player is
        
        agent = GetComponent<NavMeshAgent>(); 
        //game object component belonging to enemy

        enemyDead = false;

        
    }

    private void Update()
    {
        // Boolean check fields to determine whether player is within enemy vision/attack range
        playerWithinVision = Physics.CheckSphere(transform.position, visionRange, playerIndicator);
        playerWithinAttackRange = Physics.CheckSphere(transform.position, attackRange, playerIndicator);

        
        if (enemyDead == false && health.health>0){
            //if the player is not dead

            distance = Vector3.Distance(target.position, transform.position);
            //distance between the player and enemy

            // Appropriate enemy actions based on boolean check field permutations
            if (playerWithinVision && !playerWithinAttackRange) chasePlayer();
            if (playerWithinVision && playerWithinAttackRange) EnemyAttackPlayer();
        }  
        
    }

    void chasePlayer(){
        

        if (distance<=enemyRange){
            
            FacePlayer();
            agent.SetDestination(target.position);;
            //chase player if the player is within range
            
            //Debug.Log("Chasing");
            if (enemyDead==false && walkExecuted==false ){
                animator.SetInteger("condition", 1);
                //only play animation of the player is alive
                
                FindObjectOfType<audioManager>().Play("EnemyWalking"); //choose which sound to play
                walkExecuted=true;
                Invoke(nameof(EnemyResetWalk), attackTimeDelta);

            }
            
            if (distance<=agent.stoppingDistance){
               
                FacePlayer();
                //if the enemy is close enough then face the player

            }
            
        }
        
    }
    
    void FacePlayer(){
        Vector3 direction = (target.position - transform.position).normalized;
        //player direction

        Quaternion rotate = Quaternion.LookRotation(new Vector3(direction.x,0, direction.z));
        //which way we rotate

        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 5f);
        //update our own direction
    }

    private void EnemyAttackPlayer()
    {
   
        if (!attackExecuted)
        {
 
            Debug.Log("attacking player");

            if (enemyDead==false && health.health >0){
                animator.SetInteger("condition", attackCondition);
                //if the player is not dead change the animation
                FindObjectOfType<audioManager>().Play("EnemyAttacking"); //choose which sound to play
            }
            else{
                animator.SetInteger("condition", 0);
                FindObjectOfType<audioManager>().Play("Silence");
            }

            health.receiveDamage(10); 
            //making damage to the player

            attackExecuted = true;
            Invoke(nameof(EnemyResetAttack), attackTimeDelta);
            //used to stop spam attacks
        }
   
    }

    private void EnemyResetAttack()
    {
        attackExecuted = false;
        
    }

    private void EnemyResetWalk()
    {
        walkExecuted = false;
        
    }

    public void takeDamage(float damage)
    {
        targetHealth -= damage;
        //take away from enemy health

        if (targetHealth <= 0f)
        {
            if(enemyDead==false){
                //Debug.Log("enemy has died");
                //health.displayDeath();
                agent.baseOffset=0;
                enemy.speed=0;
                enemy.acceleration=0;
                animator.SetInteger("condition", 3);
                enemyDead=true;
                //plays death animation and stops enemy from moving
            }
            
        }
    }

}
