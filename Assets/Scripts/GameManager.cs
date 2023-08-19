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
    private IPawnProvider pawnProvider;

    

    void OnEnable() {
        CustomEvents.OnCardClicked += HandleOnCardClicked;
        CustomEvents.OnDiceNumberGenerated += HandleOnDiceNumberGenerated;
    }

    void OnDisable() {
        CustomEvents.OnCardClicked -= HandleOnCardClicked;
        CustomEvents.OnDiceNumberGenerated -= HandleOnDiceNumberGenerated;
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

        if(PawnMoverProvider.Instance != null) {
            pawnMover = PawnMoverProvider.Instance.GetPawnMover();
        }

        cardActuator = CardActuator.Instance;
        
    }

    private void HandleOnCardClicked(ICard card) {
        if(cardActuator == null) return;
        cardActuator.UseCard(card);
    }

    private void HandleOnDiceNumberGenerated(int diceNumber) {
        if(pawnMover == null) {
            pawnMover = PawnMoverProvider.Instance?.GetPawnMover();
        }
        if(PawnManager.Instance.CurrentPawn.PawnData.WillGoBackwards) {
            pawnMover.MoveBackward_By_Steps(PawnManager.Instance.CurrentPawn, diceNumber);
            return;
        }

        pawnMover.MoveForward_By_Steps(PawnManager.Instance.CurrentPawn, diceNumber);
    }


    public delegate void DiceNumberGenaretedEvent(int diceNumber);
    public DiceNumberGenaretedEvent onDiceNumberGenerated_Event;
}
