using System.Globalization;
using UnityEngine;

public class Supply : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    SupplyData data = new SupplyData();

    int quantityOfUses = 0;

    public Supply(SupplyData supplyData)
    {
        data = supplyData;
    }

    public void Use()
    {
        Player.Instance.AddHealth(data.fillHealth);
        Player.Instance.AddSatiety(data.fillHungry);
        Player.Instance.AddHydratation(data.fillThirst);

        quantityOfUses++;

        if(quantityOfUses >= data.maxUses)
            Destroy(gameObject);
    }
}