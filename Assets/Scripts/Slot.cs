using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject item;
    public int id;
    public string type;

    public bool empty;
    public Sprite icon;

    public GameObject HolderCard;
    public GameObject card;


    public Transform slotIconGameObject;

    private void Start()
    {
        slotIconGameObject = transform.GetChild(0);
    }
    public void UpdateSlot()
    {
        slotIconGameObject.GetComponent<Image>().sprite = icon;
    }

    public void ShowPokemon()
    {
        if (empty == false)
        {
            card.SetActive(true);
        }
    }
}