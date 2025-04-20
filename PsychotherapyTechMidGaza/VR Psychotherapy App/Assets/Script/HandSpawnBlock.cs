/*using UnityEngine;

public class HandSpawnBlock : MonoBehaviour
{
    public GameObject blockPrefab;
    public OVRHand DetectHand;
    public Transform handHoldPoint; // Reference to the empty GameObject on the hand
    private GameObject heldObject = null;

    public string handType = "left"; // "left" or "right"
    private bool lastPinchState = false;

    void Update()
    {
        if (DetectHand == null || handHoldPoint == null)
            return;

        bool isPinching = DetectHand.GetFingerIsPinching(OVRHand.HandFinger.Middle);

        if (isPinching)
        {
            Debug.Log(isPinching);
            if (isPinching && !lastPinchState)
            {
                Debug.Log("hold object is null");
                heldObject = Instantiate(blockPrefab, handHoldPoint.position, handHoldPoint.rotation);
                heldObject.transform.SetParent(handHoldPoint);
                heldObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                NetworkManager.Instance?.SendHandObjectState(handType, true);
            }
        }
        else if (!isPinching && lastPinchState)
        {
            Debug.Log("hold object is not null");

            Destroy(heldObject);
            heldObject = null;
            NetworkManager.Instance?.SendHandObjectState(handType, false);
        }
        lastPinchState = isPinching;
    }
}
*/
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

        bool isPinching = DetectHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

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
