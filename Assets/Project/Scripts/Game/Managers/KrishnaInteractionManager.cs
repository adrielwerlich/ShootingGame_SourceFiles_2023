﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class KrishnaInteractionManager : MonoBehaviour
{
    [SerializeField] private GameObject _templeTreasures;
    [SerializeField] private GameObject _treasureWeaponsPrefab;
    [SerializeField] private GameObject _treasureHealingPrefab;


    [SerializeField] private TextMeshProUGUI userMessage;
    [SerializeField] private GameObject interactionPanel;

    private bool _weaponsCreated = false;
    private bool _healingsCreated = false;

    [SerializeField] private GameObject _scrool; 

    public static event Action<Helper.MantraIndex> PlayMantra;
    public static event Action<Transform> FindTheMantra;


    // Start is called before the first frame update
    void Start()
    {
        //Player.PlayerInArea += IsTempleArea;
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

            if (!Player.HasHealingMantra && !Player.HasWeaponsMantra)
            {
                userMessage.text =
                    "Hello I'm Krishna.\r\n" +
                    "You need to get the mantra scrool.\r\n" +
                    "Follow the arrow to find it";
                FindTheMantra.Invoke(_scrool.transform);
            } 
            else if (treasureCountIsZero())
            {
                userMessage.text = string.Format(
                    $"Shri Vishnu blesses you.\r\n" +
                    "{0}\r\n" +
                    "{1}",
                    getWeaponsLine(),
                    getHealersLine()
                    );
            }
            else 
            {
                userMessage.text = 
                    "Bhagavan says:\r\n" +
                    "Enjoy this great song.\r\n" +
                    "And recharge yourself.";
            } 
        }
    }

    private string getWeaponsLine()
    {
        return "Press 1 to chant the weapons mantra";
    }
    private string getHealersLine()
    {
        return "Press 2 to chant the healing potions mantra";
    }

    private void printUserMessage()
    {
        string weaponsMantra = _weaponsCreated
               ? "All the weapons items are now available for recharge"
               : string.Format("{0}", getWeaponsLine());

        string healingMantra = _healingsCreated
            ? "All the healing items are now available for recharge"
            : string.Format("{0}", getHealersLine());

        userMessage.text =
            $"\r\n{weaponsMantra}" +
            $"\r\n{healingMantra}";
    }

    async void OnTriggerStay()
    {
        if (Input.GetKey(KeyCode.Alpha1) && getWeaponsCount() == 0)
        {
            if (!_weaponsCreated)
            {
                _weaponsCreated = true;
                PlayMantra.Invoke(Helper.MantraIndex.WeaponMantra);
                var prefab = Instantiate(_treasureWeaponsPrefab, _templeTreasures.transform);
                prefab.tag = "WeaponsPackage";
                printUserMessage();
                //userMessage.text = "All the weapons items are now available for recharge";
            }
            await Task.Delay(3000);
        } 
        else if (Input.GetKey(KeyCode.Alpha2) && getHealersCount() == 0)
        {
            if (!_healingsCreated)
            {
                _healingsCreated = true;
                PlayMantra.Invoke(Helper.MantraIndex.HealingMantra);
                var prefab = Instantiate(_treasureHealingPrefab, _templeTreasures.transform);
                prefab.tag = "HealingPackage";
                printUserMessage();
                //userMessage.text = "All the healing items are now available for recharge";
            }

            await Task.Delay(3000);
        }
    }

    private bool treasureCountIsZero()
    {
        return getWeaponsCount() == 0 && getHealersCount() == 0;
    }

    private int getWeaponsCount()
    {
        int weaponsCount = 0;
        List<GameObject> weaponsPackageList = new List<GameObject>();
        foreach (Transform child in _templeTreasures
                    .gameObject
                    .transform)
        {
            if (child.tag == "WeaponsPackage")
            {
                weaponsCount = child.childCount;
                if (weaponsCount == 0)
                {
                    Destroy(child.gameObject);
                    _weaponsCreated = false;
                } 
                else
                {
                    weaponsPackageList.Add(child.gameObject) ;
                }
            }
        }
        if (weaponsPackageList.Count > 1)
        {
            weaponsPackageList.RemoveRange(1, weaponsPackageList.Count);
        }



        return weaponsCount;
    }

    private int getHealersCount()
    {
        int healersCount = 0;
        List<GameObject> healersPackageList = new List<GameObject>();
        foreach (Transform child in _templeTreasures
                    .gameObject
                    .transform)
        {
            if (child.tag == "HealingPackage")
            {
                healersCount = child.childCount;
                if (healersCount == 0)
                {
                    Destroy(child.gameObject);
                    _healingsCreated = false;
                }
                else
                {
                    healersPackageList.Add(child.gameObject);
                }
            }
        }
        if (healersPackageList.Count > 1)
        {
            healersPackageList.RemoveRange(1, healersPackageList.Count);
        }



        return healersCount;
    }

    void OnTriggerExit()
    {
        interactionPanel.gameObject.SetActive(false);
        userMessage.text = "";
    }
}
