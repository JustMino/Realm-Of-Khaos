using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossName : MonoBehaviour
{

  GameManager gm;
  public GameObject gameM;
  public bool ran = false;
    // Start is called before the first frame update
    void Start()
    {
      gameM = GameObject.Find("GameManager");
      gm = gameM.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
      if (gm.inbossroom && !ran)
      {
        ran = true;
        StartCoroutine(FadeInAndOut());
      }
      if (gm.inbossroom == false)
      {
        ran = false;
      }
    }

    IEnumerator FadeInAndOut()
    {
      StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<Text>()));
      yield return new WaitForSeconds(2);
      StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<Text>()));
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
