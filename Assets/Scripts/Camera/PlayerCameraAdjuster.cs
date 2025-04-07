using UnityEngine;

public class PlayerCameraAdjuster : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float defaultHeight = 1.6f;
    [SerializeField] private float forwardOffset = 0.1f;
    [SerializeField] private float crouchHeightReduction = 0.5f;

    private Transform cameraTransform;
    private float currentHeight;

    void Awake()
    {
        cameraTransform = GetComponent<Transform>();
        currentHeight = defaultHeight;
    }

    void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 newPosition = transform.parent.position + 
                             Vector3.up * currentHeight + 
                             transform.forward * forwardOffset;
        
        cameraTransform.position = newPosition;
    }

    public void SetCrouching(bool isCrouching)
    {
        currentHeight = isCrouching ? 
            defaultHeight - crouchHeightReduction : 
            defaultHeight;
    }
}