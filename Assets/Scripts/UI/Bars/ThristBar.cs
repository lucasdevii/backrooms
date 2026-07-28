using UnityEngine;
using UnityEngine.UI;

public class ThristBar : MonoBehaviour
{
    [SerializeField] private Slider ThristSlider;
    [SerializeField] private Player player;

    public void Start()
    {        
        ThristSlider.maxValue = player.maxThirst;
        ThristSlider.value = player.currentThirst;

        player.OnThristChanged += SetThrist;
    }

    public void Update()
    {
        
    }

    void SetThrist(float currentThrist, float maxThrist)
    {
        if(maxThrist != ThristSlider.maxValue)
        {
            ThristSlider.maxValue = maxThrist;
        }
        ThristSlider.value = currentThrist;
    }
}