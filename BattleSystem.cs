using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState {START, UNINTERACTABLE, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour {

    public BattleState state;

    //GameObjects to reference the unit prefabs, Units for actual logics
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
        Dialogue.text = "Chaos' turn...";
    }

    public void OnButton1() {
        if(state != BattleState.PLAYERTURN) return;

        StartCoroutine(BasicAttack());
    }

    public void OnButton2() {
        if(state != BattleState.PLAYERTURN) return;

        StartCoroutine(BodyHardening());
    }

    public void OnButton3() {
        if(state != BattleState.PLAYERTURN) return;

        StartCoroutine(LeechingBite());
    }

    public void OnButton4() {
        if(state != BattleState.PLAYERTURN) return;

        StartCoroutine(ChaoticExplosions());
    }

    IEnumerator BasicAttack() {
        Dialogue.text = ChaosUnit.Name + " used \"Basic Attack\"!";
        state = BattleState.UNINTERACTABLE;

        StartCoroutine(ChaosUnit.BasicAttack(OrderUnit, OrderHUD));

        yield return new WaitForSeconds(5);

        DidOrderSurvive();
    }

    IEnumerator BodyHardening() {
        Dialogue.text = ChaosUnit.Name + " used \"Body Hardening\"!";
        state = BattleState.UNINTERACTABLE;

        StartCoroutine(ChaosUnit.BodyHardening(ChaosHUD));

        yield return new WaitForSeconds(5);

        DidOrderSurvive();
    }

    IEnumerator LeechingBite() {
        Dialogue.text = ChaosUnit.Name + " used \"Leeching Bite\"!";
        state = BattleState.UNINTERACTABLE;
        
        StartCoroutine(ChaosUnit.LeechingBite(OrderUnit, ChaosHUD, OrderHUD));

        yield return new WaitForSeconds(5);

        DidOrderSurvive();
    }

    IEnumerator ChaoticExplosions() {
        Dialogue.text = ChaosUnit.Name + " used \"Chaotic Explosions\"!";
        state = BattleState.UNINTERACTABLE;
        
        StartCoroutine(ChaosUnit.ChaoticExplosions(OrderUnit, OrderHUD));

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
        if(Round == 1) StartCoroutine(GearUp()); else StartCoroutine(BurstFire());
    }

    IEnumerator BurstFire() {
        Dialogue.text = OrderUnit.Name + " used \"Burst Fire\"!";

        StartCoroutine(OrderUnit.BurstFire(ChaosUnit, ChaosHUD));

        yield return new WaitForSeconds(5);

        DidChaosSurvive();
    }

    IEnumerator GearUp() {
        Dialogue.text = OrderUnit.Name + " used \"Gear Up\"!";

        StartCoroutine(OrderUnit.GearUp());

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
