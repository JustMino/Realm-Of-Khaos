using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleReset : MonoBehaviour
{
  public GameObject player;
  Death death;
  public GameObject particle;
  public Sprite emptyheart;

  // Start is called before the first frame update
  void Start()
  {
    player = GameObject.Find("Player");
    death = player.GetComponent<Death>();
    particle = transform.GetChild(0).gameObject;
  }

  // Update is called once per frame
  void Update()
  {
    if (death.spawnPoint != transform.position)
    {
      ParticleSystem particleS = particle.GetComponent<ParticleSystem>();
      var ts = particleS.textureSheetAnimation;
      ts.SetSprite(0, emptyheart);
    }
  }
}
