using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PawnMover : MonoBehaviour, IPawnMover
{
    
    public float timeTakenByPawnToMove;
    public async Task MovePawnBySteps(Pawn pawn, int steps)
    {
        MoveForward_By_Steps(pawn, steps);
    }

    public async Task MoveForward_By_Steps(Pawn pawn, int steps) {
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
    }

    async Task<bool> GoToNextCell_Coroutine() {
        if(Pawn.CurrentPawn.CurrentFacingDirection == Pawn.PawnFacingDirection.DirectionA && Pawn.CurrentPawn.CurrentCell.NextCellInDirectionA != null) {
            await MovePawnTo(
                Pawn.CurrentPawn,
                Pawn.CurrentPawn.CurrentCell.NextCellInDirectionA.transform.position + Pawn.CurrentPawn.positionOffset,
                Pawn.CurrentPawn.CurrentCell.NextCellInDirectionA.transform.rotation
            );
            Pawn.CurrentPawn.CurrentCell = Pawn.CurrentPawn.CurrentCell.NextCellInDirectionA;
            return true;
        }

        if(Pawn.CurrentPawn.CurrentFacingDirection == Pawn.PawnFacingDirection.DirectionB && Pawn.CurrentPawn.CurrentCell.NextCellInDirectionB != null) {
            await MovePawnTo(
                Pawn.CurrentPawn,
                Pawn.CurrentPawn.CurrentCell.NextCellInDirectionB.transform.position + Pawn.CurrentPawn.positionOffset,
                Pawn.CurrentPawn.CurrentCell.NextCellInDirectionB.transform.rotation
            );
            Pawn.CurrentPawn.CurrentCell = Pawn.CurrentPawn.CurrentCell.NextCellInDirectionB;
            return true;
        }

        return false;
    }

    async Task<bool> GoToPreviousCell_Coroutine() {
        if(Pawn.CurrentPawn.CurrentFacingDirection == Pawn.PawnFacingDirection.DirectionA && Pawn.CurrentPawn.CurrentCell.NextCellInDirectionB != null) {
            await MovePawnTo(
                Pawn.CurrentPawn,
                Pawn.CurrentPawn.CurrentCell.NextCellInDirectionB.transform.position + Pawn.CurrentPawn.positionOffset,
                Pawn.CurrentPawn.CurrentCell.NextCellInDirectionB.transform.rotation
            );
            Pawn.CurrentPawn.CurrentCell = Pawn.CurrentPawn.CurrentCell.NextCellInDirectionB;

            return true;
        }

        if(Pawn.CurrentPawn.CurrentFacingDirection == Pawn.PawnFacingDirection.DirectionB && Pawn.CurrentPawn.CurrentCell.NextCellInDirectionA != null) {
            await MovePawnTo(
                Pawn.CurrentPawn,
                Pawn.CurrentPawn.CurrentCell.NextCellInDirectionA.transform.position + Pawn.CurrentPawn.positionOffset,
                Pawn.CurrentPawn.CurrentCell.NextCellInDirectionA.transform.rotation
            );
            Pawn.CurrentPawn.CurrentCell = Pawn.CurrentPawn.CurrentCell.NextCellInDirectionA;

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
