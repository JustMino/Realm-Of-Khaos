using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charattack : MonoBehaviour
{
  GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
      gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void checkchar()
    {
      if (gm.selectedchar == 1)
      {

      }
      else if (gm.selectedchar == 2)
      {
        
      }
    }
}
