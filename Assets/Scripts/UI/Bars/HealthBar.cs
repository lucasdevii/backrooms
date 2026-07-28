using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private Player player;

    public void Start()
    {
        HealthSlider.maxValue = player.maxHealth;
        HealthSlider.value = player.currentHealth;

        player.OnHealthChanged += SetHealth;
    }

    public void Update()
    {
        
    }

    void SetHealth(float currentHealth, float maxHealth)
    {
        if(maxHealth != HealthSlider.maxValue)
        {
            HealthSlider.maxValue = maxHealth;
        }
        HealthSlider.value = currentHealth;
    }
}
