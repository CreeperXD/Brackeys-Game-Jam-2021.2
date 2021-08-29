using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L2BattleSystem : MonoBehaviour {

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

    int Cooldown;

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
        Cooldown--;
        Dialogue.text = "Chaos' turn...";
    }

    public void OnButton1() {
        if(state != BattleState.PLAYERTURN) return;

        StartCoroutine(BasicAttack());
    }

    public void OnButton2() {
        if(state != BattleState.PLAYERTURN) return;
        if(Cooldown > 0) {
            Dialogue.text = "This skill still has a cooldown period of " + Cooldown + " round(s)!";
        } else {
            Cooldown = 3;
            StartCoroutine(BodyReinforcing());
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

    void DidOrderSurvive() {
        if(OrderUnit.Alive()) {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        } else {
            Dialogue.text = "Let there be Chaos!";
            state = BattleState.WON;
            YouWon.SetActive(true);
        }
    }

    public void EnemyTurn() {
        if(Round % 3 == 1) StartCoroutine(Bazooka()); else StartCoroutine(Grenade());
    }

    IEnumerator Bazooka() {
        Dialogue.text = OrderUnit.Name + " used \"Bazooka\"!";

        StartCoroutine(OrderUnit.Bazooka(ChaosUnit, ChaosHUD));

        yield return new WaitForSeconds(5);

        DidChaosSurvive();
    }

    IEnumerator Grenade() {
        Dialogue.text = OrderUnit.Name + " used \"Grenade\"!";

        StartCoroutine(OrderUnit.Grenade(ChaosUnit, ChaosHUD));

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
            YouLost.SetActive(true);
        }
    }
}