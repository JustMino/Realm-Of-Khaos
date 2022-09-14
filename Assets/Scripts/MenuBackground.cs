using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBackground : MonoBehaviour
{
  Image img;
  public Sprite[] back;
  public int val;
    // Start is called before the first frame update
    void Start()
    {
      img = GetComponent<Image>();
      val = Random.Range(0, 2);
      img.sprite = back[val];
    }
}
