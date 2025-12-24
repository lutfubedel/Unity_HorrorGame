using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float enemyHealth;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void TakeDamage(float damage)
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= damage;
            anim.SetTrigger("Hit");
        }
    }
}
