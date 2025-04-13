using UnityEngine;
using static JsonClasses;

public class AdminControll : MonoBehaviour
{
    NetworkManager networkManager;
    

    private void Start()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    // use on app btn prefab on click event
    // use on app btn prefab on click event
    // use on app btn prefab on click event
    public void SelectAppClick(string appName)
    {
        networkManager.SendApplication(appName);
    }

    // use on start pandol btn on click event
    public void StartPandol()
    {
        networkManager.SocketIO.Emit("startPandol");
    }

    // use on stop pandol btn on click event21
    public void StopPandol()
    {
        networkManager.SocketIO.Emit("stopPandol");
    }

    // use on flip card btn on click event
    public void FlipCard(GameObject cardObj)
    {
        networkManager.FlipCardSend(cardObj.gameObject.name);
    }

    public void SpeedCar(CarJson carJson)
    {
        networkManager.CarSpeedSend(carJson);
    }
}