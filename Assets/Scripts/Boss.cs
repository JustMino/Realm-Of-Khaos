using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
  public int bosshealth;
  public int maxbosshealth;
  public GameObject boss;

  public GameManager gm;
  public GameObject gameM;

  public BossHealthBar bhb;

  public bool ready = false;

  SpriteRenderer[] All;

  public bool dead = false;

  void Start()
  {
    All = GetComponentsInChildren<SpriteRenderer>();
    gameM = GameObject.Find("GameManager");
    gm = gameM.GetComponent<GameManager>();
    bosshealth = gm.bossmaxhealth;
    maxbosshealth = gm.bossmaxhealth;
    bhb.SetMaxHealth(maxbosshealth);
  }

  void Update()
  {
    if (gm.inbossroom)
    {
      if (bosshealth <= 0)
      {
        bhb.dead = true;
        bosshealth = 0;
      }
      else if (bosshealth > 0)
      {
        bhb.SetHealth(bosshealth);
      }
    }
    else
    {
      return;
    }
  }

  public void TakeDamage(int damage)
  {
    bosshealth -= damage;

    if (bosshealth <= 0 && !dead)
    {
      dead = true;
      GetComponent<Animator>().SetTrigger("Dead");
      foreach (SpriteRenderer SR in All)
      {
        SR.enabled = !SR.enabled;
      }
    }
  }

  void Die()
  {
    Destroy(boss, 0.1f);
    SceneManager.LoadScene(5, LoadSceneMode.Single);
  }
}
