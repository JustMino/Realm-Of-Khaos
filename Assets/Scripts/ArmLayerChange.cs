using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLayerChange : MonoBehaviour
{
  GameManager gm;
  PlayerController player;
  public bool facingR = true;
    // Start is called before the first frame update
    void Start()
    {
      gm = GameObject.Find("GameManager").GetComponent<GameManager>();
      player = GameObject.Find("Player 1").GetComponent<PlayerController>();
    }

    void Update()
    {
      rotatearm();
    }

    void rotatearm()
    {
      if (!Input.GetKey(gm.strafekey))
      {
        if (player.flipped)
        {
          facingR = false;
        }
        else
        {
          facingR = true;
        }
      }
      if (Input.GetKey(gm.strafekey))
      {
        if (player.flipped && !facingR)
        {
          if (Input.GetKey(gm.leftkey))
          {
            transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
          }
          else if (Input.GetKey(gm.rightkey))
          {
            transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
          }
        }
        if (player.flipped && facingR)
        {
          if (Input.GetKey(gm.leftkey))
          {
            transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
          }
          else if (Input.GetKey(gm.rightkey))
          {
            transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
          }
        }
      }
      else if (!Input.GetKey(gm.strafekey))
      {
        transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
      }
    }
}
