using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  [SerializeField]
  private GameObject[] enemy;

  public bool spawnable = false;
  public bool notspawnable = false;
  public bool collisioncheck = false;

  [SerializeField] LayerMask layerscheck;

  public int enemytype;

    void Start()
    {
      StartCoroutine(SpawnCountdown());
    }

    void Update()
    {
      UpdateSpawnable();
    }

    IEnumerator SpawnCountdown()
    {
      if (spawnable && !notspawnable)
      {
        enemytype = Random.Range(1, 20);
        if (enemytype > 0 && enemytype <= 8)
        {
          Instantiate(enemy[0], transform.position, new Quaternion (0f, 0f, 0f, 0f));
        }
        if (enemytype > 8 && enemytype <= 15)
        {
          Instantiate(enemy[1], transform.position, new Quaternion (0f, 0f, 0f, 0f));
        }
        else
        {
          Instantiate(enemy[2], transform.position, new Quaternion (0f, 0f, 0f, 0f));
        }
        yield return new WaitForSeconds(8);
        StartCoroutine(SpawnCountdown());
      }
      else
      {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SpawnCountdown());
      }
    }

    void UpdateSpawnable()
    {
      Collider2D[] col = Physics2D.OverlapBoxAll(transform.position, new Vector2 (1.0f, 1.0f), 0.0f, layerscheck);
      notspawnable = (col != null) ? true : false;
      if (notspawnable) Debug.Log("Found something");
      else Debug.Log("Found nothing");
    }
    // void OnTriggerStay2D(Collider2D other)
    // {
    //   // if (other.gameObject.tag == "SpawnZone") spawnable = true;
    //   if (other.gameObject.tag == "World") notspawnable = true;
    // }
    //
    // void OnTriggerExit2D(Collider2D other)
    // {
    //   if (other.gameObject.tag == "SpawnZone") spawnable = false;
    //   if (other.gameObject.tag == "World") notspawnable = false;
    // }
}
