using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    private int id;
    public int Id {
        get {
            return id;
        }
        set {
            id = value;
        }
    }
    
    List<Cell> cells;
}
