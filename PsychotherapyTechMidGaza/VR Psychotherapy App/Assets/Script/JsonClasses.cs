using System;

public class JsonClasses
{
    [Serializable]
    public class CameraData
    {
        public float[] position;
        public float[] rotation;
    }

    [System.Serializable]
    public class HandData
    {
        public float[] leftPosition;
        public float[] leftRotation;
        public float[] rightPosition;
        public float[] rightRotation;
    }

    [Serializable]
    public class AppJson
    {
        public string ID;

        public AppJson() { }
    }

    [Serializable]
    public class CardJson
    {
        public string ID;

        public CardJson() { }
    }

    [Serializable]
    public class CarJson
    {
        public string ID;
        public int speed;
        
        public CarJson() { }

        public CarJson(string ID, int speed)
        {
        }
    }

}
