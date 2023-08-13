using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUIController : MonoBehaviour
{
    [SerializeField]
    private Button diceRoll_Btn;

    [SerializeField]
    private Image diceRoll_SpriteRenderer;

    [SerializeField]
    private Button backwardPower_Btn;

    [SerializeField]
    private Button imprisionPower_Btn;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private Sprite[] diceFaces;

    private bool keepAnimationDiceRoll = false;

    void OnEnable() {
        if(gameManager) {
            gameManager.onDiceNumberGenerated_Event += HandleDiceNumberGenerated;
        }
        
        if(diceRoll_Btn) {
            diceRoll_Btn.onClick.AddListener(RollDice);
        }

        if(backwardPower_Btn) {
            backwardPower_Btn.onClick.AddListener(OnBackwardPowerClicked);
        }

        if(imprisionPower_Btn) {
            imprisionPower_Btn.onClick.AddListener(OnImprisionPowerClicked);
        }
    }

    void OnDisable() {
        if(gameManager) {
            gameManager.onDiceNumberGenerated_Event -= HandleDiceNumberGenerated;
        }
        
        if(diceRoll_Btn) {
            diceRoll_Btn.onClick.RemoveAllListeners();
        }

        if(backwardPower_Btn) {
            backwardPower_Btn.onClick.RemoveAllListeners();
        }

        if(imprisionPower_Btn) {
            imprisionPower_Btn.onClick.RemoveAllListeners();
        }
    }

    void Awake() {
        if(gameManager == null) {
            Debug.LogError("Game Manager not found");
        }

        if(diceRoll_Btn == null) {
            Debug.LogError("Dice Roll Btn not found");
        }

        if(backwardPower_Btn == null) {
            Debug.LogError("Backward Power Btn not found");
        }

        if(imprisionPower_Btn == null) {
            Debug.LogError("Imprison Power Btn not found");
        }

        if(diceRoll_SpriteRenderer == null) {
            Debug.LogError("Dice roll sprite renderer not found");
        }
    }

    void HandleDiceNumberGenerated(int diceNumber) {
        Debug.Log(diceNumber);
        keepAnimationDiceRoll = false;
        diceRoll_SpriteRenderer.sprite = diceFaces[diceNumber - 1];
    }

    public async void RollDice() {
        if(gameManager == null) return;
        diceRoll_Btn.interactable = false;
        backwardPower_Btn.interactable = false;
        imprisionPower_Btn.interactable = false;

        AnimateDiceRoll();
        await gameManager.RollDice();
        keepAnimationDiceRoll = false;
        
        diceRoll_Btn.interactable = true;
        backwardPower_Btn.interactable = true;
        imprisionPower_Btn.interactable = true;
    }

    private async void AnimateDiceRoll() {
        if(diceFaces.Length == 0) return;
        if(diceRoll_SpriteRenderer == null) return;

        keepAnimationDiceRoll = true;
        int index = 0;
        float timeWhenDiceRolled = Time.realtimeSinceStartup;

        while(keepAnimationDiceRoll && (Time.realtimeSinceStartup - timeWhenDiceRolled) < 10) {
            diceRoll_SpriteRenderer.sprite = diceFaces[index];
            index++;
            index = index >= diceFaces.Length ? 0 : index;
            await Task.Delay(200);
        }
        
    }

    public void OnBackwardPowerClicked() {
        gameManager.PowerUsed(Pawn.PowerType.BACKWARD);
        backwardPower_Btn.interactable = false;
    }

    public void OnImprisionPowerClicked() {
        gameManager.PowerUsed(Pawn.PowerType.IMPRISON);
        imprisionPower_Btn.interactable = false;
    }
}
