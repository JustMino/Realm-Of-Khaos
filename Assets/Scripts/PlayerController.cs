using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
  [Header("Needed Components")]
  GameManager GM;
  PlayerAttack playerAttack;
  Animator animator;
  [SerializeField] AudioSource audioS;
  [SerializeField] AudioClip[] clips;
  Rigidbody2D rb;

  [Header("Player Parameters")]
  float horizontalInput;
  public bool flipped = false;
  bool isGrounded = true;
  [SerializeField] LayerMask WhatIsGround;
  float MovementSmoothing = 0.05f;
  bool footsteps;
  [SerializeField] float raycastdistance = 5f;

  void Awake()
  {
    GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    playerAttack = GetComponent<PlayerAttack>();
    animator = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    horizontalInput = Input.GetAxisRaw("Horizontal");
    isGrounded = Physics2D.Raycast(transform.position, Vector3.down, raycastdistance, WhatIsGround);
    Debug.DrawRay(transform.position, Vector3.down*raycastdistance, Color.green);
    flipped = (horizontalInput >= 0) ? true : false;
    transform.localScale = (horizontalInput >= 0) ? new Vector3 (1.0f, 1.0f, 1.0f) : new Vector3 (-1.0f, 1.0f, 1.0f);
    animator.SetBool("isRunning", (horizontalInput != 0) ? true : false);
    animator.SetBool("isGrounded", isGrounded);
    if (isGrounded) animator.SetBool("isJumping", false);
    if (Input.GetButtonDown("Jump")) Jump();
  }

  void FixedUpdate()
  {
    Move(horizontalInput * GM.moveSpeed);
  }

  void Move(float move)
  {
    footsteps = true;
    Vector3 velocity = Vector3.zero;
    Vector3 targetVelocity = new Vector2 (move * 10f, rb.velocity.y);
    rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, MovementSmoothing);
  }

  void Jump()
  {
    if (isGrounded)
    {
      Vector3 temp = rb.velocity;
      temp.y = 0;
      rb.velocity = temp;
      rb.AddForce(transform.up * GM.groundjumpForce, ForceMode2D.Impulse);
      animator.SetTrigger("Jump");
    }
  }

  IEnumerator Footsteps()
  {
    if (!footsteps)
    {
      int randfootstep = Random.Range(0,4);
      audioS.clip = clips[randfootstep];
      audioS.Play();
      yield return new WaitForSeconds(audioS.clip.length);
      footsteps = false;
    }
  }
}
