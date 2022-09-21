using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : MonoBehaviour
{
  public float maxscale = 3.0f;
  public float traveltime = 2.0f;
  Vector3 curscale;

  void Start()
  {
    transform.localScale = (0f, 0f, 0f);
    StartCoroutine(MoveEMP());
  }

  IEnumerator MoveEMP()
  {
    curscale = transform.localScale;
    yield return new WaitForSeconds(traveltime/(maxscale*10f));
    curscale += new Vector3(0.1f, 0.1f, 0.1f);
    curscale = new Vector3 (curscale.x, Mathf.Clamp(curscale.y, 0f, 1.0f), curscale.z);
    transform.localScale = curscale;
    if (curscale.x < maxscale)
    {
      StartCoroutine(MoveEMP());
    }
  }
}
