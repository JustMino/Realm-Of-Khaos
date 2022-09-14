using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidAirExplosion : MonoBehaviour
{
  public LayerMask playerLayers;
  public float[] explosionsize = {0.5f, 1f, 2f, 2.5f, 3.5f, 4f, 4.5f};
  public int explosionstate = 0;

  Animator animator;
  public GameManager gm;
  public GameObject gameM;

  public bool hitplayer = false;
  void Start()
  {
    gameM = GameObject.Find("GameManager");
    gm = gameM.GetComponent<GameManager>();
    animator = GetComponent<Animator>();
    animator.SetInteger("ExplosionCheck", 2);
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
        player.TakeDamage(gm.rocketdmg);
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
