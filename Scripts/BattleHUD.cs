using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour {
    public TextMeshProUGUI Name, HealthNumbers, DamageText, DefenceText;
    //Referencing the "HealthBar" script
    public HealthBar healthBar;

    public void SetHUD(Unit unit) {
        unit.CurrentHP = (int)unit.CurrentHP;
        // unit.Defence = (int)unit.Defence;

        Name.text = unit.Name;
        healthBar.SetMaxHealth(unit.MaxHP);
        healthBar.SetHealth((int)unit.CurrentHP);

        HealthNumbers.text = unit.CurrentHP + " / " + unit.MaxHP;
        DamageText.text = unit.Damage + "";
        DefenceText.text = (int)(unit.Defence * 100) + "%";
    }
}