using UnityEngine;
using static JsonClasses;

public class CarManager : MonoBehaviour
{
    public AdminControll adminControll;
    public Animator animator;
    public GameObject speedPanel;
    public ParticleSystem celebration;

    public CarJson carJson = new();
    public Transform restPos;

    private Vector3 initialPosition;
    private bool animationFinished = false;

    // use on anim buttons in list of car speed

    void Start()
    {
        initialPosition = restPos.position;
        animationFinished = false;
    }
    void Update()
    {
        if (animator.enabled && !animationFinished)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName("Finish") && state.normalizedTime >= 1f)
            {
                animationFinished = true;
                OnFinishAnimation();
            }
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
        animationFinished = false;
    }
    public void OnFinishAnimation()
    {
        celebration.Play(true);
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
        animationFinished = false;
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


}
