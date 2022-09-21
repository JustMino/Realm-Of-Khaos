using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthBar : MonoBehaviour
{
  public GameObject healthBar;
  public GameObject canvas;
  public GameObject instance;
  public Enemy enemy;
  public Slider slider;
  public float healthBarCorrection = 1f;
  public Vector3 enemyPosition;
  public Vector3 barPosition;
  public bool healthbar = false;
  public bool dead = false;
  public bool shocked = false;

  void Start()
  {
    canvas = GameObject.Find("Canvas");
    if (healthBar == null)
    {
      healthBar = Resources.Load<GameObject>("Enemyhealthbar");
    }
    enemy = GetComponent<Enemy>();
  }
  public void HealthBar()
  {
    float targethealth = enemy.enemyhealth;
    if (targethealth < enemy.maxenemyhealth)
    {
      if (!healthbar)
      {
        healthbar = true;
        instance = Instantiate(healthBar);
        slider = instance.GetComponent<Slider>();
      }
      instance.transform.SetParent(canvas.transform, false);
      enemyPosition = new Vector3(transform.position.x, transform.position.y + healthBarCorrection, 0);
      slider.minValue = 0;
      slider.value = enemy.enemyhealth;
      instance.transform.position = Camera.main.WorldToScreenPoint(enemyPosition);
      if (targethealth > 0)
      {
        slider.maxValue = enemy.maxenemyhealth;
        enemyPosition = new Vector3(transform.position.x, transform.position.y + healthBarCorrection, 0);
        instance.GetComponent<Transform>().position = Camera.main.WorldToScreenPoint(enemyPosition);
        slider.value = enemy.enemyhealth;
      }
      else
      {
        Destroy(instance, 0.1f);
        dead = true;
      }
    }
  }

  void Update()
  {
    if (!dead)
    {
      HealthBar();
    }
  }
}
