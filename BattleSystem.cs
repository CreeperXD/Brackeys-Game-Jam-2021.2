using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState {START, UNINTERACTABLE, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour {

    public BattleState state;

    //GameObjects to reference prefabs, Units for actual logics
    public GameObject Chaos1;
    public GameObject Order1;
    Unit ChaosUnit1;
    Unit OrderUnit1;

    public Transform ChaosBattleStation1;
    public Transform OrderBattleStation1;

    public BattleHUD Chaos1HUD;
    public BattleHUD Order1HUD;

    // Start is called before the first frame update
    void Start() {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle() {
        //These -GO are temporary for transfering purposes
        GameObject Chaos1GO = Instantiate(Chaos1, ChaosBattleStation1);
        ChaosUnit1 = Chaos1GO.GetComponent<Unit>();

        GameObject Order1GO = Instantiate(Order1, OrderBattleStation1);
        OrderUnit1 = Order1GO.GetComponent<Unit>();

        Chaos1HUD.SetHUD(ChaosUnit1);
        Order1HUD.SetHUD(OrderUnit1);

        yield return new WaitForSeconds(5f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn() {

    }

    public void OnAttackButton() {
        if(state != BattleState.PLAYERTURN) return;

        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack() {
        state = BattleState.UNINTERACTABLE;

        yield return new WaitForSeconds(2.5f);

        OrderUnit1.TakeDamage(ChaosUnit1.Damage);
        Order1HUD.SetHUD(OrderUnit1);

        yield return new WaitForSeconds(2.5f);

        if(OrderUnit1.Alive()) {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        } else state = BattleState.WON;
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
        yield return new WaitForSeconds(2.5f);

        ChaosUnit1.TakeDamage(OrderUnit1.Damage);
        Chaos1HUD.SetHUD(ChaosUnit1);

        yield return new WaitForSeconds(2.5f);

        if(ChaosUnit1.Alive()) {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        } else state = BattleState.LOST;
    }

    IEnumerator EnemyHeal() {
        yield return new WaitForSeconds(2.5f);

        OrderUnit1.Heal(OrderUnit1.Damage);
        Order1HUD.SetHUD(OrderUnit1);

        yield return new WaitForSeconds(2.5f);

        if(ChaosUnit1.Alive()) {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        } else state = BattleState.LOST;
    }
}
