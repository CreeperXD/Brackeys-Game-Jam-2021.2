using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour {

    public BattleState state;

    public GameObject Chaos1;
    public GameObject Order1;

    public Transform ChaosBattleStation1;
    public Transform OrderBattleStation1;

    Unit ChaosUnit1;
    Unit OrderUnit1;

    // Start is called before the first frame update
    void Start() {
        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle() {
        GameObject Chaos1GO = Instantiate(Chaos1, ChaosBattleStation1);
        ChaosUnit1 = Chaos1GO.GetComponent<Unit>();

        GameObject Order1GO = Instantiate(Order1, OrderBattleStation1);
        OrderUnit1 = Order1GO.GetComponent<Unit>();
    }
}
