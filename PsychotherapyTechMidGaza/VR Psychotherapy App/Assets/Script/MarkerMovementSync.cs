using UnityEngine;
using System.Collections.Generic;
using BeyondLimitsStudios.VRInteractables;
using static JsonClasses;

public class MarkerMovementSync : MonoBehaviour
{
    public List<Transform> markerParents = new List<Transform>();
    private Dictionary<string, Vector3> lastPositions = new();
    private Dictionary<string, Quaternion> lastRotations = new();

    private float syncThreshold = 0.001f;

    private void Start()
    {
        Marker[] allMarkers = FindObjectsByType<Marker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Marker marker in allMarkers)
        {
            if (!markerParents.Contains(marker.transform.parent))
            {
                markerParents.Add(marker.transform.parent);
            }
        }
    }
    private void Update()
    {
        foreach (Transform parent in markerParents)
        {
            Marker marker = parent.GetComponentInChildren<Marker>();
            if (marker == null) continue;

            string id = marker.MarkerId;
            parent.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);
            if (!lastPositions.ContainsKey(id))
            {
                lastPositions[id] = pos;
                lastRotations[id] = rot;
                continue;
            }

            if (Vector3.Distance(lastPositions[id], pos) > syncThreshold || Quaternion.Angle(lastRotations[id], rot) > syncThreshold)
            {
                lastPositions[id] = pos;
                lastRotations[id] = rot;

                SendMarkerMovement(id, pos, rot);
            }
        }
    }

    private void SendMarkerMovement(string id, Vector3 pos, Quaternion rot)
    {
        if (!NetworkManager.Instance.canSync) return;

        DrawJson drawData = new()
        {
            ID = id,
            position = new float[] { pos.x, pos.y, pos.z },
            rotation = new float[] { rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z }
        };

        string json = JsonUtility.ToJson(drawData);
        NetworkManager.Instance.SocketIO.Emit("markerUpdate", new JSONObject(json));
    }
}
