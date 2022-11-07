using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStepUp : MonoBehaviour
{
    int enemyID;
    // Start is called before the first frame update
    void Start()
    {
        enemyID = GetComponent<Enemy>().enemyID;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      Vector3 startpos = transform.position;
      startpos.y -= 1.5f;
      Vector3 temp = transform.position;
      temp.x -= 1.5f;
      temp.y -= 1.5f;
      Gizmos.DrawLine(startpos, temp);
    }
}
