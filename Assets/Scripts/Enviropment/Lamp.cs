using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    Vector3 Position = new Vector3();
    [SerializeField] private GameObject LightObject;
    private Light lightComponent;

    void Awake()
    {
        lightComponent = LightObject.GetComponent<Light>();
    }
    public void Inicialize(Vector3 position, bool lightIsActive, GameObject chunk, float angle = 0)
    {
        SetPosition(position);
        if(angle != 0) SetRotation(angle);
        SetLight(lightIsActive);
        transform.SetParent(chunk.transform);
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
        transform.position = new Vector3(Position.x, Position.y, Position.z);
    }
    public void SetRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void SetLight(bool value)
    {
        LightObject.SetActive(value);
    }

    public void SetShadow(bool value)
    {
        lightComponent.shadows = value ? LightShadows.Soft : LightShadows.None;
    }

}