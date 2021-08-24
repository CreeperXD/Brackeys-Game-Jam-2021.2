using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public string Name;
    public int Damage;
    public float Defence;
    public int MaxHP;
    public int CurrentHP;

    //**********
    //Base actions a move can have
    //**********

    public void TakeDamage(int Amount) {
        CurrentHP -= Amount - (int)(Amount * Defence);
        if(CurrentHP < 0) CurrentHP = 0;
    }

    public void Heal(int Amount) {
        CurrentHP += Amount;
        if(CurrentHP > MaxHP) CurrentHP = MaxHP;
    }

    public void ModifyDamage(float Multiplier) {
        Damage = (int)(Damage * Multiplier);
    }

    public void IncreaseDefence(float Amount) {
        Defence += Amount;
        if(Defence > 1) Defence = 1;
    }

    public bool Alive() {
        if(CurrentHP > 0) return true; else return false;
    }

    //**********
    //Chaos moves
    //**********

    public IEnumerator BasicAttack(Unit EnemyUnit, BattleHUD EnemyHUD) {
        yield return new WaitForSeconds(2.5f);

        EnemyUnit.TakeDamage(Damage);
        EnemyHUD.SetHUD(EnemyUnit);

        yield return new WaitForSeconds(0.5f);

        EnemyUnit.TakeDamage(Damage);
        EnemyHUD.SetHUD(EnemyUnit);
    }

    public IEnumerator BodyHardening(BattleHUD SelfHUD) {
        yield return new WaitForSeconds(2.5f);

        Heal((int)(Damage * 1.5f));
        IncreaseDefence(0.5f);
        SelfHUD.SetHUD(this);
    }

    public IEnumerator LeechingBite(Unit EnemyUnit, BattleHUD SelfHUD, BattleHUD EnemyHUD) {
        yield return new WaitForSeconds(2.5f);

        EnemyUnit.TakeDamage((int)(Damage * 0.5f));
        EnemyUnit.ModifyDamage(0.9f);
        EnemyUnit.IncreaseDefence(-0.1f);
        Heal(Damage);
        EnemyHUD.SetHUD(EnemyUnit);
        SelfHUD.SetHUD(this);
    }

    public IEnumerator ChaoticExplosions(Unit EnemyUnit, BattleHUD EnemyHUD) {
        int i = 1;

        IEnumerator ChaoticExplosion() {
            EnemyUnit.TakeDamage((int)(Damage * 3.5f));
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

    //**********
    //Order moves
    //**********

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

    public IEnumerator GearUp() {
        yield return new WaitForSeconds(2.5f);

        IncreaseDefence(0.25f);
    }
}
