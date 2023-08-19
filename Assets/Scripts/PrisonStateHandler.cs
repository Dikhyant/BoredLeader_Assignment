using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonStateHandler : MonoBehaviour {
    void OnEnable() {
        CustomEvents.OnPawnForfeitARound += HandleOnPawnForfeitARound;
    }

    void OnDisable() {
        CustomEvents.OnPawnForfeitARound -= HandleOnPawnForfeitARound;
    }


    private void HandleOnPawnForfeitARound(Pawn pawn) {
        pawn.PawnData.NumberOfRoundsImprisionedFor++;
        if(pawn.PawnData.NumberOfRoundsImprisionedFor > PowerModel.Instance._ImprisonPowerModel.MaxNumberOfRoundsToBeImprisioned) {
            pawn.PawnData.IsImprisioned = false;
            pawn.PawnData.NumberOfRoundsImprisionedFor = 0;
            CustomEvents.DispatchOnPawnFreedFromPrison(pawn);
        }
    }

}