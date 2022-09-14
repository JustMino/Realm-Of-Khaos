using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class backHealthBar : MonoBehaviour
{
  public Slider slider;
  public Gradient gradient;
  public Image fill;

  public void SetMaxHealth(int health)
  {
    slider.maxValue = health;
    slider.value = health;
    fill.color = gradient.Evaluate(1f);
  }

  public void SetHealth(int health)
  {
    float diff = slider.value - health;
    StartCoroutine(DecreaseHealth(diff, health));
  }
  IEnumerator DecreaseHealth(float diff, int health)
  {
    for (int i = 0; i < diff; i++)
    {
      float time = 0.05f/diff;
      slider.value--;
      fill.color = gradient.Evaluate(slider.normalizedValue);
      yield return new WaitForSeconds(time);
    }
  }
}
