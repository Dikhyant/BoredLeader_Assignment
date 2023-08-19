using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    private Dice dice;
    private static DiceManager instance;
    public static DiceManager Instance {
        get {
            return instance;
        }
    }

    void OnEnable() {
        CustomEvents.OnDiceClicked += HandleOnDiceClicked;
    }
    
    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(this);
            return;
        }

        if(dice == null) {
            dice = new Dice();
        }
    }

    private void HandleOnDiceClicked() {
        GenerateNumberForDice();
    }

    public int GetDiceNumber() {
        return dice.DiceNumber;
    }

    public async Task GenerateNumberForDice() {
        await Task.Delay(2000);
        dice.DiceNumber = (new System.Random()).Next(1, 7);
        CustomEvents.DispatchOnDiceNumberGenerated(dice.DiceNumber);
    }

    
}
