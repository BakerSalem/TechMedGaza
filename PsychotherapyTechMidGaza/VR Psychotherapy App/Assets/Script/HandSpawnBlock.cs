using UnityEngine;

public class HandSpawnBlock : MonoBehaviour
{
    public GameObject blockPrefab;
    public OVRHand DetectHand;
    public Transform handHoldPoint; // Reference to the empty GameObject on the hand

    private GameObject heldObject = null;

    void Update()
    {
        if (DetectHand == null || handHoldPoint == null)
            return;

        bool isPinching = DetectHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (isPinching)
        {
            if (heldObject == null)
            {
                // Instantiate at the palm (handHoldPoint) position
                heldObject = Instantiate(blockPrefab, handHoldPoint.position, handHoldPoint.rotation);

                // Parent to the hand so it follows correctly
                heldObject.transform.SetParent(handHoldPoint);

                // Optional: Reset local position if needed
                heldObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
        }
        else
        {
            if (heldObject != null)
            {
                Destroy(heldObject);
                heldObject = null;
            }
        }
    }
}
