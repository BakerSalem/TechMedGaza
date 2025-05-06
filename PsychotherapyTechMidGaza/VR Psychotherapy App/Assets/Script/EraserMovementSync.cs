using UnityEngine;
using SocketIO;
using static JsonClasses;

public class EraserMovementSync : MonoBehaviour
{
    public string ID = "Eraser"; 
    public Transform eraserTransform;

    private void Update()
    {
        if (NetworkManager.Instance.canSync)
        {
            SendEraserMovement();
        }
    }

    public void SendEraserMovement()
    {
        DrawJson data = new()
        {
            ID = ID,
            position = new float[]
            {
                eraserTransform.position.x,
                eraserTransform.position.y,
                eraserTransform.position.z
            },
            rotation = new float[]
            {
                eraserTransform.eulerAngles.x,
                eraserTransform.eulerAngles.y,
                eraserTransform.eulerAngles.z
            }
        };

        string json = JsonUtility.ToJson(data);
        NetworkManager.Instance.SocketIO.Emit("eraserUpdate", new JSONObject(json));
    }
}
