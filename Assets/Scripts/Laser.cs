using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
  LineRenderer line;

  public GameObject firePoint;
  public GameObject reticle;

  void Start()
  {
    firePoint = GameObject.Find("FirePoint");
    reticle = GameObject.Find("Reticle");
    line = GetComponent<LineRenderer>();
  }

  void Update()
  {
    List<Vector3> pos = new List<Vector3>();
    pos.Add(firePoint.transform.position);
    pos.Add(reticle.transform.position);
    line.startWidth = 0.25f;
    line.endWidth = 0.25f;
    line.SetPositions(pos.ToArray());
    line.useWorldSpace = true;
  }
}
