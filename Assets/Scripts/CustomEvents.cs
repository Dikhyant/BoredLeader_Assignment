using System;
using UnityEngine;

public class CustomEvents : MonoBehaviour
{
    public static Action OnDiceClicked;
    public static Action<ICard> OnCardClicked;

    public static void DispatchOnDiceClicked() {
        OnDiceClicked?.Invoke();
    }

    public static void DispatchOnCardClicked(ICard card) {
        OnCardClicked?.Invoke(card);
    }
}
