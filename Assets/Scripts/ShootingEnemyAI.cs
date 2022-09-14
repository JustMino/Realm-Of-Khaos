using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Seeker))]
public class ShootingEnemyAI : MonoBehaviour
{
  public enum State
  {
    Attacking,
    Movingto,
    Movingback,
    Busy
  }

  private State state;
  public Enemy enemy;
  public PlayerStats player;
  public Transform target;
  public GameObject targetgm;
  public float updateRate = 1f;
  public Seeker seeker;
  public Rigidbody2D m_Rigidbody2D;
  public Path path;
  public float norSpeed = 2f;
  public ForceMode2D fMode = ForceMode2D.Impulse;
  public Transform enemyFirepointP;
  public Transform enemyFirepoint;
  public GameObject bulletPrefab;
  public float cooldown = 1f;
  public bool canshoot = false;
  public float minattackDistance = 10f;
  public float maxattackDistance = 15f;
  public bool movingto = false;
  public bool movingback = false;
  public float thrust = 5f;
  public int lor;
  Animator animator;

  public bool dead = false;

  AudioSource audioS;
  public AudioClip[] clips;

  [HideInInspector]
  public bool pathIsEnded = false;

  public float nextWaypointDistance = 4;
  public int currentWaypoint = 0;
  public bool searchingForPlayer = false;

  public void Start()
  {
    audioS = GetComponent<AudioSource>();
    animator = GetComponent<Animator>();
    enemy = GetComponent<Enemy>();
    enemy.enemyID = 2;
    enemy.maxenemyhealth = 100;
    seeker = GetComponent<Seeker>();
    m_Rigidbody2D = GetComponent<Rigidbody2D>();
    state = State.Attacking;
    enemyFirepointP = transform.Find("enemyFirepoint Pivot");
    enemyFirepoint = enemyFirepointP.Find("enemyFirepoint");
    InvokeRepeating("Shoot", 0.0f, cooldown);
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
    if (!dead)
    {
      if (target == null)
      {
        if (!searchingForPlayer)
        {
          searchingForPlayer = true;
          StartCoroutine(SearchForPlayer());
        }
      }

      if (path == null)
      {
        return;
      }

      if (currentWaypoint >= path.vectorPath.Count)
      {
        if(pathIsEnded)
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
        dir *= norSpeed;// * Time.fixedDeltaTime;
        movingto = false;
        m_Rigidbody2D.AddForce(dir, fMode);
      }
      if (movingback)
      {
        Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= norSpeed * -1f;// * Time.fixedDeltaTime;
        movingback = false;
        m_Rigidbody2D.AddForce(dir, fMode);
      }
      if (transform.position.y < target.position.y)
      {
        m_Rigidbody2D.AddForce(transform.up * thrust, fMode);
      }
      lor = Random.Range(1, 15);
      if (lor == 7)
      {
        m_Rigidbody2D.AddForce(transform.right * thrust * -1f, fMode);
      }
      if(lor == 11)
      {
        m_Rigidbody2D.AddForce(transform.right * thrust, fMode);
      }
      float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
      if (dist < nextWaypointDistance)
      {
        currentWaypoint++;
        return;
      }
    }
  }

  void Shoot()
  {
    if (!dead)
    {
      if (!canshoot)
      {
        return;
      }
      animator.SetTrigger("Bat Attack");
    }
  }

  void Update()
  {
    if (!dead)
    {
      Vector3 targetPosition = target.position;
      float targetDistance = Vector3.Distance(transform.position, targetPosition);
      switch (state)
      {
        case State.Attacking:
          if (enemy != null)
          {
            if (targetDistance < minattackDistance)
            {
              canshoot = false;
              state = State.Movingback;
            }
            else if (targetDistance > maxattackDistance)
            {
              canshoot = false;
              state = State.Movingto;
            }
            else
            {
              canshoot = true;
            }
          }
        break;
      case State.Movingto:
        movingto = true;
        if (targetDistance > minattackDistance && targetDistance < maxattackDistance)
        {
          state = State.Attacking;
        }
        break;
      case State.Movingback:
        movingback = true;
        if (targetDistance < maxattackDistance)
        {
          state = State.Attacking;
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

  void FireProjectile()
  {
    audioS.clip = clips[1];
    audioS.Play();
    Instantiate(bulletPrefab, enemyFirepoint.position, enemyFirepointP.rotation);
  }

  void SetDead()
  {
    dead = true;
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
