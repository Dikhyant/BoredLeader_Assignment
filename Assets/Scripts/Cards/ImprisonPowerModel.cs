using UnityEngine;

[CreateAssetMenu(menuName = "ImprisonCard")]
public class ImprisonPowerModel : ScriptableObject , ICard
{
    [SerializeField]
    private int maxNumberOfRoundsToBeImprisioned;

    public int MaxNumberOfRoundsToBeImprisioned {
        get {
            return maxNumberOfRoundsToBeImprisioned;
        }
    }
}
