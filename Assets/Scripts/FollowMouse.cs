using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
  public Vector3 target;
  public float speed = 99999.0f;
    void Awake()
    {
      target = transform.position;
      Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
      target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      target.z = transform.position.z;
      transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
