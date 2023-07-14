using TMPro;
using UnityEngine;

public class KrishnaInteractionManager : MonoBehaviour
{
    private bool _playerInTemple;

    [SerializeField] private TextMeshProUGUI userMessage;
    [SerializeField] private GameObject interactionPanel;

    // Start is called before the first frame update
    void Start()
    {
        Player.PlayerInArea += IsTempleArea;

    }

    private void IsTempleArea(string area)
    {
        if (area == "KrishnaTemple")
        {
            _playerInTemple = true;

        }
        else if (area == "OutsideKrishnaTemple")
        {
            _playerInTemple = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            interactionPanel.gameObject.SetActive(true);
            userMessage.text =
                "Hello\r\nPlease press G to chant the secret mantra";
        }
    }

    void OnTriggerStay()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("interaction");
        }
    }

    void OnTriggerExit()
    {
        interactionPanel.gameObject.SetActive(false);
        userMessage.text = "";
    }
}
