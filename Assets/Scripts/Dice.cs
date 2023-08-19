using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : IDice
{
    private int diceNumber;
    public int DiceNumber {
        get {
            return diceNumber;
        }
        set {
            diceNumber = value;
        }
    }
}
