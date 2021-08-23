using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    
    public Slider HealthBarSlider;

    public void SetHealth(int Health) {
        HealthBarSlider.value = Health;
    }

    public void SetMaxHealth(int Health) {
        HealthBarSlider.maxValue = Health;
    }
}
