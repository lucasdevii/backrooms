using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameraObject; 

    public float sensitivityX = 0.5f;
    public float sensitivityY = 0.5f;
    private float xRotation;
    private float yRotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraRotation();
    }

    void SetCameraPosition()
    {
        cameraObject.transform.position = transform.position;
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
}

