using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAiming : MonoBehaviour
{
  public GameObject target;
  public Vector3 targetv;
  public Transform parent;
  public float RotationSpeed = 2f;
  public float offset;

  void Update()
  {
    target = GameObject.FindWithTag("Player");
    parent = transform.parent;
    targetv = target.transform.position;
    RotateFirepoint(targetv, RotationSpeed, offset);
  }

  void RotateFirepoint(Vector3 targetv, float RotationSpeed, float offset)
  {
    Vector3 dir = targetv - parent.transform.position;

    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

    Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
  }
}
