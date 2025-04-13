using SocketIO;
using UnityEngine;
using static JsonClasses;

public class NetworkManager : MonoBehaviour
{
    public SocketIOComponent SocketIO;
    public Transform camTransform;
    public Transform adminCamera;
    public GameObject playerBody;
    public Transform[] applications;
    public bool canSync;

    public PandolManager pandolManager;
    public CardsControl cardsControl;
    public CarManager carManager;


    private void Start()
    {
        SocketIO.On("cameraUpdate", (response) =>
        {
            //Debug.Log("Received Camera Update: " + response);
            CameraData camData = JsonUtility.FromJson<CameraData>(response.data.ToString());
            ApplyCameraData(camData);
        });

        SocketIO.On("appSelect", (response) =>
        {

            Debug.Log("Application Selected Update: " + response);
            AppJson appData = JsonUtility.FromJson<AppJson>(response.data.ToString());

            foreach (Transform app in applications)
            {
                if (app.gameObject.name == appData.ID)
                {
                    app.parent.gameObject.SetActive(true);
                    playerBody.transform.position = app.position;
                    playerBody.transform.rotation = app.rotation;
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
    }

    private void Update()
    {
        SendCameraData();
    }
    public void SendCameraData()
    {
        if (canSync)
        {
            CameraData camData = new CameraData
            {
                position = new float[] { camTransform.position.x, camTransform.position.y, camTransform.position.z },
                rotation = new float[] { camTransform.eulerAngles.x, camTransform.eulerAngles.y, camTransform.eulerAngles.z }
            };

            string json = JsonUtility.ToJson(camData);


            SocketIO.Emit("cameraUpdate", new JSONObject(json));
        }
    }

    private void ApplyCameraData(CameraData camData)
    {
        adminCamera.gameObject.SetActive(true);
        adminCamera.transform.position = new Vector3(camData.position[0], camData.position[1], camData.position[2]);
        adminCamera.transform.eulerAngles = new Vector3(camData.rotation[0], camData.rotation[1], camData.rotation[2]); ;
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

    
}



