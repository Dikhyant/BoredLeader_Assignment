using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PrisonAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject prisonPrefab;

    private GameObject prisonGameObject;

    public Vector3 appearingOffset;

    public Vector3 removalOffset;

    void Awake() {
        if(prisonPrefab == null) {
            Debug.LogError("Prison Prefab not found");
        }
    }

    public void SpawnPrisonAt(Vector3 location) {
        CreatePrisonGameObject(location);
        AnimatePrisonSpawn(location);
    }

    private void CreatePrisonGameObject(Vector3 location) {
        if(prisonPrefab == null) return;
        prisonGameObject = Instantiate(prisonPrefab, location, Quaternion.identity);
    }

    private async void AnimatePrisonSpawn(Vector3 finalLocation) {
        if(prisonGameObject == null) return;

        Vector3 initialLocation = finalLocation + appearingOffset;

        prisonGameObject.transform.position = initialLocation;

        float lerpAmount = 0;
        float deltaLerpAmount = 0.01f;

        while(lerpAmount < 1) {
            prisonGameObject.transform.position = Vector3.Lerp(initialLocation, finalLocation, lerpAmount);
            lerpAmount += deltaLerpAmount;
            await Task.Delay(10);
        }
    }

    public void RemovePrison() {
        AnimatePrisonRemoval();
    }

    private async void AnimatePrisonRemoval() {
        if(prisonGameObject == null) return;

        Vector3 initialLocation = new Vector3(
            prisonGameObject.transform.position.x,
            prisonGameObject.transform.position.y,
            prisonGameObject.transform.position.z
        );

        Vector3 finalLocation = prisonGameObject.transform.position + removalOffset;

        float lerpAmount = 0;
        float deltaLerpAmount = 0.01f;

        while(lerpAmount < 1) {
            prisonGameObject.transform.position = Vector3.Lerp(initialLocation, finalLocation, lerpAmount);
            lerpAmount += deltaLerpAmount;
            await Task.Delay(10);
        }

        Destroy(prisonGameObject);
    }
}
