using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("FirstPerson")]
    public float firstPersonSens = 400.0f;
    public float firstPersonYOffset = 1.0f;
    public float firstPersonFollowSpeed = 10.0f;
    public float firstPersonVerticalLimit = 45.0f;
    public float firstPersonRotationSpeed = 10.0f;
    public Transform firstPersonCameraPosition;

    private Transform target;
    private float rotX, rotY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        CameraRotate();
        FirstPersonCameraTargetRotate();
    }

    private void LateUpdate()
    {
        Follow();
    }

    void CameraRotate()
    {
        rotX -= Input.GetAxis("Mouse Y") * firstPersonSens * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * firstPersonSens * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -firstPersonVerticalLimit, firstPersonVerticalLimit);
        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
    }

    void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + target.up * firstPersonYOffset, firstPersonFollowSpeed * Time.deltaTime);
    }

    void FirstPersonCameraTargetRotate()
    {
        target.rotation = Quaternion.Lerp(target.rotation, Quaternion.Euler(0, rotY, 0), firstPersonRotationSpeed * Time.deltaTime);
    }
}
