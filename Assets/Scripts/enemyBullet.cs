using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
  public float speed = 200f;
  public int damage = 5;
  public Rigidbody2D m_Rigidbody2D;
  public GameObject impactEffect;
  public float speedmul;
  public GameObject bullet;
  public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
      m_Rigidbody2D.velocity = transform.right * speed * speedmul;
      player = GameObject.FindWithTag("Player");
      Destroy(bullet, 2);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
      PlayerStats p = player.GetComponent<PlayerStats>();
      if (hitInfo.gameObject.CompareTag("Player"))
      {
        p.TakeDamage(damage);
      }
      if (hitInfo.gameObject.CompareTag("World") || hitInfo.gameObject.CompareTag("Player"))
      {
        Destroy(bullet);
      }

    }
}
