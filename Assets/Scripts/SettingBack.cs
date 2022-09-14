using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingBack : MonoBehaviour
{
  public Gradient gradient;
  public Image back;
  public float count = 0;

    // Update is called once per frame

    void Start()
    {
      StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {

      while (true)
      {
        back.color = gradient.Evaluate(count);
        count += 0.01f;
        yield return new WaitForSeconds(0.02f);
        if (count >= 1)
        {
          count = 0;
        }
      }
    }
}
