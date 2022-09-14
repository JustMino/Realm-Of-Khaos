using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserCountdown : MonoBehaviour
{
    [SerializeField]
    private Text LaserText;

    public void SetCooldown(float currentCooldown)
    {
      currentCooldown *= 10;
      currentCooldown = Mathf.Round(currentCooldown);
      currentCooldown /= 10;
      if (currentCooldown != 0)
      {
        LaserText.fontSize = 80;
        LaserText.text = currentCooldown.ToString();
      }
      else
      {
        LaserText.text = "";
      }
    }
}
