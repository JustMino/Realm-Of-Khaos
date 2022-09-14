using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public int enemyhealth;
  public int maxenemyhealth;
  public int enemyID = 0;
  GameManager gm;
  public GameObject gameM;
  public GameObject deathEffect;
  public GameObject enemy;
  Animator animator;
  public bool dead = false;
  // EnemyHealth enemyh;

  void Start()
  {
    gameM = GameObject.Find("GameManager");
    gm = gameM.GetComponent<GameManager>();
    animator = GetComponent<Animator>();
    // enemyh = GetComponent<EnemyHealth>();
    if (enemyID == 1)
    {
      enemyhealth = gm.cenchargemaxhealth;
      maxenemyhealth = gm.cenchargemaxhealth;
    }
    else if (enemyID == 2)
    {
      enemyhealth = gm.flyingmaxhealth;
      maxenemyhealth = gm.flyingmaxhealth;
    }
    else if (enemyID == 3)
    {
      enemyhealth = gm.catmaxhealth;
      maxenemyhealth = gm.catmaxhealth;
    }
    else if (enemyID == 4)
    {
      enemyhealth = gm.slashermaxhealth;
      maxenemyhealth = gm.slashermaxhealth;
    }
    else if (enemyID == 0)
    {
      StartCoroutine(CheckEnemyType());
    }
  }

  IEnumerator CheckEnemyType()
  {
    if (enemyID == 1)
    {
      enemyhealth = gm.cenchargemaxhealth;
      maxenemyhealth = gm.cenchargemaxhealth;
    }
    else if (enemyID == 2)
    {
      enemyhealth = gm.flyingmaxhealth;
      maxenemyhealth = gm.flyingmaxhealth;
    }
    else if (enemyID == 3)
    {
      enemyhealth = gm.catmaxhealth;
      maxenemyhealth = gm.catmaxhealth;
    }
    else if (enemyID == 4)
    {
      enemyhealth = gm.slashermaxhealth;
      maxenemyhealth = gm.slashermaxhealth;
    }
    else if (enemyID == 0)
    {
      yield return new WaitForEndOfFrame();
      StartCoroutine(CheckEnemyType());
    }
  }

  public void TakeDamage(int damage)
  {
    enemyhealth -= damage;

    if (enemyhealth <= 0 && !dead)
    {
      dead = true;
      animator.SetTrigger("Death");
    }
  }

  void Die()
  {
    Destroy(enemy, 0.1f);
  }
}
