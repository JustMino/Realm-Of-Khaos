using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGO : MonoBehaviour
{
  public LayerMask playerLayers;
  public float[] explosionsize = {2.75f, 3f, 4f, 5f, 6.5f};
  public int explosionstate = 0;

  Animator animator;

  public bool hitplayer = false;
  void Start()
  {
    animator = GetComponent<Animator>();
    animator.SetInteger("ExplosionCheck", 1);
    explosionstate = 0;
  }

  void Update()
  {
    if (!hitplayer)
    {
      Collider2D hit = Physics2D.OverlapCircle(transform.position, explosionsize[explosionstate], playerLayers);
      if (hit == null)
      {
        return;
      }
      else
      {
        hitplayer = true;
        PlayerStats player = hit.GetComponent<PlayerStats>();
        player.TakeDamage(10);
      }
    }
  }

  void Destroy()
  {
    Destroy(this.gameObject);
  }

  void NextStage()
  {
    explosionstate++;
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.DrawSphere(transform.position, explosionsize[explosionstate]);
  }
}
