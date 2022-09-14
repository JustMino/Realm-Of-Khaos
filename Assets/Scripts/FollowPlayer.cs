using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
  public GameObject player;
  Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
      rb.MovePosition(new Vector2 (player.transform.position.x, player.transform.position.y));
    }
}
