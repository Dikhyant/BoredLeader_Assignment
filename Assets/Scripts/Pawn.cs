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

    public Vector3 positionOffset;
    private enum PawnFacingDirection {
        DirectionA,
        DirectionB
    }

    private PawnFacingDirection currentFacingDirection;

    public enum PowerType {
        BACKWARD,
        IMPRISON,
        NONE
    }

    public PowerType powerUsed;
    public bool isImprisioned = false;
    public bool willGoBackwards = false;
    public int numberOfRoundsImprisionedFor = 0;
    public static int maxNumberOfRoundsToBeImprisioned = 2;

    void Awake() {
        powerUsed = PowerType.NONE;
    }

    void Start()
    {
        FigureOutInitialFacingDirection();
        // SimpleTaskExecutioner();
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

    IEnumerator SimpleCoroutine() {
        yield break;

    }

    async void SimpleTaskExecutioner() {
        await Task.Delay(1000);
        await MoveForward_By_Steps(40);

        await Task.Delay(1000);
        await MoveBackward_By_Steps(30);

        await Task.Delay(1000);
        await MoveForward_By_Steps(2);
    }

    public async Task MoveForward_By_Steps(int steps) {
        bool couldMoveToNextCell = false;
        while(steps > 0) {
            couldMoveToNextCell = await GoToNextCell_Coroutine();
            if(couldMoveToNextCell) {
                await Task.Delay((int)(delayInMovingFromCellToCell * 1000));
                steps--;
            } else {
                steps = 0;
            }
            
        }
    }

    public async Task MoveBackward_By_Steps(int steps) {
        bool couldMoveToNextCell = false;
        while(steps > 0) {
            couldMoveToNextCell = await GoToPreviousCell_Coroutine();
            if(couldMoveToNextCell) {
                await Task.Delay((int)(delayInMovingFromCellToCell * 1000));
                steps--;
            } else {
                steps = 0;
            }
            
        }
    }

    async Task<bool> GoToNextCell_Coroutine() {
        if(currentFacingDirection == PawnFacingDirection.DirectionA && currentCell.NextCellInDirectionA != null) {
            await MoveTo_Coroutine(
                currentCell.NextCellInDirectionA.transform.position + positionOffset,
                currentCell.NextCellInDirectionA.transform.rotation
            );
            currentCell = currentCell.NextCellInDirectionA;
            return true;
        }

        if(currentFacingDirection == PawnFacingDirection.DirectionB && currentCell.NextCellInDirectionB != null) {
            await MoveTo_Coroutine(
                currentCell.NextCellInDirectionB.transform.position + positionOffset,
                currentCell.NextCellInDirectionB.transform.rotation
            );
            currentCell = currentCell.NextCellInDirectionB;

            return true;
        }

        return false;
    }

    async Task<bool> GoToPreviousCell_Coroutine() {
        if(currentFacingDirection == PawnFacingDirection.DirectionA && currentCell.NextCellInDirectionB != null) {
            await MoveTo_Coroutine(
                currentCell.NextCellInDirectionB.transform.position + positionOffset,
                currentCell.NextCellInDirectionB.transform.rotation
            );
            currentCell = currentCell.NextCellInDirectionB;

            return true;
        }

        if(currentFacingDirection == PawnFacingDirection.DirectionB && currentCell.NextCellInDirectionA != null) {
            await MoveTo_Coroutine(
                currentCell.NextCellInDirectionA.transform.position + positionOffset,
                currentCell.NextCellInDirectionA.transform.rotation
            );
            currentCell = currentCell.NextCellInDirectionA;

            return true;
        }

        return false;
    }

    async Task MoveTo_Coroutine(Vector3 finalPosition, Quaternion finalRotation) {
        float lerpAmount = 0;
        float deltaLerpAmount = 0.01f;
        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;
        while(lerpAmount < 1) {
            transform.position = Vector3.Lerp( initialPosition, finalPosition, lerpAmount);
            transform.rotation = Quaternion.Lerp( initialRotation, finalRotation, lerpAmount);
            lerpAmount += deltaLerpAmount;
            await Task.Delay((int)(timeTakenByPawnToMove * 1000));
        }
    }
}
