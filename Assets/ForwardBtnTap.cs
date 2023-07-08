using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForwardBtnTap : MonoBehaviour
{
    // The EventSystem object.
    private EventSystem eventSystem;

    // The button GameObject.
    private GameObject button;

    // A boolean to track if the button is touched.
    private bool isTouched;

    void Start()
    {
        //eventSystem = GetComponent<EventSystem>();

        //button = GameObject.Find("ForwardBtnTap");

        //button.GetComponent<Button>().onClick.AddListener(OnButtonClicked);

    }

    // Update is called once per frame
    void Update()
    {

        // Check if the button is touched.
        //if (eventSystem.IsPointerOverGameObject(button))
        //{
        //    isTouched = true;
        //}
        //else
        //{
        //    isTouched = false;
        //}
    }

    public void OnButtonTouched()
    {
        if (isTouched)
        {
            Debug.Log("forward btn tapped");
        }
    }

    private void OnButtonClicked()
    {
        // Do something when the button is clicked.
        Debug.Log("forward btn clicked");
    }
}
