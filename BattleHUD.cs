using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour {
    public Text Name;
    public Text HealthNumbers;
    //Referencing the "HealthBar" script
    public HealthBar healthBar;

    public void SetHUD(Unit unit) {
        Name.text = unit.Name;

        healthBar.SetMaxHealth(unit.MaxHP);
        healthBar.SetHealth(unit.CurrentHP);

        HealthNumbers.text = unit.CurrentHP + " / " + unit.MaxHP;
    }
}
