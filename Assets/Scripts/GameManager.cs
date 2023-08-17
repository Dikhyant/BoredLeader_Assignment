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
    
    [SerializeField]
    private PrisonAnimator prisonAnimator;

    private Pawn currentPawnToPlay = null;
    public bool PawnA_PlaysFirst;

    private ICardActuator cardActuator;
    private IPawnMover pawnMover;

    void OnEnable() {
        CustomEvents.OnCardClicked += HandleOnCardClicked;
    }

    void OnDisable() {
        CustomEvents.OnCardClicked -= HandleOnCardClicked;
    }

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

        if(prisonAnimator == null) {
            Debug.LogError("Prison animator not found");
        }

        
        pawnMover = PawnMoverProvider.Instance.GetPawnMover();
    }

    private void HandleOnCardClicked(ICard card) {
        if(cardActuator == null) return;
        cardActuator.UseCard(card);
    }

    public async Task RollDice() {
        if(pawnMover == null) return;
        ApplyPowerEffect();
        int diceNumber = await GenerateNumberForDice();
        onDiceNumberGenerated_Event?.Invoke(diceNumber);
        await pawnMover.MovePawnBySteps(Pawn.CurrentPawn, diceNumber);
        // await MoveCurrentPawnBySteps(diceNumber);
        // await CheckIfPawnSteppedOnOpponentPawn();
        ChooseNextPlayer();
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

    private async Task CheckIfPawnSteppedOnOpponentPawn() {
        if(currentPawnToPlay == PawnA && PawnA.CurrentCell == PawnB.CurrentCell) {
            await PawnB.ResetToInitialCell();
            return;
        }

        if(currentPawnToPlay == PawnB && PawnA.CurrentCell == PawnB.CurrentCell) {
            await PawnA.ResetToInitialCell();
            return;
        }
    }

    public void ChooseNextPlayer() {
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
            if(prisonAnimator) {
                prisonAnimator.RemovePrison();
            }
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
            if(prisonAnimator) {
                prisonAnimator.RemovePrison();
            }
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
        currentPawnToPlay == PawnA && 
        PawnB.numberOfRoundsImprisionedFor < Pawn.maxNumberOfRoundsToBeImprisioned && PawnB.isImprisioned) {
            // backward powered cannot be used;
            return;
        }

        if(powerType == Pawn.PowerType.BACKWARD && 
        currentPawnToPlay == PawnB && 
        PawnA.numberOfRoundsImprisionedFor == Pawn.maxNumberOfRoundsToBeImprisioned && PawnA.isImprisioned) {
            PawnB.powerUsed = powerType;
            return;
        }

        if(powerType == Pawn.PowerType.BACKWARD && 
        currentPawnToPlay == PawnB && 
        PawnA.numberOfRoundsImprisionedFor < Pawn.maxNumberOfRoundsToBeImprisioned && PawnA.isImprisioned) {
            return;
        }

        if(currentPawnToPlay == PawnA && powerType == Pawn.PowerType.IMPRISON) {
            PawnA.powerUsed = powerType;
            if(prisonAnimator) {
                prisonAnimator.SpawnPrisonAt(PawnB.transform.position);
            }
            return;
        }

        if(currentPawnToPlay == PawnB && powerType == Pawn.PowerType.IMPRISON) {
            PawnB.powerUsed = powerType;
            if(prisonAnimator) {
                prisonAnimator.SpawnPrisonAt(PawnA.transform.position);
            }
            return;
        }

        if(currentPawnToPlay == PawnA && powerType == Pawn.PowerType.BACKWARD) {
            PawnA.powerUsed = powerType;
            return;
        }
        
        if(currentPawnToPlay == PawnB && powerType == Pawn.PowerType.BACKWARD) {
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
