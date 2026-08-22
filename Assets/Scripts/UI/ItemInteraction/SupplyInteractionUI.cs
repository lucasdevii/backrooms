using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SupplyInteractionUI : MonoBehaviour
{ 
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI textUi;
    [SerializeField] private Image usable;

    private string itemName;
    
    void OnEnable()
    {
        if (player != null)
            player.OnRaycastItemObject += UpdateUi;
    }

    void OnDisable()
    {
        if (player != null)
            player.OnRaycastItemObject -= UpdateUi;
        
        gameObject.SetActive(false);
    }

    void UpdateUi(GameObject supply)
    {
        if (supply == null)
        {
            HideUI();
            return;
        }

        Supply itemScript = supply.GetComponent<Supply>();

        if (itemScript != null && itemScript.Data != null)
        {
            itemName = itemScript.Data.itemName;
            textUi.text = itemName;
            gameObject.SetActive(true);
            
            Debug.Log("Item: " + itemName); 
        }
        else    
            HideUI();

    }

    void HideUI()
    {
        itemName = "";
        textUi.text = itemName;
        gameObject.SetActive(false);
    }
}
