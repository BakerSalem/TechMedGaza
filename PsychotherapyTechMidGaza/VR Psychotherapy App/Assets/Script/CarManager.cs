using UnityEngine;
using static JsonClasses;

public class CarManager : MonoBehaviour
{
    public AdminControll adminControll;
    public Animator animator;
    public GameObject speedPanel;
    public CarJson carJson = new();
    public Transform restPos;
    public ParticleSystem celebrationEffect;
    private Vector3 initialPosition;

    // use on anim buttons in list of car speed

    void Start()
    {
        initialPosition = restPos.position;
    }
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("Finish"))
        {
            TriggerCelebration();
        }
    }
    public void SetName(string name)
    {
        carJson.ID = name;
    }
    public void SetSpeed(string speed)
    {
        carJson.speed = int.Parse(speed);
    }

    public void SaveData()
    {
        adminControll.SpeedCar(carJson);
    }

    public void StartAnimation()
    {
        animator.enabled = true;
    }

    public void StopAnimation()
    {
        animator.enabled = false;
    }

    public void RestartAnimation()
    {
        animator.enabled = false;
        ResetPosition();
        animator.Rebind();
        animator.Update(0);

        animator.SetInteger("Start", 1);

    }

    public void OnDataSync(CarJson carJson)
    {
        //int speedValue = carJson.speed;

        animator.SetFloat(carJson.ID, 0.9f / carJson.speed);


        //animator.speed = Mathf.Clamp(speedValue / 10f, 0.1f, 5f);
    }

    public void ResetPosition()
    {
        if (restPos != null)
        {
            transform.position = restPos.position;
        }
        else
        {
            Debug.LogWarning("restPos is not assigned! Using initial position.");
            transform.position = initialPosition;
        }
    }

    private void TriggerCelebration()
    {
        if (celebrationEffect != null && !celebrationEffect.isPlaying)
        {
            celebrationEffect.Play();
        }
    }
}
