using UnityEngine;

public class Supply : MonoBehaviour
{
    [SerializeField] private SupplyData data;
    [SerializeField] private int quantityOfUses;

    public SupplyData Data => data;

    public void Initialize(SupplyData supplyData)
    {
        data = supplyData;

        if (data != null)
        {
            gameObject.name = data.itemName;
            transform.position = data.position;
        }
    }

    public void Use()
    {
        if (data == null)
            return;

        Player.Instance.AddHealth(data.fillHealth);
        Player.Instance.AddSatiety(data.fillHungry);
        Player.Instance.AddHydratation(data.fillThirst);

        quantityOfUses++;

        if (quantityOfUses >= data.maxUses)
            Destroy(gameObject);
    }
}