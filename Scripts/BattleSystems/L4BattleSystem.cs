using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L4BattleSystem : MonoBehaviour {

    public BattleState state;

    //Prefabs of the units
    public GameObject Chaos;
    public GameObject Order;

    Unit ChaosUnit;
    Unit OrderUnit;

    public Transform ChaosBattleStation;
    public Transform OrderBattleStation;

    public BattleHUD ChaosHUD;
    public BattleHUD OrderHUD;

    public Text Dialogue;

    public GameObject YouWon;
    public GameObject YouLost;

    int Round;

    int Cooldown1, Cooldown2, Cooldown3 = 5, Cooldown4, Cooldown5;

    // Start is called before the first frame update
    void Start() {
        Dialogue.text = "A standoff between Chaos and Order!";
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle() {
        //These -GO are temporary for transfering purposes
        GameObject ChaosGO = Instantiate(Chaos, ChaosBattleStation);
        ChaosUnit = ChaosGO.GetComponent<Unit>();

        GameObject OrderGO = Instantiate(Order, OrderBattleStation);
        OrderUnit = OrderGO.GetComponent<Unit>();

        ChaosHUD.SetHUD(ChaosUnit);
        OrderHUD.SetHUD(OrderUnit);

        yield return new WaitForSeconds(5f);

        StartCoroutine(ChaosEffect());
    }

    void PlayerTurn() {
        Round++;
        Cooldown1--;
        Cooldown2--;
        Cooldown3--;
        Cooldown4--;
        Cooldown5--;
        Dialogue.text = "Chaos' turn...";
    }

    public void OnButton1() {
        if(state != BattleState.PLAYERTURN) return;

        StartCoroutine(ChaosBasicAttack());
    }

    public void OnButton2() {
        if(state != BattleState.PLAYERTURN) return;
        if(Cooldown1 > 0) {
            Dialogue.text = "This skill still has a cooldown period of " + Cooldown1 + " round(s)!";
        } else {
            Cooldown1 = 3;
            StartCoroutine(ChaosBodyReinforcing());
        }
    }

    public void OnButton3() {
        if(state != BattleState.PLAYERTURN) return;
        if(Cooldown2 > 0) {
            Dialogue.text = "This skill still has a cooldown period of " + Cooldown2 + " round(s)!";
        } else {
            Cooldown2 = 3;
            StartCoroutine(Raid());
        }
    }

    public void OnButton4() {
        if(state != BattleState.PLAYERTURN) return;
        if(Cooldown3 > 0) {
            Dialogue.text = "This skill still has a cooldown period of " + Cooldown3 + " round(s)!";
        } else {
            Cooldown3 = 5;
            StartCoroutine(ChaoticExplosions());
        }
    }

    IEnumerator ChaosBasicAttack() {
        Dialogue.text = ChaosUnit.Name + " used \"Basic Attack\"!";
        state = BattleState.UNINTERACTABLE;

        StartCoroutine(ChaosUnit.BasicAttack(OrderUnit, OrderHUD));

        yield return new WaitForSeconds(5);

        DidOrderSurvive();
    }

    IEnumerator ChaosBodyReinforcing() {
        Dialogue.text = ChaosUnit.Name + " used \"Body Reinforcing\"!";
        state = BattleState.UNINTERACTABLE;

        StartCoroutine(ChaosUnit.BodyReinforcing(ChaosHUD));

        yield return new WaitForSeconds(5);

        DidOrderSurvive();
    }

    IEnumerator Raid() {
        Dialogue.text = ChaosUnit.Name + " used \"Raid\"!";
        state = BattleState.UNINTERACTABLE;
        
        StartCoroutine(ChaosUnit.Raid(OrderUnit, ChaosHUD, OrderHUD));

        yield return new WaitForSeconds(5);

        DidOrderSurvive();
    }

    IEnumerator ChaoticExplosions() {
        Dialogue.text = ChaosUnit.Name + " used \"Chaotic Explosions\"";
        state = BattleState.UNINTERACTABLE;

        StartCoroutine(ChaosUnit.ChaoticExplosions(OrderUnit, OrderHUD));

        yield return new WaitForSeconds(6);

        DidOrderSurvive();
    }

    void DidOrderSurvive() {
        if(OrderUnit.Alive() || !HopeGranted) {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        } else {
            Dialogue.text = "Let there be Chaos!";
            state = BattleState.WON;
            YouWon.SetActive(true);
        }
    }

    bool HopeGranted = false;
    public void EnemyTurn() {
        if(OrderUnit.CurrentHP == 0 && !HopeGranted) {
            StartCoroutine(OrdersLastHope());
            HopeGranted = true;
        } else if(OrderUnit.MaxHP - OrderUnit.CurrentHP >= 150 && Cooldown4 <= 0) {
            StartCoroutine(OrderBodyReinforcing());
            Cooldown4 = 4;
        } else if(Cooldown5 <= 0) {
            StartCoroutine(ChaosExterminator());
            Cooldown5 = 4;
        } else StartCoroutine(OrderBasicAttack());
    }

    IEnumerator OrderBasicAttack() {
        Dialogue.text = OrderUnit.Name + " used \"Basic Attack\"!";

        StartCoroutine(OrderUnit.BasicAttack(ChaosUnit, ChaosHUD));

        yield return new WaitForSeconds(5);

        DidChaosSurvive();
    }

    IEnumerator ChaosExterminator() {
        Dialogue.text = OrderUnit.Name + " used \"Chaos Exterminator\"!";

        StartCoroutine(OrderUnit.ChaosExterminator(ChaosUnit, ChaosHUD));

        yield return new WaitForSeconds(5);

        DidChaosSurvive();
    }

    IEnumerator OrderBodyReinforcing() {
        Dialogue.text = OrderUnit.Name + " used \"Body Reinforcing\"!";

        StartCoroutine(OrderUnit.BodyReinforcing(OrderHUD));

        yield return new WaitForSeconds(5);

        DidChaosSurvive();
    }

    IEnumerator OrdersLastHope() {
        Dialogue.text = OrderUnit.Name + " used \"Order\'s Last Hope\"!";

        StartCoroutine(OrderUnit.OrdersLastHope(OrderHUD));

        yield return new WaitForSeconds(5);

        DidChaosSurvive();
    }

    void DidChaosSurvive() {
        if(ChaosUnit.Alive()) {
            StartCoroutine(ChaosEffect());
        } else {
            Dialogue.text = "Order shall prevail!";
            state = BattleState.LOST;
            YouLost.SetActive(true);
        }
    }

    IEnumerator ChaosEffect() {
        state = BattleState.CHAOSEFFECT;
        switch(Random.Range(1, 9)) {
            case 1:
                Dialogue.text = "Chaotic enlargement!";
                ChaosUnit.AddMaxHP(100);
                break;
            case 2:
                Dialogue.text = "Chaotic blood supply!";
                ChaosUnit.Heal(200);
                break;
            case 3:
                Dialogue.text = "Chaotic hardening!";
                ChaosUnit.IncreaseDefence(0.75f);
                break;
            case 4:
                Dialogue.text = "Chaotic bloodlust!";
                ChaosUnit.Damage += 25;
                break;
            case 5:
                Dialogue.text = "Chaotic shrinking!";
                OrderUnit.AddMaxHP(-100);
                break;
            case 6:
                Dialogue.text = "Chaotic blood drain!";
                if(OrderUnit.CurrentHP <= 200) OrderUnit.CurrentHP = 1; else OrderUnit.CurrentHP -= 200;
                break;
            case 7:
                Dialogue.text = "Chaotic softening!";
                OrderUnit.IncreaseDefence(-0.75f);
                break;
            case 8:
                Dialogue.text = "Chaotic nightmare!";
                OrderUnit.Damage -= 25;
                break;
        }
        ChaosHUD.SetHUD(ChaosUnit);
        OrderHUD.SetHUD(OrderUnit);

        yield return new WaitForSeconds(5);
    
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
}