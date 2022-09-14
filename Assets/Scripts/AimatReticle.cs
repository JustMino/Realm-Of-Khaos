using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimatReticle : MonoBehaviour
{
  public Vector3 target;
  public GameObject main;
  public GameObject player;
  public float RotationSpeed;
  public float offset;
  private Vector3 mainpos;
  public float scalemul;
  SpriteRenderer spriteRenderer;
  public float x;
  public bool flipped;
  PlayerController playerController;

  public Animator animator;

  void Awake()
  {
    playerController = player.GetComponent<PlayerController>();
    spriteRenderer = GetComponent<SpriteRenderer>();
  }
    // Update is called once per frame
  void Update()
  {
    target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    RotateMainObject(target, RotationSpeed, offset);
  }

  void RotateMainObject(Vector3 target, float RotationSpeed, float offset)
  {
    Vector3 dir = target - transform.position;

    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    if (!playerController.flipped)
    {
      if (target.x >= player.transform.position.x)
      {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        main.transform.localScale = new Vector3(1.0f * scalemul, 1.0f * scalemul, 1.0f * scalemul);
      }
      else
      {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        main.transform.localScale = new Vector3(1.0f * scalemul, -1.0f * scalemul, 1.0f * scalemul);
      }
    }
    else
    {
      if (target.x >= player.transform.position.x)
      {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        main.transform.localScale = new Vector3(-1.0f * scalemul, 1.0f * scalemul, 1.0f * scalemul);
      }
      else
      {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        main.transform.localScale = new Vector3(-1.0f * scalemul, -1.0f * scalemul, 1.0f * scalemul);
      }
    }
  }
}
