using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Seeker))]
public class Rocket : MonoBehaviour
{
  public float speed = 50f;
  public int damage = 15;
  public Rigidbody2D m_Rigidbody2D;
  public GameObject impactEffect;
  public float speedmul;
  public GameObject rocket;
  public float thrust = 5f;
  public PlayerStats player;
  public Seeker seeker;
  public Path path;
  public ForceMode2D fMode = ForceMode2D.Impulse;
  public Transform target;
  public GameObject targetgm;
  public float norSpeed = 1f;
  public float updateRate = 1f;
  public Vector2 dir;
  public float vermul;

  [HideInInspector]
  public bool pathIsEnded = false;

  public float nextWaypointDistance = 4;
  public int currentWaypoint = 0;
  public bool searchingForPlayer = false;

  // Start is called before the first frame update
  void Start()
  {
    seeker = GetComponent<Seeker>();

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
      if(pathIsEnded)
      {
        OnPathComplete(path);
      }
      pathIsEnded = true;
      return;
    }
    pathIsEnded = false;

    Vector3 targetPosition = target.position;
    float targetDistance = Vector3.Distance(transform.position, targetPosition);
    if (targetDistance >= 10f)
    {
      dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
      dir *= norSpeed;
      m_Rigidbody2D.AddForce(dir, fMode);
      vermul = Random.Range(-1, 1);
      vermul *= 0.1f;
      m_Rigidbody2D.AddForce(transform.up * vermul, fMode);
    }
    else
    {
      m_Rigidbody2D.AddForce(dir, fMode);
    }
  }
}
