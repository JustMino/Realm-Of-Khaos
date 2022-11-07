using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLayerChange : MonoBehaviour
{
  GameManager gm;
  PlayerController player;
  PlayerAttack atk;
  public bool facingR = true;
  public bool flippedcheck;
    // Start is called before the first frame update
    void Start()
    {
      gm = GameObject.Find("GameManager").GetComponent<GameManager>();
      player = GameObject.Find("Player 1").GetComponent<PlayerController>();
      atk = GameObject.Find("Player 1").GetComponent<PlayerAttack>();
    }

    void Update()
    {
      flippedcheck = player.flipped;
      rotatearm();
    }

    void rotatearm()
    {
      if (Input.GetKey(gm.strafekey))
      {
        if (Input.GetKey(gm.leftkey))
        {
          if (player.flipped && facingR)
          {
            transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
          }
          else if (player.flipped && !facingR)
          {
            transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
          }
        }
        if (Input.GetKey(gm.rightkey))
        {
          if (!player.flipped && facingR)
          {
            transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
          }
          else if (!player.flipped && !facingR)
          {
            transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
          }
        }
      }
      else
      {
        transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
        if (player.flipped)
        {
          facingR = false;
        }
        else
        {
          facingR = true;
        }
      }
    }
}
