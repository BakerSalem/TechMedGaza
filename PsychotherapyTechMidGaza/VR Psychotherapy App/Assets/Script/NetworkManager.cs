using BeyondLimitsStudios.VRInteractables;
using SocketIO;
using UnityEngine;
using static JsonClasses;

public class NetworkManager : MonoBehaviour
{
    #region Variables
    public SocketIOComponent SocketIO;
    public static NetworkManager Instance;
    public Transform camTransform;
    public Transform adminCamera;
    public GameObject playerBody;
    public Transform[] applications;
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    public Transform adminLeftHand;
    public Transform adminRightHand;
    public GameObject adminRose;
    public GameObject adminCandle;
    public bool canSync;

    #region State of Rest
    //public PandolManager pandolManager;
    //public CardsControl cardsControl;
    #endregion

    public CarManager carManager;
    public VoiceToggle voiceToggle;
    public ModeToggle ModeToggle;

    private string currentApp = "";
    #endregion

    #region Unity Setup
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        SocketIO.On("cameraUpdate", (response) =>
        {
            //Debug.Log("Received Camera Update: " + response);
            CameraData camData = JsonUtility.FromJson<CameraData>(response.data.ToString());
            ApplyCameraData(camData);
        });

        SocketIO.On("handUpdate", (response) =>
        {
            HandData handData = JsonUtility.FromJson<HandData>(response.data.ToString());
            ApplyHandData(handData);
        });

        SocketIO.On("handObjectToggle", (response) =>
        {
            HandObjectData data = JsonUtility.FromJson<HandObjectData>(response.data.ToString());
            ApplyHandObject(data);
        });

        SocketIO.On("appSelect", (response) =>
        {
            Debug.Log("Application Selected Update: " + response);
            AppJson appData = JsonUtility.FromJson<AppJson>(response.data.ToString());

            currentApp = appData.ID;

            if (currentApp == "Deeb_Breath")
            {
                if (adminRose != null) adminRose.SetActive(true);
                if (adminCandle != null) adminCandle.SetActive(true);
            }
            else
            {
                if (adminRose != null) adminRose.SetActive(false);
                if (adminCandle != null) adminCandle.SetActive(false);
            }

            foreach (Transform app in applications)
            {
                if (app.gameObject.name == appData.ID)
                {
                    app.parent.gameObject.SetActive(true);
                    playerBody.transform.SetPositionAndRotation(app.position, app.rotation);
                }
                else
                {
                    app.parent.gameObject.SetActive(false);
                }
            }
            canSync = true;
        });

        SocketIO.On("objectMove", (response) =>
        {
            MoveData data = JsonUtility.FromJson<MoveData>(response.data.ToString());
            ApplyMoveData(data);
        });

        #region State of Rest

        /*  SocketIO.On("cardFlip", (response) =>
          {
              Debug.Log("Received Card Flip: " + response);
              CardJson cardData = JsonUtility.FromJson<CardJson>(response.data.ToString());
              cardsControl.OnCardSync(cardData.ID);
          });*/

        /* SocketIO.On("flipAllCards", _ =>
         {
             Debug.Log("Received Show All Cards");
             cardsControl.FlipAllCards();
         });*/

        /*SocketIO.On("startPandol", _ =>
        {
            Debug.Log("Received Pandol Start ");
            pandolManager.StartPandol();
        });*/

        /*SocketIO.On("stopPandol", _ =>
        {
            Debug.Log("Received Pandol Stop ");
            pandolManager.StopPandol();
        });*/

        #endregion

        SocketIO.On("carSpeed", (response) =>
        {
            Debug.Log("Received Car Speed " + response);
            CarJson carData = JsonUtility.FromJson<CarJson>(response.data.ToString());
            carManager.OnDataSync(carData);
        });

        SocketIO.On("carStart", _ =>
        {
            Debug.Log("Received Car Start ");
            carManager.StartAnimation();

        });

        SocketIO.On("carStop", _ =>
        {
            Debug.Log("Received Car Stop ");
            carManager.StopAnimation();
        });

        SocketIO.On("carRestart", _ =>
        {
            Debug.Log("Received Car Restart ");
            carManager.RestartAnimation();
        });

        SocketIO.On("startVoice", _ =>
        {
            Debug.Log("Received Start Voice ");
            voiceToggle.StartVoice();
        });

