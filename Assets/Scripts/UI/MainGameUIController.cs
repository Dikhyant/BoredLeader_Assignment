using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUIController : MonoBehaviour
{
    [SerializeField]
    private Button diceRoll_Btn;
    void OnEnable() {
        GameManager.Instance.onDiceNumberGenerated_Event += HandleDiceNumberGenerated;
        if(diceRoll_Btn) {
            diceRoll_Btn.onClick.AddListener(RollDice);
        }
    }

    void OnDisable() {
        GameManager.Instance.onDiceNumberGenerated_Event -= HandleDiceNumberGenerated;
        if(diceRoll_Btn) {
            diceRoll_Btn.onClick.RemoveAllListeners();
        }
    }

    void Awake() {
        if(GameManager.Instance == null) {
            GameManager.CreateGameManager();
        }
    }

    void HandleDiceNumberGenerated(int diceNumber) {
        Debug.Log(diceNumber);
    }

    public async void RollDice() {
        diceRoll_Btn.interactable = false;
        await GameManager.Instance.RollDice();
        diceRoll_Btn.interactable = true;
    }
}
