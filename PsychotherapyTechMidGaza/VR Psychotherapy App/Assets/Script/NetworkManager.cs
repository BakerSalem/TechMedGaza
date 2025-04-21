using SocketIO;
using UnityEngine;
using static JsonClasses;




public class NetworkManager : MonoBehaviour
{
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
    public bool canSync;

    public GameObject adminRose;
    public GameObject adminCandle;

    public PandolManager pandolManager;
    public CardsControl cardsControl;
    public CarManager carManager;
    public VoiceToggle voiceToggle;

    private string currentApp = "";

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

            if (currentApp == "Five Senses")
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

        SocketIO.On("cardFlip", (response) =>
        {
            Debug.Log("Received Card Flip: " + response);
            CardJson cardData = JsonUtility.FromJson<CardJson>(response.data.ToString());
            cardsControl.OnCardSync(cardData.ID);
        });

        SocketIO.On("flipAllCards", _ =>
        {
            Debug.Log("Received Show All Cards");
            cardsControl.FlipAllCards();
        });

        SocketIO.On("startPandol", _ =>
        {
            Debug.Log("Received Pandol Start ");
            pandolManager.StartPandol();
        });

        SocketIO.On("stopPandol", _ =>
        {
            Debug.Log("Received Pandol Stop ");
            pandolManager.StopPandol();
        });

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
    }

    private void Update()
    {
        SendCameraData();
        SendHandData();
    }
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
}



