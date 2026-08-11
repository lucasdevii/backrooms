public class Supply
{
    protected float fillHungry = 0;
    protected float fillThirst = 0;
    protected float fillHealth = 0;
    
    protected int quantityOfUses = 0;
    protected int maxUses = 1;

    protected Use()
    {
        player.AddHealth(fillHealth);
        player.AddSatiety(fillHungry);
        player.AddHydratation(fillThirst);

        quantityOfUses++;
    }
}