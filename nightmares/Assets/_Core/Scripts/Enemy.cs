using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{
    private GameObject player;
    private PlayerManager pm;
    public int attackDamage = 10, currentHealth;
    public const int enemyHealth = 60;
    public const int goldValue = 10;
    public float sinkSpeed = 2.5f;
    private AudioSource enemyAudio;
    private Rigidbody rig;
    private CapsuleCollider capsuleCollider;
    private ParticleSystem hitParticles;
    public AudioClip deathClip;
    public AudioClip hurtClip;
    private bool isSinking = false;

    private UnityEngine.AI.NavMeshAgent nav;
    private Animator ani;
    public bool isDead = false;
    private bool playerInRange = false;
    private UIManager ui;

    void Awake() {
        player = GameObject.FindGameObjectWithTag ("Player");
        pm = player.GetComponent<PlayerManager>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        hitParticles = GetComponentInChildren <ParticleSystem>();
        enemyAudio = GetComponent<AudioSource>();
        enemyAudio.clip = hurtClip;
        capsuleCollider = GetComponent<CapsuleCollider>();
        currentHealth = enemyHealth;
        ui = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    void Update(){
        Movement();
        Sinking();
    }

    void Movement(){
        if(!isDead && !pm.isDead){
            nav.SetDestination(player.transform.position);
        }else {nav.enabled = false;}
    }

    void OnTriggerEnter (Collider other){
        if(other.gameObject == player){
            playerInRange = true;
            Attack();
        }
    }

    void OnTriggerExit (Collider other){
        if(other.gameObject == player){
            playerInRange = false;
        }
    }

    void Attack(){
        if(playerInRange){
            //Debug.Log("Attack!");
            if(!pm.isDead){
                pm.TakeDamage(attackDamage);
                Invoke("Attack", 1);
            }else ani.SetTrigger("PlayerDead");
        }
    }

    public void TakeDamage(int amount, Vector3 hitPoint){
        if(isDead) return;

        enemyAudio.Play();
        currentHealth -= amount;
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();
        if(currentHealth <= 0) {Death();}
    }

    void Death(){
        isDead =true;
        ani.SetTrigger("Dead");
        enemyAudio.clip = deathClip;
        enemyAudio.Play();
        ui.GoldUI(goldValue);
    }

    void StartSinking(){
        isSinking = true;
        nav.enabled = false;
        rig.isKinematic = true;
        capsuleCollider.isTrigger = true;
    }

    void Sinking(){
        if(isSinking) {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    public void Reset() {
        isSinking = false;
        isDead =false;
        currentHealth = enemyHealth;
        enemyAudio.clip = hurtClip;
        capsuleCollider.isTrigger = false;
        ani.SetTrigger("Reset");
        nav.enabled = true;
        rig.isKinematic = false;
    }

}
