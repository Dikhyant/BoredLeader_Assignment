using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnManager : MonoBehaviour, IPawnProvider
{
    [SerializeField]
    private List<Pawn> pawns;
    private INextPlayerDecisionMaker nextPlayerDecisionMaker;
    private IWhichPlayerWillStartTheRoundDecisionMaker whichPlayerWillStartTheRoundDecisionMaker;

    public List<Pawn> Pawns {
        get {
            return pawns;
        }
    }

    private static PawnManager instance;
    public static PawnManager Instance {
        get {
            return instance;
        }
    }

    private Pawn currentPawn;
    public Pawn CurrentPawn {
        get {
            return currentPawn;
        }
    }

    void OnEnable() {
        CustomEvents.OnCurrentPawnFinishedTurn += HandleOnCurrentPawnFinishedTurn;
    }

    void OnDisable() {
        CustomEvents.OnCurrentPawnFinishedTurn -= HandleOnCurrentPawnFinishedTurn;
    }

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else if(instance != null) {
            Destroy(this);
        }

        nextPlayerDecisionMaker = NextPlayerDecisionMaker.Instance;
        whichPlayerWillStartTheRoundDecisionMaker = WhichPlayerWillStartTheRoundDecisionMaker.Instance;
        currentPawn = whichPlayerWillStartTheRoundDecisionMaker.GetFirstPlayer();
    }

    void HandleOnCurrentPawnFinishedTurn() {
        currentPawn = nextPlayerDecisionMaker.GetNextPlayerToPlay();
    }

}

class NextPlayerDecisionMaker : INextPlayerDecisionMaker
{
    private bool shouldLetCurrentPlayerHaveAnotherTurn = false;
    private Pawn playerNotAllowedToPlayOneRound;
    private static NextPlayerDecisionMaker instance;
    public static NextPlayerDecisionMaker Instance {
        get {
            if(instance == null) {
                instance = new NextPlayerDecisionMaker();
            }
            return instance;
        }
    }
    public Pawn GetNextPlayerToPlay()
    {
        if(PawnManager.Instance == null) return null;
        if(PawnManager.Instance.Pawns == null) return null;
        if(PawnManager.Instance.Pawns.Count == 0) return null;
        
        int i = 0;
        int numberOfPawnsUnableToPlay = 0;

        while(i < PawnManager.Instance.Pawns.Count && numberOfPawnsUnableToPlay < PawnManager.Instance.Pawns.Count) {
            if(PawnManager.Instance.Pawns[i] == PawnManager.Instance.CurrentPawn) {
                i = i + 1 == PawnManager.Instance.Pawns.Count ? 0 : i + 1;
            }

            if(PawnManager.Instance.Pawns[i].PawnData.IsImprisioned) {
                CustomEvents.DispatchOnPawnForfeitARound(PawnManager.Instance.Pawns[i]);
                numberOfPawnsUnableToPlay++;
                i = i + 1 == PawnManager.Instance.Pawns.Count ? 0 : i + 1;
            }

            else {
                return PawnManager.Instance.Pawns[i];
            }
        }

        return null;
    }

    public void DontLetPlayerHaveATurn(Pawn pawn)
    {
        playerNotAllowedToPlayOneRound = pawn;
    }
}

class WhichPlayerWillStartTheRoundDecisionMaker : IWhichPlayerWillStartTheRoundDecisionMaker {
    private static WhichPlayerWillStartTheRoundDecisionMaker instance;
    public static WhichPlayerWillStartTheRoundDecisionMaker Instance {
        get {
            if(instance == null) {
                instance = new WhichPlayerWillStartTheRoundDecisionMaker();
            }
            return instance;
        }
    }

    public Pawn GetFirstPlayer()
    {
        if(PawnManager.Instance == null) return null;
        if(PawnManager.Instance.Pawns == null) return null;
        if(PawnManager.Instance.Pawns.Count == 0) return null;

        return PawnManager.Instance.Pawns[0];
    }
}
