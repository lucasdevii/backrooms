using System;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Slider StaminaSlider;
    [SerializeField] private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.OnStaminaChanged += SetStamina;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetStamina(float currentStamina, float maxStamina)
    {
        if(maxStamina != StaminaSlider.maxValue)
        {
            StaminaSlider.maxValue = maxStamina;
        }
        StaminaSlider.value = currentStamina;
    }
}
