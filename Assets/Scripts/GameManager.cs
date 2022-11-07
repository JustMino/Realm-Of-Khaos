using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;
  void Awake()
  {
    if (Instance == null)
    {
      DontDestroyOnLoad(gameObject);
      Instance = this;
    }
    else if (Instance != this)
    {
      Destroy(gameObject);
    }
  }

  void Start()
  {
    Time.timeScale = 1.0f;
  }

  [Header("Settings")]
  public bool fullscreen = true;
  public float mastervolume;
  public float gameeffectsvolume;
  public float musicvolume;
  public int resolutionwidth = 1920;
  public int resolutionheight = 1080;
  public float brightness = 0;
  public int quality = 3;

  [Header("Keybinds")]
  public KeyCode primfire = KeyCode.Mouse0;
  public KeyCode subfire = KeyCode.Mouse1;
  public KeyCode meleekey = KeyCode.E;
  public KeyCode reloadkey = KeyCode.R;
  public KeyCode jumpkey = KeyCode.Space;
  public KeyCode leftkey = KeyCode.A;
  public KeyCode rightkey = KeyCode.D;
  public KeyCode downkey = KeyCode.S;
  public KeyCode pausekey = KeyCode.Escape;
  public KeyCode strafekey = KeyCode.LeftShift;

  [Header("Player Controller Values")]
  public float moveSpeed = 2.0f;
  public float groundjumpForce = 50.0f;
  public float MovementSmoothing = 0.05f;

  [Space(5)]

  [Header("Player Stats Values")]
  public int maxhealth = 100;

  [Space(5)]

  [Header("Player Attack Values")]
  [Header("Basic Slash Attack")]
  public float attackRange = 0.5f;

  [Space(5)]

  [Header("Shooting")]
  public float cooldown = 0.5f;
  public int maxAmmo = 95;
  [Header("Bullet")]
  public float speed = 200f;
  public int damage = 20;

  [Space(5)]

  [Header("Death/Health")]
  [Range(1, 99)]
  public int lives = 3;
  public int deadpos = -200;
  public int waterDamage = 5;
  public int enemyDamage = 10;

  [Header("Enemies")]
  [Header("Cencharge")]
  public int cenchargemaxhealth = 250;
  public int cenchargedamage = 10;

  [Header("Flying Enemy")]
  public int flyingdamage = 5;
  public int flyingmaxhealth = 100;

  [Header("Cat")]
  public float catspeed = 1.5f;
  public int catmaxhealth = 200;
  public int catdamage = 5;
  public float catattackDistance = 3.5f;
  public float catattackRange = 1f;
  public float catattackcooldown = 1f;

  [Header("Slasher")]
  public float slasherspeed = 2.5f;
  public int slashermaxhealth = 100;
  public int slasherdamage = 10;
  public float slasherattackdistance = 3.5f;
  public float slasherattackrange = 1f;
  public float slasherattackcooldown = 1f;

  [Header("Boss")]
  public int bossmaxhealth = 7500;
  public int flamedmg = 1;
  public float flamecooldown = 0.025f;
  public int rocketdmg = 10;
  public int shockwavedmg = 10;
  public float bossspeed = 1.5f;

  [Header("1st Level Gen Parameters")]

  [Space(1)]

  [Header("Walking Ground Gen")]
  public int mingroundlength = 10;
  public int maxgroundlength = 21;
  public int minheightchange = 10;
  public int maxheightchange = 16;

  [Space(1)]

  [Header("Underground to surface Gen")]
  public int fillstartypos = -5;
  public int fillstartxpos = -10;

  [Space(1)]

  [Header("Wall")]
  public int wallheight = 20;
  public int extrawalls = 10;


  [Space(5)]

  [Header("Backend")]
  public int shockwavecount = 0;
  public bool inbossroom = false;
  public int selectedchar = 0;
}
