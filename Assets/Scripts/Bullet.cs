using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  public float speed = 200f;
  public int damage = 10;
  public Rigidbody2D m_Rigidbody2D;
  public GameObject impactEffect;
  public float speedmul;
  public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
      m_Rigidbody2D.velocity = transform.right * speed * speedmul;
      Destroy(bullet, 2);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
      if (hitInfo.gameObject.CompareTag("Enemy"))
      {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
          enemy.TakeDamage(damage);
          Destroy(bullet);
        }
      }
      else if (hitInfo.gameObject.CompareTag("Boss"))
      {
        Boss boss = hitInfo.GetComponent<Boss>();
        if (boss != null)
        {
          boss.TakeDamage(damage);
          Destroy(bullet);
        }
      }
      if (hitInfo.gameObject.CompareTag("World")  )
      {
        Destroy(bullet);
      }

    }
}
