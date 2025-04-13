using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float speed = 5.0f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A, D or Left, Right
        float vertical = Input.GetAxis("Vertical");   // W, S or Up, Down

        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }
}

