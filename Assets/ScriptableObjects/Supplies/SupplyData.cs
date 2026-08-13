using UnityEngine;

[CreateAssetMenu(fileName = "New Supply", menuName = "Supplies/Supply Data")]
public class SupplyData : ScriptableObject
{
    public string itemName;

    public float fillHungry;
    public float fillThirst;
    public float fillHealth;

    private Vector2 position;

    public int maxUses;

    public GameObject prefab;

    public void SetPosition(Vector2 position)
    {
        this.position = position;
    }
}