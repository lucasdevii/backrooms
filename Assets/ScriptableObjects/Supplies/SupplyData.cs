using UnityEngine;

[CreateAssetMenu(fileName = "New Supply", menuName = "Supplies/Supply Data")]
public class SupplyData : ScriptableObject
{
    public string itemName;

    public float fillHungry;
    public float fillThirst;
    public float fillHealth;

    public int maxUses;

    public GameObject prefab;
}