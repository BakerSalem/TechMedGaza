using UnityEngine;
using static JsonClasses;

// use this script Cards objcet
public class CardManager : MonoBehaviour
{
    public CardJson cardData = new CardJson();
    public bool onFace = true;
    public MeshRenderer meshRenderer;

    [SerializeField] Material whiteColor;
    [SerializeField] Material changeColor;

    private void OnEnable()
    {
        cardData.ID = gameObject.name;
        FlipCard();
    }
    public void FlipCard()
    {
        if (onFace)
        {
            onFace = false;
            Flip(-90, changeColor);
        }
        else
        {
            onFace = true;
            Flip(90, whiteColor);
        }
    }
   
    void Flip(float yAxis, Material mat)
    {
        gameObject.transform.eulerAngles = new Vector3(-90, yAxis, 0);
        Material[] mats = meshRenderer.materials; // Get the materials array
        mats[0] = mat; // Change the first material
        meshRenderer.materials = mats; // Apply the modified materials array back
    }
}
