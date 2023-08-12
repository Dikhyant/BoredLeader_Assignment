using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    [SerializeField]
    private Pawn PawnA;

    [SerializeField]
    private Pawn PawnB;

    private Pawn currentPawnToPlay = null;
    public bool PawnA_PlaysFirst;

    void Awake() {
        if(PawnA == null) {
            Debug.LogError("PawnA not found");
        }
        if(PawnB == null) {
            Debug.LogError("PawnB not found");
        }

        if(PawnA != null && PawnB != null && PawnA_PlaysFirst) {
            currentPawnToPlay = PawnA;
        }

        if(PawnA != null && PawnB != null && !PawnA_PlaysFirst) {
            currentPawnToPlay = PawnB;
        }
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
