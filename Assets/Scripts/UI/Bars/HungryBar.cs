using UnityEngine;
using UnityEngine.UI;

public class HungryBar : MonoBehaviour
{
    [SerializeField] private Slider HungrySlider;
    [SerializeField] private Player player;

    public void Start()
    {
        HungrySlider.maxValue = player.maxHungry;
        HungrySlider.value = player.currentHungry;

        player.OnHungryChanged += SetHungry;
    }

    public void Update()
    {
        
    }

    void SetHungry(float currentHungry, float maxHungry)
    {
        if(maxHungry != HungrySlider.maxValue)
        {
            HungrySlider.maxValue = maxHungry;
        }
        HungrySlider.value = currentHungry;
    }
}