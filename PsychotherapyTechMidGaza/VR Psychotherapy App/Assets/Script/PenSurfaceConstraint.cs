using UnityEngine;
using Oculus.Interaction;

[RequireComponent(typeof(Transform))]
public class PenSurfaceConstraint : MonoBehaviour
{
    [SerializeField]
    private PokeInteractor _pokeInteractor;

    [Tooltip("Slight offset so the pen tip hovers just above the surface.")]
    [SerializeField]
    private float surfaceOffset = 0.001f;

    [Tooltip("How far you need to pull away (along the normal) to break contact.")]
    [SerializeField]
    private float releaseThreshold = 0.002f;

    void LateUpdate()
    {
        if (_pokeInteractor == null || _pokeInteractor.Interactable == null)
        {
            return;
        }

        Vector3 desiredPos = transform.position;
        Vector3 surfacePoint = _pokeInteractor.TouchPoint;
        Vector3 surfaceNormal = _pokeInteractor.TouchNormal;
        Vector3 delta = desiredPos - surfacePoint;

        float depth = Vector3.Dot(delta, surfaceNormal);

        if (depth > releaseThreshold)
        {
            return;
        }

        Vector3 tangent = Vector3.ProjectOnPlane(delta, surfaceNormal);
        Vector3 constrainedPos = surfacePoint
                                + tangent
                                + surfaceNormal * surfaceOffset;

        transform.position = constrainedPos;
    }
}
