using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackAI : MonoBehaviour
{
  public enum State
  {
    Idle,
    Attacking,
    Moving,
  }

  public enum AttackState
  {
    Flamethrower,
    Rocket,
    Groundslam,
    Melee,
  }

  public State state;
  public AttackState attstate;
  public PlayerStats player;
  public Boss boss;
  Animator animator;
  public Transform target;
  public GameObject targetgm;
  public Rigidbody2D m_Rigidbody2D;
  public float norSpeed = 2f;
  public ForceMode2D fMode = ForceMode2D.Impulse;
  public Transform bossFirepointP;
  public Transform bossFirepoint;
  public Transform bossRocketFirepoint;
  public Transform bossGroundslampoint;
  public Transform[] bossFlamethrowerpoint;
  public int flamestage = 0;
  public Vector2[] flamesize = {new Vector2 (0.75f, 0.75f), new Vector2 (2.75f, 1f), new Vector2 (3.7f, 1.95f), new Vector2 (6.55f, 2.25f), new Vector2 (12.75f, 3f), new Vector2 (13.7f, 3.5f), new Vector2 (14.25f, 2.65f), new Vector2 (13.7f, 3.65f), new Vector2 (14f, 2.5f)};
  public LayerMask playerLayers;
  public bool flamehit = false;
  public GameObject rocketPrefab;
  public GameObject shockPrefab;
  public float cooldown = 1f;
  public bool canattack = false;
  public bool canshoot = false;
  public float minattackDistance = 5f;
  public float maxattackDistance = 15f;
  public float thrust = 5f;
  public int lor;
  public bool searchingForPlayer = false;
  public int attacktype;
  public float ftoffset;
  public bool doingflame = false;
  public int flamechange = 0;
  public float angle = 0;
  public float flameangleoffset = 0f;
  public Vector2 flamepoint = new Vector2 (0f, 0f);
  public Vector3 targetPosition;
  public bool forward = false;
  public bool back = false;
  public bool isdead = false;
  public AudioClip[] clips;

  public GameManager gm;
  public GameObject gameM;

  public void Start()
  {
    gameM = GameObject.Find("GameManager");
    gm = gameM.GetComponent<GameManager>();
    animator = GetComponent<Animator>();
    boss = GetComponent<Boss>();
    boss.bosshealth = 1500;
    boss.maxbosshealth = 1500;
    m_Rigidbody2D = GetComponent<Rigidbody2D>();
    state = State.Idle;
    attstate = AttackState.Rocket;
    bossFirepointP = GameObject.Find("Boss FireArm").transform;
    bossFirepoint = bossFirepointP.Find("Boss FirePoint");
    bossRocketFirepoint = GameObject.Find("Rocket Firepoint").transform;
    bossGroundslampoint = GameObject.Find("GroundStomp Point").transform;
    for (int i = 0; i < 9; i++)
    {
      bossFlamethrowerpoint[i] = GameObject.Find("Boss Flamethrower " + (i+1).ToString()).transform;
    }
    if (target == null)
    {
      if (!searchingForPlayer)
      {
        searchingForPlayer = true;
        StartCoroutine(SearchForPlayer());
      }
      return;
    }
  }

  IEnumerator SearchForPlayer()
  {
    GameObject sResult = GameObject.FindWithTag("Player");
    if (sResult == null)
    {
      yield return new WaitForSeconds(0.5f);
      StartCoroutine(SearchForPlayer());
    }
    else
    {
      targetgm = sResult;
      target = sResult.transform;
      player = targetgm.GetComponent<PlayerStats>();
      searchingForPlayer = false;
      yield return false;
    }
  }

  void FixedUpdate()
  {
    if (!isdead)
    {
      float speedInUnitPerSecond = m_Rigidbody2D.velocity.magnitude;
      animator.SetFloat("VelocityX", Mathf.Abs(speedInUnitPerSecond));
      if (target == null)
      {
        if (!searchingForPlayer)
        {
          searchingForPlayer = true;
          StartCoroutine(SearchForPlayer());
        }
      }
    }
  }

  void Update()
  {
    if (!isdead)
    {
      Flamethrower();
      Vector3 targetPosition = target.position;
      float targetDistance = Vector3.Distance(transform.position, targetPosition);
      switch (state)
      {
        case State.Idle:
          if (gm.inbossroom)
          {
            CancelInvoke("Lazer");
            if (targetDistance < minattackDistance || targetDistance > maxattackDistance)
            {
              state = State.Moving;
              if (targetDistance < minattackDistance)
              {
                back = true;
              }
              else if (targetDistance > maxattackDistance)
              {
                forward = true;
              }
            }
            else
            {
              if (!canattack)
              {
                canattack = true;
                StartCoroutine(AttackCountdown());
                attacktype = Random.Range(1, 20);
                if(attacktype <= 8)
                {
                  attackpattern("Lazer");
                }
                else if (attacktype > 8 && attacktype <= 12)
                {
                  attackpattern("Flamethrower");
                }
                else if (attacktype > 12 && attacktype <= 18)
                {
                  attackpattern("Rocket");
                }
                else
                {
                  attackpattern("Groundslam");
                }
              }
            }
          }
          break;
        case State.Attacking:
          if (boss != null)
          {
            // if (gm.inbossroom && gm.enteredbossroom)
            // {
            //   state = State.Idle;
            // }
            if (targetDistance < minattackDistance || targetDistance > maxattackDistance)
            {
              state = State.Moving;
              if (targetDistance < minattackDistance)
              {
                back = true;
              }
              else if (targetDistance > maxattackDistance)
              {
                forward = true;
              }
            }
            else
            {
              if (!canattack)
              {
                canattack = true;
                StartCoroutine(AttackCountdown());
                attacktype = Random.Range(1, 20);
                if (attacktype > 0 && attacktype <= 8)
                {
                  attackpattern("Flamethrower");
                }
                else if (attacktype > 8 && attacktype <= 15)
                {
                  attackpattern("Rocket");
                }
                else
                {
                  attackpattern("Groundslam");
                }
              }
            }
          }
          break;
        case State.Moving:
          if (forward)
          {
            targetPosition = target.transform.position;
            Vector2 dir = (targetPosition - transform.position).normalized;
            dir *= gm.bossspeed;
            forward = false;
            m_Rigidbody2D.AddForce(dir, fMode);
            state = State.Idle;
          }
          else if (back)
          {
            Vector2 dir = (targetPosition - transform.position).normalized;
            dir *= gm.bossspeed;
            back = false;
            m_Rigidbody2D.AddForce(dir, fMode);
            state = State.Idle;
          }
          break;
      }
      if (transform.position.x - target.position.x < 0)
      {
        transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
      }
      else
      {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
      }
    }
  }

  void attackpattern(string attackname)
  {
    if (attackname == "Flamethrower")
    {
      attstate = AttackState.Flamethrower;
    }
    else if (attackname == "Rocket")
    {
      attstate = AttackState.Rocket;
    }
    else if (attackname == "Melee")
    {
      attstate = AttackState.Melee;
    }
    else
    {
      attstate = AttackState.Groundslam;
    }
    switch (attstate)
    {
      case AttackState.Flamethrower:
          doingflame = true;
        break;
      case AttackState.Rocket:
          StartCoroutine(RocketFire());
        break;
      case AttackState.Groundslam:
        Groundslam();
        break;
    }
  }

  void Groundslam()
  {
    animator.SetTrigger("GroundStomp");
  }

  void StartShockwave()
  {
    Instantiate(shockPrefab, bossGroundslampoint.position, new Quaternion (0f, 0f, 0f, 0f));
    Instantiate(shockPrefab, bossGroundslampoint.position, new Quaternion (0f, 0f, 0f, 0f));
  }

  void Flamethrower()
  {
    if (doingflame)
    {
      if (flamechange == 0)
      {
        flamechange++;
        flamestage = 0;
        targetPosition = target.transform.position;
        Vector3 dir = (targetPosition - bossFirepointP.position).normalized;
        if (transform.localScale.x == 1)
        {
          angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + ftoffset;
        }
        else if (transform.localScale.x == -1)
        {
          angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
        Quaternion rotateToTarget = Quaternion.AngleAxis(angle, Vector3.forward);
        animator.SetTrigger("Flamethrower");
        bossFirepointP.rotation = Quaternion.Slerp(bossFirepointP.rotation, rotateToTarget, Time.deltaTime * 10000f);
      }
      StartCoroutine(Flamehit());
    }
  }

  IEnumerator Flamehit()
  {
    flamepoint = new Vector2 (bossFlamethrowerpoint[flamestage].position.x, bossFlamethrowerpoint[flamestage].position.y);
    Vector3 fdir = (targetPosition - bossFlamethrowerpoint[flamestage].position).normalized;
    float fangle = Mathf.Atan2(fdir.y, fdir.x) * Mathf.Rad2Deg + flameangleoffset;
    Collider2D hit = Physics2D.OverlapBox(flamepoint, flamesize[flamestage], fangle, playerLayers);
    if (hit != null)
    {
      PlayerStats player = hit.GetComponent<PlayerStats>();
      if (!flamehit)
      {
        StartCoroutine(FlameDmg(player));
      }
    }
    else
    {
      yield return new WaitForSeconds(0.0001f);
    }
  }

  IEnumerator FlameDmg(PlayerStats mc)
  {
    flamehit = true;
    mc.TakeDamage(gm.flamedmg);
    yield return new WaitForSeconds(gm.flamecooldown);
    flamehit = false;
  }

  IEnumerator RocketFire()
  {
    for (int i = 0; i < 6; i++)
    {
      Instantiate(rocketPrefab, bossRocketFirepoint.position, bossRocketFirepoint.rotation);
      yield return new WaitForSeconds(0.1f);
    }
  }

  IEnumerator AttackCountdown()
  {
    yield return new WaitForSeconds(3.5f);
    canattack = false;
  }

  void nextflamestage()
  {
    flamestage++;
  }

  void endflame()
  {
    doingflame = false;
    flamechange = 0;
    bossFirepointP.rotation = new Quaternion (0f, 0f, 0f, 0f);
  }

  void OnDrawGizmosSelected()
  {
    Vector2 flamepoint = new Vector2 (bossFlamethrowerpoint[flamestage].position.x, bossFlamethrowerpoint[flamestage].position.y);
    Gizmos.DrawWireCube(flamepoint, flamesize[flamestage]);
  }

  void setcanattack()
  {
    canattack = false;
  }

  void SetDead()
  {
    isdead = true;
  }
}
