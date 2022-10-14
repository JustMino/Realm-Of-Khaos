using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
  Animator animator;
  public PlayerStats player;
  public GameManager gm;
  public GameObject gameM;
  public PlayerAttack playerA;
  public bool touchingWater = false;
  public bool touchingEnemy = false;
  public bool tookWaterDamage = false;
  public bool tookEnemyDamage = false;
  public bool dead = false;
  public GameObject[] hearts;
  public GameObject heart;
  public GameObject canvas;
  public Vector3 spawnPoint;
  public float HeartX = -770f;
  public float heartDist = 100f;
  public float canvasX;
  public float canvasY;
  [SerializeField]
  private bool nodie = false;
  public GameObject deathanim;
  public SpriteRenderer[] All;
  public Sprite filledheart;
  public int templives;
  Rigidbody2D m_Rigidbody2D;


  void Start()
  {
    All = GetComponentsInChildren<SpriteRenderer>();
    gameM = GameObject.Find("GameManager");
    gm = gameM.GetComponent<GameManager>();
    spawnPoint = GameObject.Find("Spawn Point").transform.position;
    animator = GetComponent<Animator>();
    player = GetComponent<PlayerStats>();
    playerA = GetComponent<PlayerAttack>();
    m_Rigidbody2D = GetComponent<Rigidbody2D>();
    deathanim = GameObject.Find("Death Anim");
    canvas = GameObject.Find("Canvas");
    heart = Resources.Load<GameObject>("Heart");
    templives = gm.lives;
    for (int i = 0; i < gm.lives; i++)
    {
      hearts[i] = Instantiate(heart);
      hearts[i].transform.SetParent(canvas.transform, true);
      hearts[i].transform.position = new Vector3(Screen.width + heartDist * i, Screen.height * 0.95f, 0);
    }
  }

  void Update()
  {
    if (!nodie)
    {
      if (player.currentHealth <= 0 || transform.position.y <= gm.deadpos)
      {
        if (!dead)
        {
          dead = true;
          animator.SetBool("Dead", true);
          if (gm.lives > 1)
          {
            Animator tempanim = hearts[gm.lives - 1].GetComponent<Animator>();
            animator.SetTrigger("Death");
            tempanim.SetTrigger("Heart Break");
            gm.lives--;
          }
          else
          {
            animator.SetTrigger("FinalDeath");
            dead = true;
          }
        }
      }
      if (transform.position.y <= gm.deadpos)
      {
        player.TakeDamage(100);
      }
    }
  }

  void FixedUpdate()
  {
    // StartCoroutine(TouchPain());
  }
  public void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag == "Pain")
    {
      touchingWater = true;
    }
    else if (other.gameObject.tag == "Checkpoint")
    {
      spawnPoint = other.transform.position;
      GameObject otherC = other.gameObject.transform.GetChild(0).gameObject;
      ParticleSystem otherP = otherC.GetComponent<ParticleSystem>();
      var ts = otherP.textureSheetAnimation;
      ts.SetSprite(0, filledheart);
    }
    else if (other.gameObject.tag == "NO DIE")
    {
      nodie = true;
    }
    else if (other.gameObject.tag == "deadpos")
    {
      player.TakeDamage(100);
    }
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    if (other.gameObject.tag == "Pain")
    {
      touchingWater = false;
    }
    else if (other.gameObject.tag == "NO DIE")
    {
      nodie = false;
    }
  }

  public void ReloadScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    gm.lives = templives;
  }

  IEnumerator TouchPain()
  {
    if (touchingWater && !tookWaterDamage)
    {
      tookWaterDamage = true;
      player.TakeDamage(gm.waterDamage);
      yield return new WaitForSeconds(0.5f);
      tookWaterDamage = false;
    }
  }
  public void Deadanim()
  {
    SpriteRenderer deathanimS = deathanim.GetComponent<SpriteRenderer>();
    foreach (SpriteRenderer SR in All)
    {
      SR.enabled = !SR.enabled;
    }
  }
  public void Respawn()
  {
    dead = false;
    animator.SetBool("Dead", false);
    m_Rigidbody2D.velocity = new Vector2 (0f, 0f);
    player.currentHealth = gm.maxhealth;
    transform.position = spawnPoint;
    touchingWater = false;
    tookWaterDamage = false;
    tookEnemyDamage = false;
  }
}
