using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket2 : MonoBehaviour
{
  GameObject target;
  public GameObject explosion;
  public GameObject midairexplosion;
  public float rotationSpeed = 10000f;
  public float offset = 90f;
  public float speedmul = 10f;
  public bool done = false;

  Quaternion rotateToTarget;
  Vector3 dir;
  public ForceMode2D fMode = ForceMode2D.Impulse;

  Rigidbody2D m_Rigidbody2D;

  // Start is called before the first frame update
  void Start()
  {
    target = GameObject.FindWithTag("Player");
    m_Rigidbody2D = GetComponent<Rigidbody2D>();
    m_Rigidbody2D.AddForce(transform.up * 200f, fMode);
  }

  // Update is called once per frame
  void Update()
  {
    Vector3 targetPosition = target.transform.position;
    float targetDistance = Vector3.Distance(transform.position, targetPosition);
    if (targetDistance >= 10f && !done)
    {
      dir = (targetPosition - transform.position).normalized;
      float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + offset;
      rotateToTarget = Quaternion.AngleAxis(angle, Vector3.forward);
      transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);

      m_Rigidbody2D.velocity = new Vector2(dir.x * speedmul, dir.y * speedmul);
    }
    else
    {
      done = true;
      m_Rigidbody2D.AddForce(dir * 0.4f, fMode);
    }
  }

  void OnTriggerEnter2D(Collider2D col)
  {
    if (col.gameObject.CompareTag("World"))
    {
      Instantiate(explosion, transform.position, Quaternion.identity);
      Destroy(this.gameObject);
    }
    else if (col.gameObject.CompareTag("Player"))
    {
      Instantiate(midairexplosion, transform.position, Quaternion.identity);
      Destroy(this.gameObject);
    }
  }
}
