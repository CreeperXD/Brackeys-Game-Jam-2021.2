using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L3BattleSystem : MonoBehaviour {

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

    int Round;

    int Cooldown1, Cooldown2;

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

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn() {
        Round++;
        Cooldown1--;
        Cooldown2--;
        Dialogue.text = "Chaos' turn...";
    }

    public void OnButton1() {
        if(state != BattleState.PLAYERTURN) return;

        StartCoroutine(BasicAttack());
    }

    public void OnButton2() {
        if(state != BattleState.PLAYERTURN) return;
        if(Cooldown1 > 0) {
            Dialogue.text = "This skill still has a cooldown period of " + Cooldown1 + " round(s)!";
        } else {
            Cooldown1 = 3;
            StartCoroutine(BodyReinforcing());
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

    IEnumerator BasicAttack() {
        Dialogue.text = ChaosUnit.Name + " used \"Basic Attack\"!";
        state = BattleState.UNINTERACTABLE;

        StartCoroutine(ChaosUnit.BasicAttack(OrderUnit, OrderHUD));

        yield return new WaitForSeconds(5);

        DidOrderSurvive();
    }

    IEnumerator BodyReinforcing() {
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

    void DidOrderSurvive() {
        if(OrderUnit.Alive()) {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        } else {
            Dialogue.text = "Let there be Chaos!";
            state = BattleState.WON;
        }
    }

    public void EnemyTurn() {
        if(Round % 4 == 1) StartCoroutine(Unyielding()); else StartCoroutine(Smite());
    }

    IEnumerator Unyielding() {
        Dialogue.text = OrderUnit.Name + " used \"Unyielding\"!";

        StartCoroutine(OrderUnit.Unyielding());

        yield return new WaitForSeconds(5);

        DidChaosSurvive();
    }

    IEnumerator Smite() {
        Dialogue.text = OrderUnit.Name + " used \"Smite\"!";

        StartCoroutine(OrderUnit.Smite(ChaosUnit, ChaosHUD));

        yield return new WaitForSeconds(5);

        DidChaosSurvive();
    }

    void DidChaosSurvive() {
        if(ChaosUnit.Alive()) {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        } else {
            Dialogue.text = "Order shall prevail!";
            state = BattleState.LOST;
        }
    }
}