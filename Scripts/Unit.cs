using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Unit : MonoBehaviour {
    public string Name;
    public float Damage;
    public float Defence;
    public int MaxHP;
    public float CurrentHP;

    //**********
    //Base actions a move can have
    //**********

    public void TakeDamage(float Amount) {
        CurrentHP -= Amount - Amount * Defence;
        if(CurrentHP < 0) CurrentHP = 0;
    }

    public void Heal(float Amount) {
        CurrentHP += Amount;
        if(CurrentHP > MaxHP) CurrentHP = MaxHP;
    }

    public void AddMaxHP(int Amount) {
        MaxHP += Amount;
        if(CurrentHP > MaxHP) CurrentHP = MaxHP;
    }

    public void AddDamage(float Amount) {
        Damage += Amount;
        if(Damage < 0) Damage = 0;
    }

    public void IncreaseDefence(float Amount) {
        Defence += Amount;
        if(Defence > 1) Defence = 1;
    }

    public bool Alive() {
        if(CurrentHP > 0) return true; else return false;
    }

    //**********
    //All moves
    //**********

    public IEnumerator BasicAttack(Unit EnemyUnit, BattleHUD EnemyHUD) {
        yield return new WaitForSeconds(2.5f);

        EnemyUnit.TakeDamage(Damage);
        EnemyHUD.SetHUD(EnemyUnit);

        yield return new WaitForSeconds(0.5f);

        EnemyUnit.TakeDamage(Damage);
        EnemyHUD.SetHUD(EnemyUnit);
    }

    public IEnumerator BodyReinforcing(BattleHUD SelfHUD) {
        yield return new WaitForSeconds(2.5f);

        Heal(Damage * 1.5f);
        IncreaseDefence(0.5f);
        SelfHUD.SetHUD(this);
    }

    public IEnumerator Raid(Unit EnemyUnit, BattleHUD SelfHUD, BattleHUD EnemyHUD) {
        yield return new WaitForSeconds(2.5f);

        EnemyUnit.AddDamage(-25);
        EnemyUnit.IncreaseDefence(-0.25f);
        EnemyUnit.TakeDamage(Damage);
        Heal(Damage);
        EnemyHUD.SetHUD(EnemyUnit);
        SelfHUD.SetHUD(this);
    }

    public IEnumerator ChaoticExplosions(Unit EnemyUnit, BattleHUD EnemyHUD) {
        int i = 1;

        IEnumerator ChaoticExplosion() {
            EnemyUnit.TakeDamage(Damage * 3.5f);
            EnemyUnit.IncreaseDefence(-0.1f);
            EnemyHUD.SetHUD(EnemyUnit);
            // Debug.Log(i);

            if(i < 4) {
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(ChaoticExplosion());
                i++;
            }
        }

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(ChaoticExplosion());
    }

    public IEnumerator BurstFire(Unit EnemyUnit, BattleHUD EnemyHUD) {
        yield return new WaitForSeconds(2.5f);

        EnemyUnit.TakeDamage(Damage);
        EnemyHUD.SetHUD(EnemyUnit);

        yield return new WaitForSeconds(0.25f);

        EnemyUnit.TakeDamage(Damage);
        EnemyHUD.SetHUD(EnemyUnit);

        yield return new WaitForSeconds(0.25f);

        EnemyUnit.TakeDamage(Damage);
        EnemyHUD.SetHUD(EnemyUnit);
    }

    public IEnumerator GearUp(BattleHUD SelfHUD) {
        yield return new WaitForSeconds(2.5f);

        IncreaseDefence(0.25f);
        SelfHUD.SetHUD(this);
    }

    public IEnumerator Bazooka(Unit EnemyUnit, BattleHUD EnemyHUD) {
        yield return new WaitForSeconds(2.5f);

        EnemyUnit.TakeDamage(Damage * 1.75f);
        EnemyUnit.IncreaseDefence(-0.25f);
        EnemyHUD.SetHUD(EnemyUnit);
    }

    public IEnumerator Grenade(Unit EnemyUnit, BattleHUD EnemyHUD) {
        yield return new WaitForSeconds(2.5f);

        EnemyUnit.TakeDamage(Damage * 1.25f);
        EnemyHUD.SetHUD(EnemyUnit);
    }

    public IEnumerator Unyielding(BattleHUD SelfHUD) {
        yield return new WaitForSeconds(2.5f);

        IncreaseDefence(0.5f);
        SelfHUD.SetHUD(this);
    }

    public IEnumerator Smite(Unit EnemyUnit, BattleHUD EnemyHUD) {
        yield return new WaitForSeconds(1);

        EnemyUnit.TakeDamage(Damage);
        EnemyUnit.IncreaseDefence(-0.05f);
        EnemyHUD.SetHUD(EnemyUnit);

        yield return new WaitForSeconds(1);

        EnemyUnit.TakeDamage(Damage);
        EnemyUnit.IncreaseDefence(-0.05f);
        EnemyHUD.SetHUD(EnemyUnit);

        yield return new WaitForSeconds(1);

        EnemyUnit.TakeDamage(Damage);
        EnemyUnit.IncreaseDefence(-0.05f);
        EnemyHUD.SetHUD(EnemyUnit);
    }

    public IEnumerator ChaosExterminator(Unit EnemyUnit, BattleHUD EnemyHUD) {
        yield return new WaitForSeconds(1);

        EnemyUnit.TakeDamage(Damage * 3);
        EnemyUnit.IncreaseDefence(-0.5f);
        EnemyHUD.SetHUD(EnemyUnit);
    }

    public IEnumerator OrdersLastHope(BattleHUD SelfHUD) {
        yield return new WaitForSeconds(2.5f);

        MaxHP = 2500; //int
        CurrentHP = 2500; //float
        Damage = 200;
        Defence = 1;
        SelfHUD.SetHUD(this);
    }
}