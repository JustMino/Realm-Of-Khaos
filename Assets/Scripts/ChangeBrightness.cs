using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBrightness : MonoBehaviour
{
  GameManager gm;
  public GameObject gameM;
  public Image panel;
    // Start is called before the first frame update
    void Start()
    {
      gameM = GameObject.Find("GameManager");
      gm = gameM.GetComponent<GameManager>();
      panel = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
      changebrightness(gm.brightness);
    }

    public void changebrightness(float brightness)
    {
      float alphaval = Mathf.Abs(brightness);
      if (brightness >= 0)
      {
        alphaval /= 2;
        panel.color = new Color (255, 255, 255, alphaval);
      }
      else
      {
        panel.color = new Color (0, 0, 0, alphaval);
      }
    }
}
