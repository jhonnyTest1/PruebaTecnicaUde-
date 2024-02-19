using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    private int allSlots;
    public GameObject[] slot;

    public GameObject slotHolder;
    public int PokeballsFound = 0;

    public TextMeshProUGUI pokeballs;


    // Start is called before the first frame update
    void Start()
    {
        allSlots = slotHolder.transform.childCount;

        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<Slot>().item == null)
            {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Item")
        {
            GameObject itemPickedUp = other.gameObject;

            Item item = itemPickedUp.GetComponent<Item>();

            AddItem(itemPickedUp, item.iD, item.type, item.icon, item.cardPokemon);

            PokeballsFound++;
            pokeballs.text = PokeballsFound + "/12";
        }
    }

    public void AddItem(GameObject itemObject, int ItemId, string itemType, Sprite itemIcon, GameObject card)
    {
        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().empty)
            {
                itemObject.GetComponent<Item>().pickedUp = true;

                slot[i].GetComponent<Slot>().item = itemObject;
                slot[i].GetComponent<Slot>().id = ItemId;


                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().icon = itemIcon;

                slot[i].GetComponent<Slot>().card = card;

                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);

                slot[i].GetComponent<Slot>().UpdateSlot();

                slot[i].GetComponent<Slot>().empty = false;
                return;
            }
        }
    }
}
