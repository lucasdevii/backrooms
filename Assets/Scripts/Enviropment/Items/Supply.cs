using System.Globalization;
using UnityEngine;

public class Supply : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    public float fillHungry = 0;
    public float fillThirst = 0;
    public float fillHealth = 0;
    
    public int quantityOfUses = 0;
    public int maxUses = 1;
    public Vector3 position;
    public string itemName;

    public Supply(SupplyData supplyData)
    {
        fillHealth = supplyData.fillHealth;
        fillHungry = supplyData.fillHungry;
        fillThirst = supplyData.fillThirst;

        maxUses = supplyData.maxUses;
        position = supplyData.position;
        itemName = supplyData.itemName;
    }

    public void Use()
    {
        Player.Instance.AddHealth(fillHealth);
        Player.Instance.AddSatiety(fillHungry);
        Player.Instance.AddHydratation(fillThirst);

        quantityOfUses++;

        if(quantityOfUses >= maxUses)
            Destroy(gameObject);
    }
}