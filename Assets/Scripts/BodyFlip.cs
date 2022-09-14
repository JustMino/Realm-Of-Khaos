using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFlip : MonoBehaviour
{
  public Vector3 target;
  public GameObject body;
  public GameObject player;
  SpriteRenderer spriteRenderer;

  void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }


  // Update is called once per frame
  void Update()
  {
    target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    RotateBody(target);
  }

  void RotateBody(Vector3 target)
  {
    if (target.x >= player.transform.position.x)
    {
      body.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
    }
    else
    {
      body.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
  }
}
