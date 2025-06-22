using UnityEngine;
using UnityEngine.EventSystems;

public class MoveObject : MonoBehaviour
{
    public string objectID;
    public float baseMoveAmount = 0.1f;
    public float acceleration = 0.05f;
    public float maxMoveAmount = 1.0f;

    private float currentMoveAmount;
    private Coroutine moveCoroutine;

    private void Start()
    {
        currentMoveAmount = baseMoveAmount;
    }

    public void OnMoveButtonDown(string direction)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveContinuously(direction));
    }

    public void OnMoveButtonUp()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        currentMoveAmount = baseMoveAmount;
    }

    private System.Collections.IEnumerator MoveContinuously(string direction)
    {
        while (true)
        {
            MoveInDirection(direction);

            // Increase speed gradually while held
            currentMoveAmount = Mathf.Min(currentMoveAmount + acceleration * Time.deltaTime, maxMoveAmount);

            yield return null;
        }
    }

    private void MoveInDirection(string direction)
    {
        Vector3 moveVector = Vector3.zero;

        switch (direction)
        {
            case "forward":
                moveVector = new Vector3(0, 0, currentMoveAmount);
                break;
            case "backward":
                moveVector = new Vector3(0, 0, -currentMoveAmount);
                break;
            case "up":
                moveVector = new Vector3(0, currentMoveAmount, 0);
                break;
            case "down":
                moveVector = new Vector3(0, -currentMoveAmount, 0);
                break;
            case "left":
                moveVector = new Vector3(-currentMoveAmount, 0, 0);
                break;
            case "right":
                moveVector = new Vector3(currentMoveAmount, 0, 0);
                break;
        }

        transform.position += moveVector;
        NetworkManager.Instance.SendMoveData(objectID, transform.position);
    }
}
