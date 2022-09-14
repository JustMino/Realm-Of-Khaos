using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour
{
  public Transform target;

  public float updateRate = 1f;

  private Seeker seeker;
  private Rigidbody2D m_Rigidbody2D;

  public Path path;

  public float speed = 20f;
  public ForceMode2D fMode;

  [HideInInspector]
  public bool pathIsEnded = false;

  public float nextWaypointDistance = 3;

  private int currentWaypoint = 0;

  private bool searchingForPlayer = false;

  private float thrust = 20f;


  void Start()
  {
    seeker = GetComponent<Seeker>();
    m_Rigidbody2D = GetComponent<Rigidbody2D>();

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

    StartCoroutine(UpdatePath());
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
      target = sResult.transform;
      searchingForPlayer = false;
      StartCoroutine(UpdatePath());
      yield return false;
    }
  }

  IEnumerator UpdatePath()
  {
    if (target == null)
    {
      if(!searchingForPlayer)
      {
        searchingForPlayer = true;
        StartCoroutine(SearchForPlayer());
      }
      yield return false;
    }
    seeker.StartPath (transform.position, target.position, OnPathComplete);

    yield return new WaitForSeconds(1f/updateRate);
    StartCoroutine(UpdatePath());
  }

  public void OnPathComplete(Path p)
  {
    if(!p.error)
    {
      path = p;
      currentWaypoint = 0;
    }
  }

  void FixedUpdate()
  {
    if(target == null)
    {
      if(!searchingForPlayer)
      {
        searchingForPlayer = true;
        StartCoroutine(SearchForPlayer());
      }
    }

    if (path == null)
    {
      return;
    }

    if(currentWaypoint >= path.vectorPath.Count)
    {
      if(pathIsEnded)
      {
        OnPathComplete(path);
      }

      pathIsEnded = true;
      return;
    }
    pathIsEnded = false;

    Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
    dir *= speed;// * Time.fixedDeltaTime;

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
      m_Rigidbody2D.AddForce(transform.up * thrust, ForceMode2D.Impulse);
    }
  }
}
