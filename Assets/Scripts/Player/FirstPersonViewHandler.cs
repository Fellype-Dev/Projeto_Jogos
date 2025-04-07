using UnityEngine;

public class FirstPersonViewHandler : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Renderer fullBodyRenderer;
    [SerializeField] private GameObject neckBlocker;

    [Header("Configurações")]
    [SerializeField] private float cameraForwardOffset = 0.2f;
    [SerializeField] private Material firstPersonMaterial;

    private Vector3 originalCameraPosition;

    void Start()
    {
        originalCameraPosition = playerCamera.localPosition;
        InitializeFirstPersonView();
    }

    public void ToggleFirstPersonView(bool isFirstPerson)
    {
        if (isFirstPerson)
        {
            EnableFirstPersonView();
        }
        else
        {
            DisableFirstPersonView();
        }
    }

    private void InitializeFirstPersonView()
    {
        // Ajusta posição da câmera
        playerCamera.localPosition += new Vector3(0, 0, cameraForwardOffset);

        // Configura neck blocker
        if (neckBlocker != null)
        {
            neckBlocker.SetActive(true);
        }
    }

    private void EnableFirstPersonView()
    {
        if (fullBodyRenderer != null)
        {
            fullBodyRenderer.enabled = false;
        }

        if (neckBlocker != null)
        {
            neckBlocker.SetActive(true);
        }
    }

    private void DisableFirstPersonView()
    {
        if (fullBodyRenderer != null)
        {
            fullBodyRenderer.enabled = true;
        }

        if (neckBlocker != null)
        {
            neckBlocker.SetActive(false);
        }
    }
}