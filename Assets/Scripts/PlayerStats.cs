using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
  GameManager gm;
  public GameObject gameM;
  public int currentHealth;

  public HealthBar healthBar;
  public backHealthBar back;
  public SpriteRenderer[] All;
  public float colornum = 0.55f;
    // Start is called before the first frame update
    void Start()
    {
      gameM = GameObject.Find("GameManager");
      gm = gameM.GetComponent<GameManager>();
      All = GetComponentsInChildren<SpriteRenderer>();
      currentHealth = gm.maxhealth;
      healthBar.SetMaxHealth(gm.maxhealth);
      back.SetMaxHealth(gm.maxhealth);
      foreach (SpriteRenderer SR in All)
      {
        SR.color = new Color (colornum, colornum, colornum);
      }
    }

    // Update is called once per frame
    void Update()
    {
      if (currentHealth < 0)
      {
        currentHealth = 0;
      }
      healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
      if (currentHealth > 0)
      {
        foreach (SpriteRenderer SR in All)
        {
          SR.color = new Color(1, 0, 0);
        }
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        back.SetHealth(currentHealth);
        StartCoroutine(DmgColor());
      }
    }

    IEnumerator DmgColor()
    {
      yield return new WaitForSeconds(0.1f);
      foreach (SpriteRenderer SR in All)
      {
        SR.color = new Color (colornum, colornum, colornum);
      }
    }

}
