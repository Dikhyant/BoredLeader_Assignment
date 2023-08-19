using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerModel : MonoBehaviour
{
    [SerializeField]
    private BackwardPowerModel backwardPowerModel;

    public BackwardPowerModel _BackwardPowerModel {
        get {
            return backwardPowerModel;
        }
    }

    [SerializeField]
    private ImprisonPowerModel imprisonPowerModel;

    public ImprisonPowerModel _ImprisonPowerModel {
        get {
            return imprisonPowerModel;
        }
    }

    private static PowerModel instance;
    public static PowerModel Instance {
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
            return;
        }
    }
}
