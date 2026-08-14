using UnityEngine;

[CreateAssetMenu(fileName = "New Supply", menuName = "Supplies/Supply Data")]
public class SupplyData : ScriptableObject
{
    public string itemName;

    public float fillHungry;
    public float fillThirst;
    public float fillHealth;

    public Vector3 position;

    public int maxUses;

    public GameObject prefab;

    public void SetPosition(Vector3 position)
    {
        this.position = position;
    }
}