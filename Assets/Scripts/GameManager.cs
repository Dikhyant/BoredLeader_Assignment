using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {
        get {
            return instance;
        }
    }

    [SerializeField]
    private Pawn PawnA;

    [SerializeField]
    private Pawn PawnB;

    private Pawn currentPawnToPlay = null;

    void Awake()
    {
        if(instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(this);
        }

        if(PawnA != null && PawnB != null) {
            currentPawnToPlay = PawnA;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CreateGameManager() {
        GameObject gameObject = Instantiate((new GameObject()));
        gameObject.AddComponent<GameManager>();
        gameObject.name = "GameManager";
    }

    public async Task RollDice() {
        int diceNumber = await GenerateNumberForDice();
        onDiceNumberGenerated_Event?.Invoke(diceNumber);
        await MoveCurrentPawnBySteps(diceNumber);
    }

    public async Task<int> GenerateNumberForDice() {
        await Task.Delay(1000);
        return (new System.Random()).Next(1, 3);
    }

    async Task MoveCurrentPawnBySteps(int steps) {
        if(PawnA == null) return;
        if(PawnB == null) return;
        
        if(currentPawnToPlay == PawnA) {
            await PawnA.MoveForward_By_Steps(steps);
            currentPawnToPlay = PawnB;
        }
        else if(currentPawnToPlay == PawnB) {
            await PawnB.MoveForward_By_Steps(steps);
            currentPawnToPlay = PawnA;
        }
    }

    public delegate void DiceNumberGenaretedEvent(int diceNumber);
    public DiceNumberGenaretedEvent onDiceNumberGenerated_Event;
}
