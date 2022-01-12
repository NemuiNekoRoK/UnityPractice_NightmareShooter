using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour{
    public int health = 100;

    private Animator ani;
    private CharacterController cc;
    private TouchManager tm;
    private AudioSource playerAudio;
    public AudioClip deathClip;

    public bool isDead = false;
    private bool isMove = false;
    private UIManager ui;
    void Start(){
        ani = GetComponent<Animator>();
        tm = GetComponent<TouchManager>();
        cc = GetComponent<CharacterController>();
        playerAudio = GetComponent<AudioSource>();
        ui = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    void FixedUpdate() {
        Movement();
        AnimatorController();
    }

    void Movement(){
        if(!isDead){
            if(tm.movement != Vector3.zero){
                isMove = true;
                cc.Move(tm.movement);
                this.transform.LookAt(tm.lookAtPos);
            }else{isMove = false;}
        }
    }

    void AnimatorController(){
        ani.SetBool("IsWalking", isMove);
    }

    public void RestartLevel(){
        Application.LoadLevel(Application.loadedLevel);
    }

    public void TakeDamage (int amount){
        playerAudio.Play();
        health -= amount;
        ui.damaged = true;
        ui.HealthUI(health);
        if(health <= 0 && !isDead){
            playerAudio.clip = deathClip;
            playerAudio.Play();
            isDead = true;
            ani.SetTrigger("Die");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
