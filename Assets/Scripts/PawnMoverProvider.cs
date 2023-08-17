using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMoverProvider : MonoBehaviour
{
    [SerializeField]
    private PawnMover pawnMover;
    private static PawnMoverProvider instance;
    public static PawnMoverProvider Instance {
        get {
            return instance;
        }
    }

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(this);
        }
    }

    public IPawnMover GetPawnMover() {
        return pawnMover;
    }
}
