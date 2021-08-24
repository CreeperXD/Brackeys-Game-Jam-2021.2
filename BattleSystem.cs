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
        Dialogue.text = "This skill is not unlocked(actually developed)... yet...";
    }

    public void OnButton4() {
        Dialogue.text = "This skill is not unlocked(actually developed)... yet...";
    }

    IEnumerator BasicAttack() {
        Dialogue.text = "<unit> used \"Basic Attack\"!";
        state = BattleState.UNINTERACTABLE;

        yield return new WaitForSeconds(2.5f);

        OrderUnit.TakeDamage(ChaosUnit.Damage);
        OrderHUD.SetHUD(OrderUnit);

        yield return new WaitForSeconds(0.5f);

        OrderUnit.TakeDamage(ChaosUnit.Damage);
        OrderHUD.SetHUD(OrderUnit);

        yield return new WaitForSeconds(2.5f);

        DidOrderSurvive();
    }

    IEnumerator BodyHardening() {
        Dialogue.text = "<unit> used \"Body Hardening\"!";
        state = BattleState.UNINTERACTABLE;

        yield return new WaitForSeconds(2.5f);

        ChaosUnit.Heal((int)(ChaosUnit.Damage * 1.5f));
        ChaosUnit.Defence += 0.5f;
        ChaosHUD.SetHUD(ChaosUnit);

        yield return new WaitForSeconds(2.5f);

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
        if(OrderUnit.CurrentHP == 300) StartCoroutine(GearUp()); else StartCoroutine(BurstFire());
    }

    IEnumerator BurstFire() {
        Dialogue.text = "<unit> used \"Burst Fire\"!";

        yield return new WaitForSeconds(2.5f);

        ChaosUnit.TakeDamage(OrderUnit.Damage);
        ChaosHUD.SetHUD(ChaosUnit);

        yield return new WaitForSeconds(0.25f);

        ChaosUnit.TakeDamage(OrderUnit.Damage);
        ChaosHUD.SetHUD(ChaosUnit);

        yield return new WaitForSeconds(0.25f);

        ChaosUnit.TakeDamage(OrderUnit.Damage);
        ChaosHUD.SetHUD(ChaosUnit);

        yield return new WaitForSeconds(2.5f);

        DidChaosSurvive();
    }

    IEnumerator GearUp() {
        Dialogue.text = "<unit> used \"Gear Up\"!";

        yield return new WaitForSeconds(2.5f);

        OrderUnit.Defence += 0.25f;

        yield return new WaitForSeconds(2.5f);

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
