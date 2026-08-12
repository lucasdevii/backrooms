using UnityEngine;

public class Supply : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    protected float fillHungry = 0;
    protected float fillThirst = 0;
    protected float fillHealth = 0;
    
    protected int quantityOfUses = 0;
    protected int maxUses = 1;

    protected void SetValues(float hungry, float thirst, float health)
    {
        fillHungry = hungry;
        fillThirst = thirst;
        fillHealth = health;
    }

    protected void Use()
    {
        Player.Instance.AddHealth(fillHealth);
        Player.Instance.AddSatiety(fillHungry);
        Player.Instance.AddHydratation(fillThirst);

        quantityOfUses++;

        if(quantityOfUses >= maxUses)
            Destroy(gameObject);
    }
}