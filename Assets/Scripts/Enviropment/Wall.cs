using UnityEngine;

public class Wall : MonoBehaviour
{
    Vector2 size = new Vector3();
    Vector2 Position = new Vector3();
    static public float thickness = 0.25f;

    public void SetPosition(Vector2 position)
    {
        Position = position;
        transform.position = new Vector3(Position.x, transform.position.y, Position.y);
    }

    public void SetSize(float width, float height)
    {

        size = new Vector2(width, height);
        transform.localScale = new Vector3(size.x, size.y, thickness);
    }

    public void SetRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void Inicialize(Vector2 position, float width, float height, float angle)
    {
        SetPosition(position);
        SetSize(width, height);
        SetRotation(angle);
    }
}
