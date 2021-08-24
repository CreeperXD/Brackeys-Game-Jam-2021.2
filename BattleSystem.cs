using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState {START, UNINTERACTABLE, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour {

    public BattleState state;

    //GameObjects to reference prefabs, Units for actual logics
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

    public void OnAttackButton() {
        if(state != BattleState.PLAYERTURN) return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton() {
        if(state != BattleState.PLAYERTURN) return;

        StartCoroutine(PlayerHeal());
    }

    IEnumerator PlayerAttack() {
        Dialogue.text = "Chaos launched an assault!";
        state = BattleState.UNINTERACTABLE;

        yield return new WaitForSeconds(2.5f);

        Dialogue.text = "Order took some damage!";
        OrderUnit.TakeDamage(ChaosUnit.Damage);
        OrderHUD.SetHUD(OrderUnit);

        yield return new WaitForSeconds(2.5f);

        DidOrderSurvive();
    }

    IEnumerator PlayerHeal() {
        Dialogue.text = "Chaos is requesting support!";
        state = BattleState.UNINTERACTABLE;

        yield return new WaitForSeconds(2.5f);

        Dialogue.text = "Chaos recovered some strength!";
        ChaosUnit.Heal(ChaosUnit.Damage);
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

    int choice;

    public void EnemyTurn() {
        choice++;
        switch(choice) {
            case 1:
                StartCoroutine(EnemyAttack());
                break;
            case 2:
                StartCoroutine(EnemyHeal());
                choice -= 2;
                break;
        }
    }

    IEnumerator EnemyAttack() {
        Dialogue.text = "Order retaliated!";
        yield return new WaitForSeconds(2.5f);

        Dialogue.text = "Chaos took some damage!";
        ChaosUnit.TakeDamage(OrderUnit.Damage);
        ChaosHUD.SetHUD(ChaosUnit);

        yield return new WaitForSeconds(2.5f);

        DidChaosSurvive();
    }

    IEnumerator EnemyHeal() {
        Dialogue.text = "Order is requesting backup!";
        yield return new WaitForSeconds(2.5f);

        Dialogue.text = "Order recovered some strength!";
        OrderUnit.Heal(OrderUnit.Damage);
        OrderHUD.SetHUD(OrderUnit);

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
