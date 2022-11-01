using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Seeker))]
public class BasicEnemy : MonoBehaviour
{
  public enum State
  {
    Attacking,
    Movingto,
    Movingaway,
  }

  public State state;
  public Enemy enemy;
  public PlayerStats player;
  public Animator animator;
  public Transform target;
  public GameObject targetgm;
  public float updateRate = 1f;
  public Seeker seeker;
  public Rigidbody2D m_Rigidbody2D;
  public Path path;
  public ForceMode2D fMode = ForceMode2D.Impulse;
  public Transform attackPoint;
  public float thrust = 20f;
  public LayerMask playerLayers;
  public bool movingto = false;
  public bool movingaway = false;
  public bool canhitplayer = true;
  public bool countingdown = false;
  public bool hittingplayer = false;
  public bool isdead = false;

  SpriteRenderer spriteRenderer;
  GameManager gm;
  public GameObject gameM;

  AudioSource audioS;
  public AudioClip[] clips;

  //[HideInInspector]
  public bool pathIsEnded = false;

  public float nextWaypointDistance = 4;
  public int currentWaypoint = 0;
  public bool searchingForPlayer = false;

  public float targetDistance;

  public void Start()
  {
    gameM = GameObject.Find("GameManager");
    gm = gameM.GetComponent<GameManager>();
    animator = GetComponent<Animator>();
    audioS = GetComponent<AudioSource>();
    enemy = GetComponent<Enemy>();
    enemy.enemyID = 3;
    seeker = GetComponent<Seeker>();
    m_Rigidbody2D = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    state = State.Movingto;
    if (target == null)
    {
      if (!searchingForPlayer)
      {
        searchingForPlayer = true;
        StartCoroutine(SearchForPlayer());
      }
      return;
    }
    seeker.StartPath(transform.position, target.position, OnPathComplete);
    StartCoroutine(Screech());
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
      StartCoroutine(UpdatePath());
      yield return false;
    }
  }

  IEnumerator UpdatePath()
  {
    if (target == null)
    {
      if (!searchingForPlayer)
      {
        searchingForPlayer = true;
        StartCoroutine(SearchForPlayer());
      }
      yield return false;
    }
    seeker.StartPath(transform.position, target.position, OnPathComplete);
    yield return new WaitForSeconds(1f/updateRate);
    StartCoroutine(UpdatePath());
  }

  public void OnPathComplete(Path p)
  {
    if (!p.error)
    {
      path = p;
      currentWaypoint = 0;
    }
  }

  void FixedUpdate()
  {
    if (!isdead)
    {
      float speedInUnitPerSecond = m_Rigidbody2D.velocity.magnitude;
      animator.SetFloat("VelocityX", Mathf.Abs(speedInUnitPerSecond));
      if (target == null && !searchingForPlayer)
      {
        searchingForPlayer = true;
        StartCoroutine(SearchForPlayer());
      }
      if (path == null)
        {
          return;
        }

      if (currentWaypoint >= path.vectorPath.Count)
      {
        if (pathIsEnded)
        {
          OnPathComplete(path);
        }
        pathIsEnded = true;
        return;
      }
      pathIsEnded = false;

      if (movingto)
      {
        Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= gm.catspeed;
        movingto = false;
        m_Rigidbody2D.AddForce(dir, fMode);
      }
      else if (movingaway)
      {
        Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= gm.catspeed;
        movingaway = false;
        m_Rigidbody2D.AddForce(dir, fMode);
      }
      float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
      if (dist < nextWaypointDistance)
      {
        currentWaypoint++;
        return;
      }
    }
  }

  void Update()
  {
    if (!isdead)
    {
      Vector3 targetPosition = target.position;
      targetDistance = Vector3.Distance(transform.position, targetPosition);
      float maxAttackRange = gm.catattackDistance + gm.catattackRange;
      float minAttackRange = gm.catattackDistance - gm.catattackRange;
      switch (state)
      {
        case State.Attacking:
          if (enemy != null)
          {
            if (targetDistance > maxAttackRange)
            {
              state = State.Movingto;
            }
            else if(targetDistance < minAttackRange)
            {
              state = State.Movingaway;
            }
          }
          break;
        case State.Movingto:
          movingto = true;
          if (minAttackRange <= targetDistance && targetDistance <= maxAttackRange)
          {
            state = State.Attacking;
          }
          else if(targetDistance < minAttackRange)
          {
            state = State.Movingaway;
          }
          break;
        case State.Movingaway:
          movingaway = true;
          if (minAttackRange < targetDistance && targetDistance <= maxAttackRange)
          {
            state = State.Attacking;
          }
          else if (targetDistance > maxAttackRange)
          {
            state = State.Movingto;
          }
          break;
      }
      if (minAttackRange <= targetDistance && targetDistance <= maxAttackRange && canhitplayer)
      {
        animator.SetTrigger("CatAttack");
        hittingplayer = true;
        canhitplayer = false;
      }
      if (transform.position.x - target.position.x < 0)
      {
        transform.localScale = new Vector3(-2.0f, 2.0f, 2.0f);
      }
      else
      {
        transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
      }
    }
  }

  void Attack()
  {
    audioS.clip = clips[1];
    audioS.Play();
    Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, gm.catattackRange, playerLayers);
    if (hit != null)
    {
      PlayerStats playerh = hit.GetComponent<PlayerStats>();
      playerh.TakeDamage(gm.catdamage);
    }
    StartCoroutine(AttackCooldown());
    hittingplayer = false;
  }

  IEnumerator AttackCooldown()
  {
    yield return new WaitForSeconds(gm.catattackcooldown);
    canhitplayer = true;
  }

  void setDead()
  {
    isdead = true;
  }

  IEnumerator Screech()
  {
    audioS.clip = clips[0];
    audioS.Play();
    int time = Random.Range(5, 10);
    yield return new WaitForSeconds(time);
    StartCoroutine(Screech());
  }
}
