using System;
using UnityEngine;

public class CustomEvents
{
    public static Action OnDiceClicked;
    public static Action<ICard> OnCardClicked;
    public static Action OnCurrentPawnFinishedTurn;
    public static Action<int> OnDiceNumberGenerated;
    public static Action<Pawn> OnPawnForfeitARound;
    public static Action<Pawn> OnPawnImprisoned;
    public static Action<Pawn> OnPawnFreedFromPrison;

    public static void DispatchOnDiceClicked() {
        OnDiceClicked?.Invoke();
    }

    public static void DispatchOnCardClicked(ICard card) {
        OnCardClicked?.Invoke(card);
    }

    public static void DispatchOnCurrentPawnFinishedTurn() {
        OnCurrentPawnFinishedTurn?.Invoke();
    }

    public static void DispatchOnDiceNumberGenerated(int diceNumber) {
        OnDiceNumberGenerated?.Invoke(diceNumber);
    }

    public static void DispatchOnPawnForfeitARound(Pawn pawn) {
        OnPawnForfeitARound?.Invoke(pawn);
    }

    public static void DispatchOnPawnImprisoned(Pawn pawn) {
        OnPawnImprisoned?.Invoke(pawn);
    }

    public static void DispatchOnPawnFreedFromPrison(Pawn pawn) {
        OnPawnFreedFromPrison?.Invoke(pawn);
    }
}
