using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoText : MonoBehaviour
{
  [SerializeField]
  private Text ammoText;

  public void SetAmmo(int currentAmmo)
  {
    ammoText.text = currentAmmo.ToString();
  }
}
