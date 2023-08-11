using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [Range(0,1)]
    public float timeTakenByPawnToMove;

    [SerializeField]
    private Cell currentCell;

    public Vector3 positionOffset;
    private enum PawnFacingDirection {
        DirectionA,
        DirectionB
    }

    private PawnFacingDirection currentFacingDirection;

    void OnEnable() {
        // movedToDestination += UpdateCurrentCell;
    }

    void OnDisable() {
        // movedToDestination -= UpdateCurrentCell;
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

    /* void MoveForward_By_Steps(int steps) {
        void ReduceSteps() {
            steps--;
        }
        void ResetSteps() {
            steps = 0;
        }
        movedToDestinationEvent += ReduceSteps;
        cantMoveAheadEvent += ResetSteps;

        while(steps > 0) {
            GoToNextCell();
        }
        movedToDestinationEvent -= ReduceSteps;
        cantMoveAheadEvent -= ResetSteps;
    }

    void MoveBackward_By_Steps(int steps) {
        void ReduceSteps() {
            steps--;
        }
        void ResetSteps() {
            steps = 0;
        }
        movedToDestinationEvent += ReduceSteps;
        cantMoveAheadEvent += ResetSteps;

        while(steps > 0) {
            GoToPreviousCell();
        }
        movedToDestinationEvent -= ReduceSteps;
        cantMoveAheadEvent -= ResetSteps;
    } */

    void GoToNextCell() {
        if(currentFacingDirection == PawnFacingDirection.DirectionA && currentCell.NextCellInDirectionA != null) {
            void UpdateCurrentCell(){
                currentCell = currentCell.NextCellInDirectionA;
                movedToDestinationEvent -= UpdateCurrentCell;
            }
            movedToDestinationEvent += UpdateCurrentCell;

            MoveTo(
                currentCell.NextCellInDirectionA.transform.position + positionOffset,
                currentCell.NextCellInDirectionA.transform.rotation
            );
            return;
        }

        if(currentFacingDirection == PawnFacingDirection.DirectionB && currentCell.NextCellInDirectionB != null) {
            void UpdateCurrentCell(){
                currentCell = currentCell.NextCellInDirectionB;
                movedToDestinationEvent -= UpdateCurrentCell;
            }
            movedToDestinationEvent += UpdateCurrentCell;

            MoveTo(
                currentCell.NextCellInDirectionB.transform.position + positionOffset,
                currentCell.NextCellInDirectionB.transform.rotation
            );
            return;
        }

        cantMoveAheadEvent?.Invoke();
    }

    void GoToPreviousCell() {
        if(currentFacingDirection == PawnFacingDirection.DirectionA && currentCell.NextCellInDirectionB != null) {
            void UpdateCurrentCell(){
                currentCell = currentCell.NextCellInDirectionB;
                movedToDestinationEvent -= UpdateCurrentCell;
            }
            movedToDestinationEvent += UpdateCurrentCell;

            MoveTo(
                currentCell.NextCellInDirectionB.transform.position + positionOffset,
                currentCell.NextCellInDirectionB.transform.rotation
            );
            return;
        }

        if(currentFacingDirection == PawnFacingDirection.DirectionB && currentCell.NextCellInDirectionA != null) {
            void UpdateCurrentCell(){
                currentCell = currentCell.NextCellInDirectionA;
                movedToDestinationEvent -= UpdateCurrentCell;
            }
            movedToDestinationEvent += UpdateCurrentCell;

            MoveTo(
                currentCell.NextCellInDirectionA.transform.position + positionOffset,
                currentCell.NextCellInDirectionA.transform.rotation
            );
            return;
        }

        cantMoveAheadEvent?.Invoke();
    }

    void MoveTo(Vector3 finalPosition, Quaternion finalRotation) {
        StartCoroutine(MoveToCoroutine( finalPosition, finalRotation));
    }

    IEnumerator MoveToCoroutine(Vector3 finalPosition, Quaternion finalRotation) {
        float lerpAmount = 0;
        float deltaLerpAmount = 0.01f;
        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;
        while(lerpAmount < 1) {
            transform.position = Vector3.Lerp( initialPosition, finalPosition, lerpAmount);
            transform.rotation = Quaternion.Lerp( initialRotation, finalRotation, lerpAmount);
            lerpAmount += deltaLerpAmount;
            yield return new WaitForSeconds(timeTakenByPawnToMove);
        }
        movedToDestinationEvent?.Invoke();
    }

    void UpdateCurrentCell() {
        if(currentFacingDirection == PawnFacingDirection.DirectionA && currentCell.NextCellInDirectionA != null) {
            currentCell = currentCell.NextCellInDirectionA;
            return;
        }
        if(currentFacingDirection == PawnFacingDirection.DirectionB && currentCell.NextCellInDirectionB != null) {
            currentCell = currentCell.NextCellInDirectionB;
            return;
        }
        if(currentFacingDirection == PawnFacingDirection.DirectionA && currentCell.NextCellInDirectionA != null) {
            currentCell = currentCell.NextCellInDirectionA;
            return;
        }
        if(currentFacingDirection == PawnFacingDirection.DirectionB && currentCell.NextCellInDirectionB != null) {
            currentCell = currentCell.NextCellInDirectionB;
            return;
        }
    }

    private delegate void InternalEvent();
    private event InternalEvent movedToDestinationEvent;
    private event InternalEvent cantMoveAheadEvent;
}
