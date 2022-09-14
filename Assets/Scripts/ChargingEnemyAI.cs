using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
public class ChargingEnemyAI : MonoBehaviour
{
  public enum State
  {
    Normal,
    Moving,
    Charging,
    Backup,
    Cooldown
  }

  public float speedInUnitPerSecond;
  public float chargeSpeed = 50f;
  public float chargeCooldown = 5.0f;
  public int chargedmg = 15;
  public float dist;
  public bool hitplayer = false;
  public bool isdead = false;
  public bool searchingForPlayer = false;
  public bool cancharge = true;
  public bool incooldown = false;
  public bool charging = false;
  public AudioClip[] clips;
  public State state;
  public LayerMask playerLayers;

  public Vector3 pos;

  Vector2 dir;
  Animator animator;
  AudioSource audioS;
  Enemy enemy;
  PlayerStats player;
  GameObject target;
  GameObject hitbox;
  Rigidbody2D m_Rigidbody2D;
  SpriteRenderer spriteRenderer;
  public ForceMode2D fMode;

  void Start()
  {
    animator = GetComponent<Animator>();
    enemy = GetComponent<Enemy>();
    enemy.enemyID = 1;
    hitbox = transform.Find("chargehitbox").gameObject;
    pos = hitbox.transform.localPosition;
    m_Rigidbody2D = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    state = State.Normal;
    if (target == null)
    {
      if (!searchingForPlayer)
      {
        searchingForPlayer = true;
        StartCoroutine(SearchForPlayer());
      }
      return;
    }
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
      target = sResult;
      player = target.GetComponent<PlayerStats>();
      searchingForPlayer = false;
      yield return false;
    }
  }

  void FixedUpdate()
  {
    if (!isdead)
    {
      if (charging)
      {
        Collider2D hit = Physics2D.OverlapBox(hitbox.transform.position, new Vector2 (2.0f, 4.0f), 0f, playerLayers);
        if (hit != null && !hitplayer)
        {
          PlayerStats playerh = hit.GetComponent<PlayerStats>();
          playerh.TakeDamage(chargedmg);
          hitplayer = true;
        }
        if (m_Rigidbody2D.velocity.x  == 0f)
        {
          charging = false;
        }
      }
      flip();
      speedInUnitPerSecond = m_Rigidbody2D.velocity.magnitude;
      animator.SetFloat("VelocityX", Mathf.Abs(speedInUnitPerSecond));
      if (target == null)
      {
        if (!searchingForPlayer)
        {
          searchingForPlayer = true;
          StartCoroutine(SearchForPlayer());
        }
      }
      switch (state)
      {
        case State.Normal:
          if (cancharge)
          {
            if (Mathf.Abs(dist) < 15.0f)
            {
              cancharge = false;
              animator.SetTrigger("precharge");
            }
            else
            {
              state = State.Moving;
            }
          }
          else if (!incooldown)
          {
            StartCoroutine(cooldown());
          }
          else
          {
            return;
          }
          break;
        case State.Moving:
          if (Mathf.Abs(dist) > 15.0f)
          {
            dir = (target.transform.position - transform.position).normalized;
            dir *= 10.0f;
            m_Rigidbody2D.AddForce(dir, ForceMode2D.Impulse);
            m_Rigidbody2D.velocity = Vector2.ClampMagnitude(m_Rigidbody2D.velocity, 15.0f);
          }
          else
          {
            state = State.Normal;
          }
          break;
        case State.Charging:
          cancharge = false;
          charging = true;
          animator.SetTrigger("Charging");
          dir = (target.transform.position - transform.position).normalized;
          dir *= chargeSpeed;
          m_Rigidbody2D.AddForce(dir, ForceMode2D.Impulse);
          state = State.Normal;

          break;
        case State.Backup:
          animator.SetTrigger("Backup");
          dir = (target.transform.position - transform.position).normalized;
          dir *= chargeSpeed * -1f;
          m_Rigidbody2D.AddForce(dir, fMode);
          state = State.Normal;
          break;
        case State.Cooldown:
          break;
      }
    }
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

  void flip()
  {
    dist = transform.position.x - target.transform.position.x;
    if (dist < 0)
    {
      spriteRenderer.flipX = true;
      hitbox.transform.localPosition = new Vector3 (pos.x * -1f, pos.y, pos.z);
    }
    else
    {
      spriteRenderer.flipX = false;
      hitbox.transform.localPosition = pos;
    }
  }

  void prechargeend()
  {
    state = State.Charging;
  }

  IEnumerator cooldown()
  {
    incooldown = true;
    yield return new WaitForSeconds(chargeCooldown/2.0f);
    state = State.Backup;
    yield return new WaitForSeconds(chargeCooldown/2.0f);
    cancharge = true;
    incooldown = false;
  }
}
