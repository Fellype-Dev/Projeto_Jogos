using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float mouseSensitivity;
    public float verticalClamp = 80.0f;
    public float cameraHeight = 2.5f;
    public float smoothFollow = 5f; // Suavização do movimento

    private Transform playerBody;
    private CharacterController playerCC;
    private float xRotation = 0f;
    private Vector3 targetPosition;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = GameObject.FindGameObjectWithTag("Player").transform;
        playerCC = playerBody.GetComponent<CharacterController>();

        mouseSensitivity = SettingsManager.Instance.mouseSensitivity;


        transform.localPosition = new Vector3(0, cameraHeight, 0);
        transform.localRotation = Quaternion.identity;
    }

    void Update()
    {

        mouseSensitivity = SettingsManager.Instance.mouseSensitivity;
        HandleCameraRotation();
    }

    void LateUpdate()
    {
        // Suaviza o acompanhamento do pulo
        targetPosition = playerBody.position + Vector3.up * cameraHeight;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothFollow * Time.deltaTime);
    }

    void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}