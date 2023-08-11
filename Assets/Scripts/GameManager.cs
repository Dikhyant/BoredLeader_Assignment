using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateNumberForDice() {
        StartCoroutine(GenerateNumberForDiceCoroutine());
    }

    IEnumerator GenerateNumberForDiceCoroutine() {
        yield return new WaitForSeconds(1);
        int numberForDice = (new System.Random()).Next(1, 7);
    }
}
