using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameraObject; 

    [SerializeField] private float bobSpeed = 5f;
    [SerializeField] private float bobAmountY = 0.03f;
    [SerializeField] private float bobAmountX = 1f;


    public float sensitivityX = 0.5f;
    public float sensitivityY = 0.5f;
    private float xRotation;
    private float yRotation;

    private Vector3 initialCameraLocalPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetCameraPosition();
        initialCameraLocalPosition = cameraObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraRotation();
        UpdateHeadBob();
    }

    void SetCameraPosition()
    {
        initialCameraLocalPosition = cameraObject.transform.localPosition;
        cameraObject.transform.rotation = transform.rotation;
    }

    public void UpdateCameraRotation()
    {
        //Significa quantos pixels o mouse se moveu dês do ultimo frame
        Vector2 delta = Mouse.current.delta.ReadValue();

        xRotation -= delta.y * sensitivityY;
        yRotation += delta.x * sensitivityX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        SetRotationInTheCamera();
    }

    void SetRotationInTheCamera()
    {
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        player.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void UpdateHeadBob()
    {        
        float phase = Time.time * bobSpeed;

        float bobX = Player.Instance.isWalking  ?  Mathf.Sin(phase) * bobAmountX  :  0;
        float bobY = Mathf.Abs(Mathf.Sin(phase)) * bobAmountY ;

        cameraObject.transform.localPosition =
            initialCameraLocalPosition +
            Vector3.right * bobX +
            Vector3.up * bobY;
    }
}

