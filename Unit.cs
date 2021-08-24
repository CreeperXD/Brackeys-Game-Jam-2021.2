using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public string Name;
    public int Damage;
    public float Defence;
    public int MaxHP;
    public int CurrentHP;

    public void TakeDamage(int Amount) {
        CurrentHP -= (Amount - (int)(Amount * Defence));
    }

    public void Heal(int Amount) {
        CurrentHP += Amount;
        if(CurrentHP > MaxHP) CurrentHP = MaxHP;
    }

    public bool Alive() {
        if(CurrentHP > 0) return true; else return false;
    }

    // public void SkillName(int Amount) {
    //  use functions like this to handle moves?
    // }
}
