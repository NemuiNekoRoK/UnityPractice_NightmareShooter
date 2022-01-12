using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour{
    public Slider healthSlider;
    public Text ammoText;
    public Text goldText;
    public bool damaged;
    public Image damageImage;

    private int gold=0;
    private float flashSpeed = 5f;
    private Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    void Update(){
        DamageImg();
    }
    
    void DamageImg(){
        if(damaged){
            damageImage.color = flashColour;
        }else{
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void HealthUI(int health){
        healthSlider.value = health;
    }

    public void ShootUI(int ammo){
        ammoText.text = "Reload: " + ammo;
    }

    public void GoldUI(int goldValue){
        gold += goldValue;
        goldText.text = "Gold : " + gold;
    }
}
