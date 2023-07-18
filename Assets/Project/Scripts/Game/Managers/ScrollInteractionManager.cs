using UnityEngine;
using TMPro;
using System;
using System.Threading.Tasks;

public class ScrollInteractionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userMessage;
    [SerializeField] private GameObject interactionPanel;
    
    public static event Action<Helper.MantraIndex> PlayMantra;
    public static event Action HideDirectionArrow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            interactionPanel.gameObject.SetActive(true);
            printUserMessage();
        }
    }

    private void printUserMessage()
    {
        string weaponsMantra = !Player.HasWeaponsMantra 
               ? "Press 1 to learn the weapons recharge mantra"
               : "You already have the weapons mantra";

        string healingMantra = !Player.HasHealingMantra
            ? "Press 2 to learn the healing recharge mantra"
            : "You already have the healing mantra";

        userMessage.text = $"This scroll has the sacred mantra" +
            $"\r\n{weaponsMantra}" +
            $"\r\n{healingMantra}";
    }

    async void OnTriggerStay()
    {
        if (Input.GetKey(KeyCode.Alpha1) && !Player.HasWeaponsMantra) { 
            PlayMantra.Invoke(Helper.MantraIndex.WeaponMantra);
            Player.HasWeaponsMantra = true;
            await Task.Delay(3000);
        }
        else if (Input.GetKey(KeyCode.Alpha2) && !Player.HasHealingMantra) {
            PlayMantra.Invoke(Helper.MantraIndex.HealingMantra);
            Player.HasHealingMantra = true;
            await Task.Delay(3000);
        } 
        if (Player.HasWeaponsMantra && Player.HasHealingMantra)
        {
            HideDirectionArrow.Invoke();
            this.gameObject.SetActive(false);
            closePanel();
        }

        printUserMessage();
    }

    void OnTriggerExit()
    {
        closePanel();
    }

    private void closePanel()
    {
        interactionPanel.gameObject.SetActive(false);
        userMessage.text = "";
    }
}
