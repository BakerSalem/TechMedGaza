using UnityEngine;

public class HandSpawnBlock : MonoBehaviour
{
    public GameObject blockPrefab;
    public OVRHand DetectHand;
    public Transform handHoldPoint;
    public string handType = "left";

    private GameObject heldObject = null;
    private bool lastPinchState = false;

    void Update()
    {
        if (DetectHand == null || handHoldPoint == null)
            return;

        bool isPinching = DetectHand.GetFingerIsPinching(OVRHand.HandFinger.Index) ||
                          DetectHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) ||
                          DetectHand.GetFingerIsPinching(OVRHand.HandFinger.Ring);

        // Only do something when the pinch state changes
        if (isPinching != lastPinchState)
        {
            if (isPinching)
            {
                heldObject = Instantiate(blockPrefab, handHoldPoint.position, handHoldPoint.rotation);
                heldObject.transform.SetParent(handHoldPoint);
                heldObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                NetworkManager.Instance?.SendHandObjectState(handType, true);
            }
            else
            {
                if (heldObject != null)
                {
                    Destroy(heldObject);
                    heldObject = null;
                }

                NetworkManager.Instance?.SendHandObjectState(handType, false);
            }

            lastPinchState = isPinching;
        }
    }
}
