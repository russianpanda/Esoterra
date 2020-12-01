﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float enemyRange = 15f;
    //This is how far the enemy can see

    Transform target; 
    //Player reference

    NavMeshAgent agent;
    //NavMeshAgent reference

    //CharacterCombat combat;
    //reference to the CharacterCombat script for the enemy

    [SerializeField] private enemyAnimations condition;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        // this is a script that keeps track of where the player is
        
        agent = GetComponent<NavMeshAgent>(); //game object component
        //combat = GetComponent<CharacterCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        //distance between the player and enemy

        if (distance<=enemyRange){
            agent.SetDestination(target.position);
            //chase player
            Debug.Log("Chasing");
            condition.setCondition(1);
            if (distance<=agent.stoppingDistance){
                //if the enemy is next to the player

                //CharacterStats targetStats = target.GetComponent<CharacterStats>();
                //Debug.Log("about to attack");
                //if (targetStats!= null){
                //    Debug.Log("attacking with b");
                    //combat.Attack(targetStats);
                //}
                
                
                FacePlayer();
                //face the player

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
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyRange);
    }
}
