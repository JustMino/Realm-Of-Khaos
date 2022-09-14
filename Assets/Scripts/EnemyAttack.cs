using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAttack : MonoBehaviour
{

  private enum State
  {
    Roaming,
    TrackingPlayer,
    AttackingPlayer,
    Dead
  }

  public Enemy enemy;
  public PlayerStats player;
  public Transform target;
  public GameObject targetgm;
  public float updateRate = 1f;
  public Seeker seeker;
  public Rigidbody2D m_Rigidbody2D;
  public Path path;
  public float norSpeed = 0f;
  public ForceMode2D fMode = ForceMode2D.Impulse;

  public bool pathIsEnded = false;

  public float nextWaypointDistance = 4;
  public int currentWaypoint = 0;
  public bool searchingForPlayer = false;

  public Vector3 changeDir;

  public bool Attacking = false;
  public bool hitplayer = false;
  public bool canhitplayer = true;
  public float thrust = 20f;

  // public State state;

  public void Start()
  {
    enemy = GetComponent<Enemy>();
    enemy.enemyhealth = 100;
    enemy.maxenemyhealth = 100;
    seeker = GetComponent<Seeker>();
    m_Rigidbody2D = GetComponent<Rigidbody2D>();
    // State = State.Roaming;
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
      if (pathIsEnded)
      {
        OnPathComplete(path);
      }

      pathIsEnded = true;
      return;
    }
    pathIsEnded = false;

    Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
    dir *= norSpeed;

    m_Rigidbody2D.AddForce(dir, fMode);

    float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
    if (dist < nextWaypointDistance)
    {
      currentWaypoint++;
      return;
    }
  }

  void OnTriggerEnter2D(Collider2D col)
  {
    if (col.gameObject.CompareTag("World"))
    {
      m_Rigidbody2D.AddForce(transform.up * thrust, fMode);
    }
  }
}
