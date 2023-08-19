using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PrisonAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject prisonPrefab;

    public Vector3 appearingOffset;

    public Vector3 removalOffset;

    private Dictionary<Pawn, GameObject> prisonGameObjects;

    void OnEnable() {
        CustomEvents.OnPawnImprisoned += SpawnPrisonAt;
        CustomEvents.OnPawnFreedFromPrison += RemovePrison;
    }

    void OnDisable() {
        CustomEvents.OnPawnImprisoned -= SpawnPrisonAt;
        CustomEvents.OnPawnFreedFromPrison -= RemovePrison;
    }

    void Awake() {
        if(prisonPrefab == null) {
            Debug.LogError("Prison Prefab not found");
        }

        prisonGameObjects = new Dictionary<Pawn, GameObject>();
    }

    private void SpawnPrisonAt(Pawn pawn) {
        if(prisonGameObjects.ContainsKey(pawn)) return;

        Vector3 location = pawn.transform.position;
        prisonGameObjects.Add(pawn, CreatePrisonGameObject(location));
        AnimatePrisonSpawn(prisonGameObjects[pawn], location);
    }

    private GameObject CreatePrisonGameObject(Vector3 location) {
        if(prisonPrefab == null) return null;
        return Instantiate(prisonPrefab, location, Quaternion.identity);
    }

    private async void AnimatePrisonSpawn(GameObject prisonGameObject, Vector3 finalLocation) {
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

    private void RemovePrison(Pawn pawn) {
        if(!prisonGameObjects.ContainsKey(pawn)) return;
        AnimatePrisonRemoval(prisonGameObjects[pawn]);
    }

    private async void AnimatePrisonRemoval(GameObject prisonGameObject) {
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
