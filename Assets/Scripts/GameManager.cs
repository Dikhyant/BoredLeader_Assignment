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
        ApplyPowerEffect();
        int diceNumber = await GenerateNumberForDice();
        onDiceNumberGenerated_Event?.Invoke(diceNumber);
        await MoveCurrentPawnBySteps(diceNumber);
        CheckForImprisonment();
    }

    public async Task<int> GenerateNumberForDice() {
        await Task.Delay(2000);
        return (new System.Random()).Next(1, 7);
    }

    async Task MoveCurrentPawnBySteps(int steps) {
        if(PawnA == null) return;
        if(PawnB == null) return;

        if(currentPawnToPlay.willGoBackwards && currentPawnToPlay == PawnA) {
            await PawnA.MoveBackward_By_Steps(steps);
            PawnA.willGoBackwards = false;
            // currentPawnToPlay = PawnB;
            return;
        }

        if(currentPawnToPlay.willGoBackwards && currentPawnToPlay == PawnB) {
            await PawnB.MoveBackward_By_Steps(steps);
            PawnB.willGoBackwards = false;
            // currentPawnToPlay = PawnA;
            return;
        }
        
        if(currentPawnToPlay == PawnA) {
            await PawnA.MoveForward_By_Steps(steps);
            // currentPawnToPlay = PawnB;
            return;
        }
        
        if(currentPawnToPlay == PawnB) {
            await PawnB.MoveForward_By_Steps(steps);
            // currentPawnToPlay = PawnA;
            return;
        }
    }

    public void CheckForImprisonment() {
        if(PawnB.isImprisioned && 
           PawnB.numberOfRoundsImprisionedFor < Pawn.maxNumberOfRoundsToBeImprisioned &&
           currentPawnToPlay == PawnA) {
            
            PawnB.numberOfRoundsImprisionedFor++;
            currentPawnToPlay = PawnA;
            return;
        }

        if(PawnB.isImprisioned && 
           PawnB.numberOfRoundsImprisionedFor >= Pawn.maxNumberOfRoundsToBeImprisioned &&
           currentPawnToPlay == PawnA) {

            PawnB.numberOfRoundsImprisionedFor = 0;
            PawnB.isImprisioned = false;
            currentPawnToPlay = PawnB;
            return;
        }

        if(PawnA.isImprisioned && 
           PawnA.numberOfRoundsImprisionedFor < Pawn.maxNumberOfRoundsToBeImprisioned &&
           currentPawnToPlay == PawnB) {
            
            PawnA.numberOfRoundsImprisionedFor++;
            currentPawnToPlay = PawnB;
            return;
        }

        if(PawnA.isImprisioned && 
           PawnA.numberOfRoundsImprisionedFor >= Pawn.maxNumberOfRoundsToBeImprisioned &&
           currentPawnToPlay == PawnB) {

            PawnA.numberOfRoundsImprisionedFor = 0;
            PawnA.isImprisioned = false;
            currentPawnToPlay = PawnA;
            return;
        }

        if(currentPawnToPlay == PawnA) {
            currentPawnToPlay = PawnB;
            return;
        }

        if(currentPawnToPlay == PawnB) {
            currentPawnToPlay = PawnA;
            return;
        }
    }

    public void PowerUsed(Pawn.PowerType powerType) {
        if(PawnA == null) return;
        if(PawnB == null) return;

        if(powerType == Pawn.PowerType.BACKWARD && 
        currentPawnToPlay == PawnA && 
        PawnB.numberOfRoundsImprisionedFor == Pawn.maxNumberOfRoundsToBeImprisioned && PawnB.isImprisioned) {
            PawnA.powerUsed = powerType;
            return;
        }

        if(powerType == Pawn.PowerType.BACKWARD && 
        currentPawnToPlay == PawnB && 
        PawnA.numberOfRoundsImprisionedFor == Pawn.maxNumberOfRoundsToBeImprisioned && PawnA.isImprisioned) {
            PawnB.powerUsed = powerType;
            return;
        }

        if(currentPawnToPlay == PawnA) {
            PawnA.powerUsed = powerType;
            return;
        }
        
        if(currentPawnToPlay == PawnB) {
            PawnB.powerUsed = powerType;
            return;
        }
    }

    private void ApplyPowerEffect() {
        if(currentPawnToPlay == null) return;
        if(currentPawnToPlay.powerUsed == Pawn.PowerType.NONE) return;

        if(currentPawnToPlay.powerUsed == Pawn.PowerType.BACKWARD && currentPawnToPlay == PawnA) {
            PawnB.willGoBackwards = true;
            currentPawnToPlay.powerUsed = Pawn.PowerType.NONE;
            return;
        }

        if(currentPawnToPlay.powerUsed == Pawn.PowerType.IMPRISON && currentPawnToPlay == PawnA) {
            PawnB.isImprisioned = true;
            PawnB.numberOfRoundsImprisionedFor = 0;
            currentPawnToPlay.powerUsed = Pawn.PowerType.NONE;
            return;
        }

        if(currentPawnToPlay.powerUsed == Pawn.PowerType.BACKWARD && currentPawnToPlay == PawnB) {
            PawnA.willGoBackwards = true;
            currentPawnToPlay.powerUsed = Pawn.PowerType.NONE;
            return;
        }

        if(currentPawnToPlay.powerUsed == Pawn.PowerType.IMPRISON && currentPawnToPlay == PawnB) {
            PawnA.isImprisioned = true;
            PawnA.numberOfRoundsImprisionedFor = 0;
            currentPawnToPlay.powerUsed = Pawn.PowerType.NONE;
            return;
        }
    }

    public delegate void DiceNumberGenaretedEvent(int diceNumber);
    public DiceNumberGenaretedEvent onDiceNumberGenerated_Event;
}
