using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("LayerMasks")]
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    [Header("Patrolling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("States")]
    public float sightRange;
    public float attackRange;

    [Header("Range Control")]
    public bool playerInSightRange;
    public bool playerInAttackRange;

    [Header("Attack")]
    public float damage;
    public bool isDead;
    public GameObject hitBox;

    [Header("Audio Clips")]
    public AudioClip moveAudio;
    public AudioClip attackAudio;

    NavMeshAgent agent;
    Transform player;
    Animator anim;
    EnemyManager enemyManager;
    AudioSource source;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        player = GameObject.FindAnyObjectByType<PlayerManager>().transform;

        enemyManager = GetComponent<EnemyManager>();
        source = GetComponent<AudioSource>();

        hitBox.SetActive(false);

        InvokeRepeating(nameof(ScreamVoice), 3f, 10f);
    }

    private void Update()
    {
        if (!isDead)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patrolling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }

        if (enemyManager.enemyHealth <= 0 && !isDead)
        {
            anim.SetBool("Dead", true);
            agent.speed = 0;
            isDead = true;

            source.Stop(); 

            Destroy(this.gameObject, 2f);
        }
    }

    public void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            anim.SetBool("Attack", false);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    public void ChasePlayer()
    {
        anim.SetBool("Attack", false);
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void AttackPlayer()
    {
        if (!anim.GetBool("Attack"))
        {
            anim.SetBool("Attack", true);
            agent.SetDestination(transform.position);
        }
    }

    public void HitPlayer()
    {
        if (player != null)
        {
            player.GetComponent<PlayerManager>().playerHealth -= damage;

            source.clip = attackAudio;

            source.pitch = 1f;
            source.Play();
        }
    }

    public void EnableHitBox()
    {
        hitBox.SetActive(true);
    }

    public void DisableHitBox()
    {
        hitBox.SetActive(false);
    }

    public void ScreamVoice()
    {
        if (isDead || anim.GetBool("Attack")) return;

        source.pitch = 1f;

        source.clip = moveAudio;
        source.Play();
    }
}