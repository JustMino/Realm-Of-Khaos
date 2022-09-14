using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
  public Vector3 target;
  public GameObject head;
  public GameObject player;
  public float RotationSpeed;
  public float offset;
  SpriteRenderer spriteRenderer;
  public float scalemul;
  PlayerController playerController;
  GameManager gm;

  // Start is called before the first frame update
  void Awake()
  {
    // spriteRenderer = GetComponent<SpriteRenderer>();
    // playerController = player.GetComponent<PlayerController>();
    gm = GameObject.Find("GameManager").GetComponent<GameManager>();
  }

  // Update is called once per frame
  void Update()
  {
    // target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    // RotateGameObject(target, RotationSpeed, offset);
    rotatehead();
  }

  void rotatehead()
  {
    if (!Input.GetKey(gm.strafekey))
    {
      if(Input.GetKey(gm.leftkey))
      {
        transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
      }
      else if (Input.GetKey(gm.rightkey))
      {
        transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
      }
    }
  }

  void RotateGameObject(Vector3 target, float RotationSpeed, float offset)
  {
    Vector3 dir = target - transform.position;

    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    if (!playerController.flipped)
    {
      if (target.x >= player.transform.position.x)
      {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        head.transform.localScale = new Vector3(1.0f * scalemul, 1.0f * scalemul, 1.0f * scalemul);
      }
      else
      {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        head.transform.localScale = new Vector3(1.0f * scalemul, -1.0f * scalemul, 1.0f * scalemul);
      }
    }
    else
    {
      if (target.x >= player.transform.position.x)
      {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        head.transform.localScale = new Vector3(-1.0f * scalemul, 1.0f * scalemul, 1.0f * scalemul);
      }
      else
      {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        head.transform.localScale = new Vector3(-1.0f * scalemul, -1.0f * scalemul, 1.0f * scalemul);
      }
    }
  }
}
