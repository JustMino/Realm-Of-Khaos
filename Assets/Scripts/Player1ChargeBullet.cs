using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1ChargeBullet : MonoBehaviour
{
  public float orbscale = 0.5f;
  bool increasing = true;
  Rigidbody2D rb;
  public float speed = 100f;
  public float basedmg = 50;
  GameObject firepoint;
  PlayerController player;
  int dir = 1;
  bool fired = false;
    // Start is called before the first frame update
    void Start()
    {
      firepoint = GameObject.Find("FirePoint");
      player = GameObject.Find("Player 1").GetComponent<PlayerController>();
      rb = GetComponent<Rigidbody2D>();
      transform.localScale = new Vector3 (orbscale, orbscale, orbscale);
      StartCoroutine(OrbChange());
    }

    // Update is called once per frame
    void Update()
    {
      if (!fired)
      {
        if (increasing)
        {
          transform.position = firepoint.transform.position;
        }
        if (Input.GetButtonUp("Fire2"))
        {
          fired = true;
          increasing = false;
          if (player.flipped)
          {
            transform.rotation = new Quaternion (0, 0, 0, 0);
          }
          else
          {
            transform.rotation = new Quaternion (0, 180, 0, 0);
          }
          rb.velocity = transform.right * speed * -1;
          Destroy(gameObject, 5);
        }
      }
    }

    IEnumerator OrbChange()
    {
      yield return new WaitForSeconds(0.12f);
      orbscale += 0.1f;
      orbscale = Mathf.Clamp(orbscale, 0.5f, 3.0f);
      transform.localScale = new Vector3 (orbscale, orbscale, orbscale);
      if (increasing)
      {
        StartCoroutine(OrbChange());
      }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
      int dmg = (int) (basedmg * orbscale);
      if (hitInfo.tag == "Enemy")
      {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
          enemy.TakeDamage(dmg);
        }
      }
      else if (hitInfo.tag == "Boss")
      {
        Boss boss = hitInfo.GetComponent<Boss>();
        if (boss != null)
        {
          boss.TakeDamage(dmg);
          Destroy(gameObject);
        }
      }
      if (hitInfo.tag == "World")
      {
        Destroy(gameObject);
      }
    }
}
