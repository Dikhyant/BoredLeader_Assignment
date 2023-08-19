using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActuator : ICardActuator
{
    private static CardActuator instance;
    public static CardActuator Instance {
        get {
            if(instance == null) {
                instance = new CardActuator();
            }
            return instance;
        }
    }
    public void UseCard(ICard card)
    {
        if(card.GetType() == typeof(BackwardPowerModel)) {
            UseBackwardPower();
            return;
        }
        if(card.GetType() == typeof(ImprisonPowerModel)) {
            UseImprisonPower();
            return;
        }
    }

    private void UseImprisonPower() {
        PawnTargetModel.Instance.TargetPawn.PawnData.IsImprisioned = true;
        PawnTargetModel.Instance.TargetPawn.PawnData.NumberOfRoundsImprisionedFor = 0;
        CustomEvents.DispatchOnPawnImprisoned(PawnTargetModel.Instance.TargetPawn);
    }

    private void UseBackwardPower() {
        PawnTargetModel.Instance.TargetPawn.PawnData.WillGoBackwards = true;
    }
}

class CardCollector : ICardCollector
{
    private List<ICard> cards;
    public List<ICard> Cards {
        get {
            return cards;
        }
    }

    private static CardCollector instance;
    public static CardCollector Instance {
        get {
            if(instance == null) {
                instance = new CardCollector();
            }
            return instance;
        }
    }

    private CardCollector() {
        cards = new List<ICard>();
    }
}