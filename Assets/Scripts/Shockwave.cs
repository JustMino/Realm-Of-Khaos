using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
  public Transform[] hbp;
  public Vector2[] hitbox;
  public int shockstage = 0;
  public LayerMask playerLayers;

  public GameObject gameM;
  public GameManager gm;

  public ForceMode2D fMode = ForceMode2D.Impulse;

  public bool doingshock = false;
  public bool shockhit = false;
  public int shockchange = 0;

  Rigidbody2D m_Rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
      m_Rigidbody2D = GetComponent<Rigidbody2D>();
      gameM = GameObject.Find("GameManager");
      gm = gameM.GetComponent<GameManager>();
      if (gm.shockwavecount == 1)
      {
        transform.localScale = new Vector3 (-1f, 1f, 1f);
      }
      else if (gm.shockwavecount == 0)
      {
        transform.localScale = new Vector3 (1f, 1f, 1f);
      }
      for (int i = 0; i < 9; i++)
      {
        hbp[i] = transform.Find("SW Hit " + (i+1).ToString()).transform;
      }
      hitbox[0] = new Vector2 (0.85f, 1.75f);
      hitbox[1] = new Vector2 (1.3f, 2.95f);
      hitbox[2] = new Vector2 (2.5f, 4.05f);
      hitbox[3] = new Vector2 (3.95f, 5.2f);
      hitbox[4] = new Vector2 (4.5f, 5.15f);
      hitbox[5] = new Vector2 (4.35f, 4.7f);
      hitbox[6] = new Vector2 (4.35f, 4.7f);
      hitbox[7] = new Vector2 (4.35f, 4.7f);
      hitbox[8] = new Vector2 (4.35f, 4.7f);
      gm.shockwavecount++;
      if (gm.shockwavecount == 2)
      {
        gm.shockwavecount = 0;
      }
    }

    // Update is called once per frame
    void Update()
    {
      ShockwaveHit();
      if (transform.localScale.x == 1)
      {
        m_Rigidbody2D.AddForce(transform.right * -0.1f, fMode);
      }
      else if (transform.localScale.x == -1)
      {
        m_Rigidbody2D.AddForce(transform.right * 0.1f, fMode);
      }
    }

    void ShockwaveHit()
    {
      if (shockchange == 0)
      {
        shockchange++;
        shockstage = 0;
      }
      StartCoroutine(Shockhit());
    }

    IEnumerator Shockhit()
    {
      Vector2 mainpoint = new Vector2 (hbp[shockstage].position.x, hbp[shockstage].position.y);
      Collider2D hit = Physics2D.OverlapBox(mainpoint, hitbox[shockstage], 0f, playerLayers);
      if (hit != null && !shockhit)
      {
        shockhit = true;
        Rigidbody2D rigidbod = hit.GetComponent<Rigidbody2D>();
        PlayerStats player = hit.GetComponent<PlayerStats>();
        player.TakeDamage(gm.shockwavedmg);
        Vector2 dir = (transform.position - hit.transform.position).normalized;
        if (transform.localScale.x == 1)
        {
          rigidbod.AddForce(hit.transform.up * 50f, fMode);
          rigidbod.AddForce(hit.transform.right * -50f, fMode);
        }
        else if (transform.localScale.x == -1)
        {
          rigidbod.AddForce(hit.transform.up * 50f, fMode);
          rigidbod.AddForce(hit.transform.right * 50f, fMode);
        }
      }
      else
      {
        yield return new WaitForSeconds(0.0001f);
      }
    }

    void OnDrawGizmosSelected()
    {
      Vector2 hitboxpoint = new Vector2 (hbp[shockstage].position.x, hbp[shockstage].position.y);
      Gizmos.DrawWireCube(hitboxpoint, hitbox[shockstage]);
    }

    void nextshockstage()
    {
      shockstage++;
    }

    void looppoint()
    {
      shockstage = 5;
    }

    void Destroy()
    {
      Destroy(this.gameObject);
    }
}
