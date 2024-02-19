using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int iD;
    public string type;
    public Sprite icon;
    public GameObject cardPokemon;
    [HideInInspector]
    public bool pickedUp;


}