        SocketIO.On("stopVoice", _ =>
        {
            Debug.Log("Received Stop Voice ");
            voiceToggle.StopVoice();
        });

        SocketIO.On("drawShow", (response) =>
        {
            Debug.Log("Received Show Draw Board ");
            ModeToggle.ShowDrawBoard();
        });

        SocketIO.On("writeShow", (response) =>
        {
            Debug.Log("Received Show Write Board");
            ModeToggle.ShowWriteBoard();
        });

        SocketIO.On("engShow", (response) =>
        {
            Debug.Log("Received Show English Keyboard  ");
            ModeToggle.ShowEngKey();
        });

        SocketIO.On("arbShow", (response) =>
        {
            Debug.Log("Received Show Arabic Keyboard ");
            ModeToggle.ShowArbKey();
        });

        SocketIO.On("textUpdate", response =>
        {
            TextData data = JsonUtility.FromJson<TextData>(response.data.ToString());
            ValidateOfTMPTextValue validator = FindAnyObjectByType<ValidateOfTMPTextValue>();

            ValidateOfTMPTextValue[] validators = FindObjectsByType<ValidateOfTMPTextValue>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
            );

            foreach (var v in validators)
            {
                v.ApplyRemoteText(data.Text);
            }
        });

        SocketIO.On("markerUpdate", (response) =>
        {
            DrawJson data = JsonUtility.FromJson<DrawJson>(response.data.ToString());
            ApplyMarkerMovement(data);
        });

        SocketIO.On("eraserUpdate", (response) =>
        {
            DrawJson data = JsonUtility.FromJson<DrawJson>(response.data.ToString());
            ApplyEraserMovement(data);
        });

    }
    private void Update()
    {
        SendCameraData();
        SendHandData();
    }
    #endregion

    #region Send SocketIO
    public void SendCameraData()
    {
        if (canSync)
        {
            CameraData camData = new()
            {
                position = new float[] { camTransform.position.x, camTransform.position.y, camTransform.position.z },
                rotation = new float[] { camTransform.eulerAngles.x, camTransform.eulerAngles.y, camTransform.eulerAngles.z }
            };

            string json = JsonUtility.ToJson(camData);
            SocketIO.Emit("cameraUpdate", new JSONObject(json));
        }
    }
    public void SendHandData()
    {
        if (canSync)
        {
            HandData handData = new()
            {
                leftPosition = new float[] {
                leftHandTransform.position.x,
                leftHandTransform.position.y,
                leftHandTransform.position.z
            },
                leftRotation = new float[] {
                leftHandTransform.eulerAngles.x,
                leftHandTransform.eulerAngles.y,
                leftHandTransform.eulerAngles.z,
            },
                rightPosition = new float[] {
                rightHandTransform.position.x,
                rightHandTransform.position.y,
                rightHandTransform.position.z
            },
                rightRotation = new float[] {
                rightHandTransform.eulerAngles.x,
                rightHandTransform.eulerAngles.y,
                rightHandTransform.eulerAngles.z,
            }
            };

            string json = JsonUtility.ToJson(handData);
            SocketIO.Emit("handUpdate", new JSONObject(json));
        }
    }
    public void SendHandObjectState(string hand, bool isActive)
    {
        if (!canSync) return;

        HandObjectData obj = new()
        {
            hand = hand,
            isActive = isActive
        };

        string json = JsonUtility.ToJson(obj);
        SocketIO.Emit("handObjectToggle", new JSONObject(json));
    }
    public void SendApplication(string appID)
    {

        foreach (Transform app in applications)
        {
            if (app.gameObject.name == appID)
            {
                app.parent.gameObject.SetActive(true);
            }
            else
            {
                app.parent.gameObject.SetActive(false);
            }
        }

        AppJson appData = new AppJson { ID = appID };

        string json = JsonUtility.ToJson(appData);

        SocketIO.Emit("appSelect", new JSONObject(json));
    }
    public void SendText(string text)
    {
        if (!canSync) return;
        TextData payload = new() { Text = text };
        string json = JsonUtility.ToJson(payload);
        SocketIO.Emit("textUpdate", new JSONObject(json));
    }
    public void SendMoveData(string id, Vector3 pos)
    {
        MoveData data = new MoveData
        {
            ID = id,
            position = new float[] { pos.x, pos.y, pos.z }
        };

        string json = JsonUtility.ToJson(data);
        SocketIO.Emit("objectMove", new JSONObject(json));
    }

    #endregion

    #region ApplyControlData
    private void ApplyCameraData(CameraData camData)
    {
        adminCamera.gameObject.SetActive(true);
        adminCamera.transform.position = new Vector3(camData.position[0], camData.position[1], camData.position[2]);
        adminCamera.transform.eulerAngles = new Vector3(camData.rotation[0], camData.rotation[1], camData.rotation[2]); ;
    }
    private void ApplyHandData(HandData data)
    {
        adminLeftHand.transform.position = new Vector3(data.leftPosition[0], data.leftPosition[1], data.leftPosition[2]);
        adminLeftHand.eulerAngles = new Vector3(data.leftRotation[0], data.leftRotation[1], data.leftRotation[2]);

        adminRightHand.transform.position = new Vector3(data.rightPosition[0], data.rightPosition[1], data.rightPosition[2]);
        adminRightHand.eulerAngles = new Vector3(data.rightRotation[0], data.rightRotation[1], data.rightRotation[2]);
    }
    private void ApplyHandObject(HandObjectData data)
    {
        if (data.hand == "right" && adminRose != null)
        {
            adminRose.SetActive(data.isActive);
        }
        else if (data.hand == "left" && adminCandle != null)
        {
            adminCandle.SetActive(data.isActive);
        }
    }
    private void ApplyMarkerMovement(DrawJson data)
    {
        MarkerMovementSync markerSync = FindAnyObjectByType<MarkerMovementSync>();
        if (markerSync == null) return;

        foreach (Transform parent in markerSync.markerParents)
        {
            Marker marker = parent.GetComponentInChildren<Marker>();
            if (marker != null && marker.MarkerId == data.ID)
            {
                parent.position = new Vector3(data.position[0], data.position[1], data.position[2]);
                parent.eulerAngles = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);
                return;
            }
        }

        Debug.LogWarning($"Marker with ID '{data.ID}' not found in markerParents.");
    }
    private void ApplyEraserMovement(DrawJson data)
    {
        EraserMovementSync eraserSync = FindAnyObjectByType<EraserMovementSync>();
        if (eraserSync != null)
            eraserSync.eraserTransform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        eraserSync.eraserTransform.eulerAngles = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);
    }
    private void ApplyMoveData(MoveData data)
    {
        MoveObject[] moveObjects = FindObjectsByType<MoveObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var obj in moveObjects)
        {
            if (obj.objectID == data.ID)
            {
                obj.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
                break;
            }
        }
    }

    #endregion

    #region Send Emit
    public void FlipCardSend(string id)
    {
        CardJson cardData = new CardJson { ID = id };

        string json = JsonUtility.ToJson(cardData);

        SocketIO.Emit("cardFlip", new JSONObject(json));
    }
    public void FlipAllCardsSend()
    {
        SocketIO.Emit("flipAllCards");
    }
    public void CarSpeedSend(CarJson carData)
    {
        string json = JsonUtility.ToJson(carData);

        SocketIO.Emit("carSpeed", new JSONObject(json));
    }
    public void PandolStartSend()
    {
        SocketIO.Emit("startPandol");
    }
    public void PandolStopSend()
    {
        SocketIO.Emit("stopPandol");
    }
    public void CarStartSend()
    {
        SocketIO.Emit("carStart");
    }
    public void CarStopSend()
    {
        SocketIO.Emit("carStop");
    }
    public void CarRestartSend()
    {
        SocketIO.Emit("carRestart");
    }
    public void VoiceStartSend()
    {
        SocketIO.Emit("startVoice");
    }
    public void VoiceStopSend()
    {
        SocketIO.Emit("stopVoice");
    }
    public void DrawShowSend()
    {
        SocketIO.Emit("drawShow");
    }
    public void WriteShowSend()
    {
        SocketIO.Emit("writeShow");
    }
    public void EngShowSend()
    {
        SocketIO.Emit("engShow");
    }
    public void ArbShowSend()
    {
        SocketIO.Emit("arbShow");
    }
    #endregion
}



