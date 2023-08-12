using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUIController : MonoBehaviour
{
    [SerializeField]
    private Button diceRoll_Btn;

    [SerializeField]
    private GameManager gameManager;
    void OnEnable() {
        if(gameManager) {
            gameManager.onDiceNumberGenerated_Event += HandleDiceNumberGenerated;
        }
        
        if(diceRoll_Btn) {
            diceRoll_Btn.onClick.AddListener(RollDice);
        }
    }

    void OnDisable() {
        if(gameManager) {
            gameManager.onDiceNumberGenerated_Event -= HandleDiceNumberGenerated;
        }
        
        if(diceRoll_Btn) {
            diceRoll_Btn.onClick.RemoveAllListeners();
        }
    }

    void Awake() {
        if(gameManager == null) {
            Debug.LogError("Game Manager not found");
        }
    }

    void HandleDiceNumberGenerated(int diceNumber) {
        Debug.Log(diceNumber);
    }

    public async void RollDice() {
        if(gameManager == null) return;
        diceRoll_Btn.interactable = false;
        await gameManager.RollDice();
        diceRoll_Btn.interactable = true;
    }
}
