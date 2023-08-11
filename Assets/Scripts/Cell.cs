using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private Cell nextCellInDirectionA;

    public Cell NextCellInDirectionA {
        get {
            return nextCellInDirectionA;
        }
    }

    [SerializeField]
    private Cell nextCellInDirectionB;

    public Cell NextCellInDirectionB {
        get {
            return nextCellInDirectionB;
        }
    }

    void OnDrawGizmos() {
        if(nextCellInDirectionA != null) {
            // Gizmos.color = Color.red;
            Cell.ForGizmo(
                transform.position,
                nextCellInDirectionA.transform.position,
                Color.red
            );
            // Gizmos.DrawLine(transform.position, nextCellInDirectionA.transform.position);
        }
        if(nextCellInDirectionB != null) {
            // Gizmos.color = Color.blue;
            Cell.ForGizmo(
                transform.position,
                nextCellInDirectionB.transform.position,
                Color.blue
            );
            // Gizmos.DrawLine(transform.position, nextCellInDirectionB.transform.position);
        }
    }

    public static void ForGizmo(Vector3 initialPosition, Vector3 finalPosition, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(initialPosition, finalPosition);
       
        Vector3 right = Quaternion.LookRotation(finalPosition) * Quaternion.Euler(0,180+arrowHeadAngle,0) * new Vector3(0,0,1);
        Vector3 left = Quaternion.LookRotation(finalPosition) * Quaternion.Euler(0,180-arrowHeadAngle,0) * new Vector3(0,0,1);
        Gizmos.DrawRay( initialPosition + Vector3.Normalize(finalPosition - initialPosition), right * arrowHeadLength);
        Gizmos.DrawRay( initialPosition + Vector3.Normalize(finalPosition - initialPosition), left * arrowHeadLength);
    }
}
