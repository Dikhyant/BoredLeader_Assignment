using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PawnMover : MonoBehaviour, IPawnMover
{

    public float timeTakenByPawnToMove;

    public async Task MovePawnBySteps(Pawn pawn, int steps)
    {
        await MoveForward_By_Steps(pawn, steps);
        CustomEvents.DispatchOnCurrentPawnFinishedTurn();
    }

    public async Task MoveForward_By_Steps(Pawn pawn, int steps) {
        if(pawn == null) return;
        bool couldMoveToNextCell = false;
        while(steps > 0) {
            couldMoveToNextCell = await GoToNextCell_Coroutine();
            if(couldMoveToNextCell) {
                await Task.Delay((int)(pawn.delayInMovingFromCellToCell * 1000));
                steps--;
            } else {
                steps = 0;
            }
            
        }

        CustomEvents.DispatchOnCurrentPawnFinishedTurn();
    }

    public async Task MoveBackward_By_Steps(Pawn pawn, int steps) {
        if(pawn == null) return;
        bool couldMoveToNextCell = false;
        while(steps > 0) {
            couldMoveToNextCell = await GoToPreviousCell_Coroutine();
            if(couldMoveToNextCell) {
                await Task.Delay((int)(pawn.delayInMovingFromCellToCell * 1000));
                steps--;
            } else {
                steps = 0;
            }
            
        }

        CustomEvents.DispatchOnCurrentPawnFinishedTurn();
    }

    async Task<bool> GoToNextCell_Coroutine() {
        if(PawnManager.Instance == null) return false;
        if(PawnManager.Instance.CurrentPawn.CurrentFacingDirection == Pawn.PawnFacingDirection.DirectionA && PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionA != null) {
            await MovePawnTo(
                PawnManager.Instance.CurrentPawn,
                PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionA.transform.position + PawnManager.Instance.CurrentPawn.positionOffset,
                PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionA.transform.rotation
            );
            PawnManager.Instance.CurrentPawn.CurrentCell = PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionA;
            return true;
        }

        if(PawnManager.Instance.CurrentPawn.CurrentFacingDirection == Pawn.PawnFacingDirection.DirectionB && PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionB != null) {
            await MovePawnTo(
                PawnManager.Instance.CurrentPawn,
                PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionB.transform.position + PawnManager.Instance.CurrentPawn.positionOffset,
                PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionB.transform.rotation
            );
            PawnManager.Instance.CurrentPawn.CurrentCell = PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionB;
            return true;
        }

        return false;
    }

    async Task<bool> GoToPreviousCell_Coroutine() {
        if(PawnManager.Instance == null) return false;
        if(PawnManager.Instance.CurrentPawn.CurrentFacingDirection == Pawn.PawnFacingDirection.DirectionA && PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionB != null) {
            await MovePawnTo(
                PawnManager.Instance.CurrentPawn,
                PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionB.transform.position + PawnManager.Instance.CurrentPawn.positionOffset,
                PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionB.transform.rotation
            );
            PawnManager.Instance.CurrentPawn.CurrentCell = PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionB;

            return true;
        }

        if(PawnManager.Instance.CurrentPawn.CurrentFacingDirection == Pawn.PawnFacingDirection.DirectionB && PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionA != null) {
            await MovePawnTo(
                PawnManager.Instance.CurrentPawn,
                PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionA.transform.position + PawnManager.Instance.CurrentPawn.positionOffset,
                PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionA.transform.rotation
            );
            PawnManager.Instance.CurrentPawn.CurrentCell = PawnManager.Instance.CurrentPawn.CurrentCell.NextCellInDirectionA;

            return true;
        }

        return false;
    }

    async Task MovePawnTo(Pawn pawn, Vector3 finalPosition, Quaternion finalRotation) {
        float lerpAmount = 0;
        float deltaLerpAmount = 0.01f;
        Vector3 initialPosition = pawn.transform.position;
        Quaternion initialRotation = pawn.transform.rotation;
        while(lerpAmount < 1) {
            pawn.transform.position = Vector3.Lerp( initialPosition, finalPosition, lerpAmount);
            pawn.transform.rotation = Quaternion.Lerp( initialRotation, finalRotation, lerpAmount);
            lerpAmount += deltaLerpAmount;
            await Task.Delay((int)(timeTakenByPawnToMove * 1000));
        }
    }
}
