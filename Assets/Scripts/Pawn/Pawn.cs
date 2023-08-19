using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [Range(0,1)]
    public float timeTakenByPawnToMove; // in seconds

    [Range(0,1)]
    public float delayInMovingFromCellToCell; // in seconds

    [SerializeField]
    private Cell currentCell;

    public Cell CurrentCell {
        get {
            return currentCell;
        }

        set {
            currentCell = value;
        }
    }

    private static Pawn currentPawn;

    public static Pawn CurrentPawn {
        get {
            return currentPawn;
        }
    }

    private Cell initialCell;

    public Vector3 positionOffset;
    public enum PawnFacingDirection {
        DirectionA,
        DirectionB
    }

    private PawnFacingDirection currentFacingDirection;

    public PawnFacingDirection CurrentFacingDirection {
        get {
            return currentFacingDirection;
        }
    }


    private IPawnData pawnData;
    public IPawnData PawnData {
        get {
            return pawnData;
        }
    }

    void Awake() {
        if(currentCell) {
            initialCell = currentCell;
        }

        pawnData = PawnDataCreator.Instance.CreatePawnData();
    }

    void Start()
    {
        FigureOutInitialFacingDirection();
    }

    void FigureOutInitialFacingDirection() {
        if(currentCell != null && currentCell.NextCellInDirectionA == null) {
            currentFacingDirection = PawnFacingDirection.DirectionB;
            return;
        }
        if(currentCell != null && currentCell.NextCellInDirectionB == null) {
            currentFacingDirection = PawnFacingDirection.DirectionA;
        }
    }



    public async Task ResetToInitialCell() {
        // await MoveTo_Coroutine(initialCell.transform.position + positionOffset, initialCell.transform.rotation);
        currentCell = initialCell;
        FigureOutInitialFacingDirection();
    }
    
}

class PawnData : IPawnData
{
    private bool willGoBackwards;
    public bool WillGoBackwards {
        set {
            willGoBackwards = value;
        }
        get {
            return willGoBackwards;
        }
    }

    private bool isImprisioned;
    public bool IsImprisioned {
        set {
            isImprisioned = value;
        }
        get {
            return isImprisioned;
        }
    }

    private int numberOfRoundsImprisionedFor;
    public int NumberOfRoundsImprisionedFor {
        set {
            numberOfRoundsImprisionedFor = value;
        }
        get {
            return numberOfRoundsImprisionedFor;
        }
    }
}

class PawnDataCreator {
    private static PawnDataCreator instance;
    public static PawnDataCreator Instance {
        get {
            if(instance == null) {
                instance = new PawnDataCreator();
            }
            return instance;
        }
    }

    public IPawnData CreatePawnData() {
        return new PawnData();
    }
}

class PawnTargetModel : IPawnTargetModel
{
    private Pawn targetPawn;
    public Pawn TargetPawn {
        // code inside get section is just for assignment. May change later
        get {
            if(PawnManager.Instance.CurrentPawn == PawnManager.Instance.Pawns[0]) return PawnManager.Instance.Pawns[1];
            return PawnManager.Instance.Pawns[0];
        }
    }

    private static PawnTargetModel instance;
    public static PawnTargetModel Instance {
        get {
            if(instance == null) {
                instance = new PawnTargetModel();
            }
            return instance;
        }
    }
}

