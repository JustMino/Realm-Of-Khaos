using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
  public Slider slider;
  public Gradient gradient;
  public Image fill;
  public GameObject[] All;

  [SerializeField]
  private Text valueText;

  public bool ran = false;

  public bool dead = false;

  public GameObject GameManager;
  public GameManager gm;

  void Start()
  {
    GameManager = GameObject.Find("GameManager");
    gm = GameManager.GetComponent<GameManager>();
    foreach (GameObject GO in All)
    {
      GO.SetActive(false);
    }
  }

  void Update()
  {
    if (gm.inbossroom)
    {
      foreach (GameObject GO in All)
      {
        GO.SetActive(true);
      }
    }
    else
    {
      foreach (GameObject GO in All)
      {
        GO.SetActive(false);
      }
    }
    if (dead)
    {
      Destroy(gameObject, 0.1f);
    }
  }

  public void SetMaxHealth(int health)
  {
    slider.maxValue = health;

    fill.color = gradient.Evaluate(1f);
    string[] tmp = valueText.text.Split(':');
    valueText.text = tmp[0] + " : " + health;
  }

  public void SetHealth(int health)
  {
    if (health > 0)
    {
      string[] tmp = valueText.text.Split(':');
      if (!ran)
      {
        foreach (GameObject GO in All)
        {
          GO.SetActive(true);
        }
        StartCoroutine(MoveHealthUp(health));
      }
      else
      {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        valueText.text = tmp[0] + ":" + health;
      }
    }
  }

  IEnumerator MoveHealthUp(int health)
  {
    ran = true;
    for (int i = 0; i < slider.maxValue; i++)
    {
      float time = 2.5f/slider.maxValue;
      slider.value++;
      fill.color = gradient.Evaluate(slider.normalizedValue);
      string[] tmp = valueText.text.Split(':');
      valueText.text = tmp[0] + ":" + health;
      yield return new WaitForSeconds(time);
    }
  }
}
