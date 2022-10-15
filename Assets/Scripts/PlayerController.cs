using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
  public bool jump = false;
  public float horizontalMove = 0f;
  public float MovementSmoothing = 0.05f;
  public bool AirControl = false;
  public LayerMask WhatIsGround;
  public float horizontalInput;

  const float GroundRadius = 0.4f;
  const float CeilingRadius = 0.2f;
  public Rigidbody2D m_Rigidbody2D;
  public Vector3 Velocity = Vector3.zero;
  GameManager gm;
  public GameObject gameM;
  PlayerAttack playerAttack;
  public float basemovespeed;

  internal Animator animator;

  public int colcount = 0;

  private UnityEvent OnLandEvent;

  public class BoolEvent : UnityEvent<bool> { }

  [SerializeField]
  private Vector2 normalStruck;

  public bool isGrounded = false;
  public bool flipped = false;
  public bool platform = false;
  public float tempjf;
  public float temppjf;

  public bool footsteps = false;
  public bool canmove = true;

  public GameObject platforms;

  public AudioSource audioS;
  public AudioClip[] clips;

  [SerializeField] LayerMask GroundLayers;
  void Awake()
  {
    // audio = GetComponent<AudioSource>();
    platforms = GameObject.Find("Platforms");
    m_Rigidbody2D = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    gameM = GameObject.Find("GameManager");
    gm = gameM.GetComponent<GameManager>();
    playerAttack = GetComponent<PlayerAttack>();
    basemovespeed = gm.moveSpeed;
    tempjf = gm.groundjumpForce;
    temppjf = gm.groundjumpForce * 0.5f;

    if (OnLandEvent == null)
    {
      OnLandEvent = new UnityEvent();
    }
  }

  void Update()
  {
    isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.9f, WhatIsGround);
    Debug.DrawRay(transform.position, Vector3.down*3.1075f, Color.green);
    if (Input.GetKey(gm.leftkey))
    {
      horizontalInput = -1f;
    }
    else if(Input.GetKey(gm.rightkey))
    {
      horizontalInput = 1f;
    }
    else
    {
      horizontalInput = 0f;
    }
    // float horizontalInput = Input.GetAxis("Horizontal");
    horizontalMove = horizontalInput * gm.moveSpeed;
    if (horizontalInput > 0)
    {
      // StartCoroutine(Footsteps());
      animator.SetBool("isRunning", true);
      transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
      flipped = false;
    }
    else if (horizontalInput < 0)
    {
      // StartCoroutine(Footsteps());
      animator.SetBool("isRunning", true);
      transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
      flipped = true;
    }
    else
    {
      // StartCoroutine(StopFootsteps());
      animator.SetBool("isRunning", false);
    }
    if (Input.GetKeyDown(gm.downkey) && platform)
    {
      StartCoroutine(Phasethroughplatform());
    }
    if (colcount > 0)
    {
      isGrounded = true;
      animator.SetBool("isGrounded", true);
      animator.SetBool("isJumping", false);
      gm.groundjumpForce = tempjf;
    }
    else
    {
      isGrounded = false;
      animator.SetBool("isGrounded", false);
    }
  }

  void FixedUpdate()
  {
    Move(horizontalMove);
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
      m_Rigidbody2D.AddForce(new Vector2 (0f, 0f));
      m_Rigidbody2D.AddForce(transform.up * gm.groundjumpForce, ForceMode2D.Impulse);
      animator.SetBool("isJumping", true);
    }
  }


  public void Move(float move)
  {
    if (canmove)
    {
      Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
      m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref Velocity, MovementSmoothing);
    }
  }

  public void Knockback(Vector3 knockbackDir, float knockbackAmount)
  {
    m_Rigidbody2D.AddForce(transform.position + knockbackDir * knockbackAmount);
  }

  IEnumerator Phasethroughplatform()
  {
    canmove = false;
    platforms.GetComponent<Collider2D>().enabled = false;
    yield return new WaitForSeconds(0.5f);
    platforms.GetComponent<Collider2D>().enabled = true;
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if(other.gameObject.CompareTag("World"))
    {
      colcount++;
    }
    else if (other.gameObject.CompareTag("Boss Room Trigger"))
    {
      gm.inbossroom = true;
    }
    // else if (other.gameObject.CompareTag("Boss Room"))
    // {
    //   gm.enteredbossroom = true;
    // }
  }

  // void OnTriggerExit2D(Collider2D other)
  // {
  //   if(other.gameObject.CompareTag("World") && !platform)
  //   {
  //     colcount--;
  //   }
  //   else if (other.gameObject.CompareTag("Boss Room Trigger"))
  //   {
  //     gm.inbossroom = false;
  //   }
  // }

  // void OnCollisionEnter2D(Collision2D other)
  // {
  //   if (other.gameObject.CompareTag("Platform"))
  //   {
  //     ContactPoint2D otherContact = other.contacts[0];
  //     normalStruck = otherContact.normal;
  //
  //     if (normalStruck.y == 1.0f)
  //     {
  //       isGrounded = true;
  //       platform = true;
  //       animator.SetBool("isGrounded", true);
  //       animator.SetBool("isJumping", false);
  //       gm.groundjumpForce = temppjf;
  //       normalStruck = new Vector2 (0,0);
  //     }
  //   }
  // }

  void OnCollisionExit2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Platform"))
    {
      if (!canmove)
      {
        canmove = true;

      }
      isGrounded = false;
      platform = false;
      animator.SetBool("isGrounded", false);
    }
  }


  IEnumerator Footsteps()
  {
    if (!footsteps)
    {
      int randfootstep = Random.Range(0,4);
      // footsteps = true;
      audioS.clip = clips[randfootstep];
      audioS.Play();
      yield return new WaitForSeconds(audioS.clip.length);
      footsteps = false;
    }
  }

  IEnumerator StopFootsteps()
  {
    if (footsteps)
    {
      audioS.Stop();
      footsteps = false;
    }
    else
    {
      yield return new WaitForSeconds(0.001f);
    }
  }
}
