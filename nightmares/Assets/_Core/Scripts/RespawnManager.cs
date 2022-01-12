using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour{
    public GameObject nightmare;
    public int count = 2;
    public float respawnTime = 2.0f;

    private Transform tra;
    private GameObject [] enemy;
    private Enemy em;

    void Start(){
        tra = GetComponent<Transform>();
        enemy = new GameObject[count];
        for(int i = 0; i< count; i++){
            enemy[i] = Instantiate(nightmare, tra.position, tra.rotation) as GameObject;
            em = enemy[i].GetComponent<Enemy>();
            em.isDead = true;
            enemy[i].SetActive(false);
        }
        StartCoroutine(RespawnNightmare());
    }

    IEnumerator RespawnNightmare(){
        yield return new WaitForSeconds(respawnTime);
        for(int i = 0; i < count; i++){
            em = enemy[i].GetComponent<Enemy>();
            if(em.isDead){
                em.transform.position = tra.position;
                enemy[i].SetActive(true);
                em.Reset();
                break;
            }
        }
        StartCoroutine(RespawnNightmare());
    }
}
