using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public Transform firePoint;
    public Transform attackPoint;
    public GameObject bulletPrefab;
    public GameObject chargebulletprefab;
    public GameObject AbOnePrefab;
    // public GameObject subbulletPrefab;
    GameManager gm;
    Animator animator;
    LineRenderer line;
    public GameObject reticle;
    public GameObject cooldownimgGO;
    public Image cooldownimg;
    public float cooldown;
    public bool shooting = false;
    public bool cansubshoot = true;
    public bool subshooting = false;
    public bool reloading = false;
    public bool attacking = false;
    public bool AbOneReady = true;

    public float CBCooldown = 8.0f;
    public float AbOneCooldown = 15.0f;
    public float effectSpawnRate = 10;
    public int maxAmmo = 200;
    public int currentAmmo;
    public int bulrot = 180;
    public LayerMask enemyLayers;
    public LayerMask bossLayers;

    public AmmoText ammoText;
    public LaserCountdown laser;

    public AudioSource audioS;
    public AudioClip[] clips;

    public int tempAmmo;
    void Awake()
    {
      // audio = GetComponent<AudioSource>();
      reticle = GameObject.Find("Reticle");
      gm = GameObject.Find("GameManager").GetComponent<GameManager>();
      animator = GetComponent<Animator>();
      attackPoint = transform.Find("AttackPoint");
      bulletPrefab = Resources.Load<GameObject>("Bullet");
      chargebulletprefab = Resources.Load<GameObject>("ChargeBullet");
      AbOnePrefab = Resources.Load<GameObject>("EMP");
      ammoText = GameObject.Find("AmmoText").GetComponent<AmmoText>();
      InvokeRepeating("Shoot", 0.0f, cooldown);
      currentAmmo = maxAmmo;
    }
    void Update()
    {
      rotatebullet();
      shooting = false;
      subshooting = false;
      ammoText.SetAmmo(currentAmmo);
      if (Input.GetButton("Fire1"))
      {
        if (currentAmmo > 0)
        {
          shooting = true;
        }
      }
      if (Input.GetButtonDown("Fire2"))
      {
        Subshoot();
      }
      if (Input.GetButtonDown("Ability 1"))
      {
        if (AbOneReady)
        {
          // AbilityOne();
        }
      }
      if (Input.GetKey(gm.reloadkey) && !reloading)
      {
        if (currentAmmo != 200)
        {
          StartCoroutine(ReloadTime());
        }
      }
      if (currentAmmo <= 0 && !reloading)
      {
        StartCoroutine(ReloadTime());
      }
    }

    void Shoot()
    {
      if (shooting && !reloading && !attacking && !subshooting)
      {
        if (currentAmmo >= 0)
        {
          Instantiate(bulletPrefab, firePoint.position, new Quaternion (0, bulrot, 0, 0));
          currentAmmo--;
        }
      }
    }

    void Subshoot()
    {
      if (!shooting && !reloading && !attacking && !subshooting)
      {
        subshooting = true;
        Instantiate(chargebulletprefab, firePoint.position, new Quaternion (0, bulrot, 0, 0));
      }
    }

    void AbilityOne()
    {
      if (!shooting && !reloading && !attacking && !subshooting)
      {
        Instantiate(AbOnePrefab, transform.position, Quaternion.identity);
        StartCoroutine(AbilityOneCooldown());
      }
    }

    void rotatebullet()
    {
      if(!Input.GetKey(gm.strafekey))
      {
        if (Input.GetKey(gm.leftkey))
        {
          bulrot = 0;
        }
        else if (Input.GetKey(gm.rightkey))
        {
          bulrot = 180;
        }
      }
    }

    IEnumerator ChargeBulletCooldown()
    {
      yield return new WaitForSeconds(CBCooldown);
      subshooting = false;
    }

    IEnumerator AbilityOneCooldown()
    {
      AbOneReady = false;
      yield return new WaitForSeconds(AbOneCooldown);
      AbOneReady = true;
    }

    IEnumerator ReloadTime()
    {
      reloading = true;
      if (currentAmmo <= 0)
      {
        tempAmmo = 200;
      }
      else
      {
        tempAmmo = maxAmmo - currentAmmo;
      }
      for (int i = 0; i < tempAmmo; i++)
      {
        yield return new WaitForSeconds(1.3f/tempAmmo);
        currentAmmo++;
      }
      reloading = false;
    }
}
