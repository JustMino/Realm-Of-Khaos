using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : MonoBehaviour
{
  public float maxscale = 15.0f;
  public float traveltime = 1.0f;
  Vector3 curscale;
  [SerializeField] LayerMask enemymask;

  void Start()
  {
    transform.localScale = new Vector3 (0f, 0f, 0f);
    StartCoroutine(MoveEMP());
  }

  IEnumerator MoveEMP()
  {
    curscale = transform.localScale;
    yield return new WaitForSeconds(traveltime/(maxscale*10f));
    curscale += new Vector3(0.1f, 0.1f, 0.1f);
    curscale = new Vector3 (curscale.x, Mathf.Clamp(curscale.y, 0f, 1.0f), curscale.z);
    transform.localScale = curscale;
    Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, curscale, 0f, enemymask);
    // Debug.Log((hit == null) ? "No Enemies" : "Enemies hit");
    foreach(var hitCollider in hit)
    {
      Enemy enemy = hitCollider.GetComponent<Enemy>();
      StartCoroutine(enemy.GotShocked());
    }
    if (curscale.x < maxscale)
    {
      StartCoroutine(MoveEMP());
    }
    else
    {
      Destroy(gameObject);
    }
  }
}
