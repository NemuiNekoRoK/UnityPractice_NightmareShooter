using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootManager : MonoBehaviour{
  public int damagePerShot = 20;
  private float timeBetweenBullets = 0.7f;
  private float effectsDisplayTime = 0.2f;
  private float range = 100f;

  public int ammo = 12;
  private const int ammoMax = 12;
  private float reloadDelay = -1.5f;
  private ParticleSystem gunParticles;
  private LineRenderer gunLine;
  private AudioSource gunAudio;
  private Light gunLight;
  private PlayerManager pm;

  private float timer;
  private Ray shootRay;
  private RaycastHit shootHit;
  private int shootableMask;
  private UIManager ui;

  void Awake(){
    shootableMask = LayerMask.GetMask("Shootable");
    gunParticles = GetComponent<ParticleSystem>();
    gunLine = GetComponent<LineRenderer>();
    gunAudio = GetComponent<AudioSource>();
    gunLight = GetComponent<Light>();
    pm = GameObject.Find("Player").GetComponent<PlayerManager>();
    ui = GameObject.Find("Canvas").GetComponent<UIManager>();
  }

  void Update(){
    timer += Time.deltaTime;
    if(timer >= timeBetweenBullets * effectsDisplayTime){
      DisableEffects();
    }
  }

  public void DisableEffects(){
    gunLight.enabled = false;
    gunLine.enabled = false;
  }

  public void Reload(){
    if(!pm.isDead){
      timer = reloadDelay;
      ammo = ammoMax;
      ui.ShootUI(ammo);
    }
  }

  public void Shot(){
    if(timer <= timeBetweenBullets || ammo <= 0 || pm.isDead){
      return;
    }

    timer = 0f;
    ammo -= 1;
    ui.ShootUI(ammo);

    gunAudio.Play();
    gunLight.enabled = true;
    gunParticles.Stop();
    gunParticles.Play();
    gunLine.enabled = true;
    gunLine.SetPosition (0, transform.position);

    shootRay.origin = transform.position;
    shootRay.direction = transform.forward;

    if(Physics.Raycast(shootRay, out shootHit, range, shootableMask)){
      Enemy enemyHealth = shootHit.collider.GetComponent <Enemy>();
      if(enemyHealth != null){
        enemyHealth.TakeDamage(damagePerShot, shootHit.point);
      }
      gunLine.SetPosition(1, shootHit.point);
    }else{
      gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
    }
  }



}
