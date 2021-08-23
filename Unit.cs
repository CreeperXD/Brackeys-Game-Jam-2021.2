using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public string Name;
    public int Damage;
    public int MaxHP;
    public int CurrentHP;

    public void TakeDamage(int Amount) {
        CurrentHP -= Amount;
    }

    public void Heal(int Amount) {
        CurrentHP += Amount;
        if(CurrentHP > MaxHP) CurrentHP = MaxHP;
    }

    public bool Alive() {
        if(CurrentHP > 0) return true; else return false;
    }
}
