using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFlame : MonoBehaviour
{
  public float speed = 100f;
  public int damage = 10;
  public Rigidbody2D m_Rigidbody2D;
  public GameObject impactEffect;
  public float speedmul;
  public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
      m_Rigidbody2D.velocity = transform.right * speed * speedmul;
      Destroy(bullet, 0.25f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
      Enemy enemy = hitInfo.GetComponent<Enemy>();
      if (enemy != null)
      {
        enemy.TakeDamage(damage);
      }
      if (hitInfo.gameObject.CompareTag("World") || hitInfo.gameObject.CompareTag("Enemy"))
      {
        Destroy(bullet);
      }

    }
}
