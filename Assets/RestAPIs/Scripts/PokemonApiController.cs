using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;

public class PokemonApiController : MonoBehaviour
{
    public RawImage pokemonRawImage;
    public TextMeshProUGUI pokemonName;
    public TextMeshProUGUI pokemonNumber;
    public TextMeshProUGUI[] pokeTypeTextArray;
    public TextMeshProUGUI[] pokemonStats;

    private readonly string pokemonApiURL = "https://pokeapi.co/api/v2/";
    void Start()
    {
        pokemonRawImage.texture = Texture2D.blackTexture;
        pokemonName.text = "";
        pokemonNumber.text = "";

        foreach (TextMeshProUGUI pokemonTypes in pokeTypeTextArray)
        {
            pokemonTypes.text = "";
        }

        foreach (TextMeshProUGUI pokemonStatsNum in pokemonStats)
        {
            pokemonStatsNum.text = "";
        }

        CreatePokemon();
    }

    public void CreatePokemon()
    {
        int pokemonRandomIndex = Random.Range(1, 808);

        pokemonRawImage.texture = Texture2D.blackTexture;

        pokemonName.text = "Loading...";
        pokemonNumber.text = "# " + pokemonRandomIndex;
        foreach (TextMeshProUGUI pokemonTypes in pokeTypeTextArray)
        {
            pokemonTypes.text = "";
        }

        foreach (TextMeshProUGUI pokemonStatsNum in pokemonStats)
        {
            pokemonStatsNum.text = "";
        }

        StartCoroutine(GetPokemon(pokemonRandomIndex));
    }

    IEnumerator GetPokemon(int pokemonRandomIndex)
    {
        string pokemonURL = pokemonApiURL + "pokemon/" + pokemonRandomIndex.ToString();

        UnityWebRequest pokemonInfoRequest = UnityWebRequest.Get(pokemonURL);

        yield return pokemonInfoRequest.SendWebRequest();

        if (pokemonInfoRequest.result == webResultConnectionError || pokemonInfoRequest.result == webResultPotocolError)
        {
            Debug.LogError(pokemonInfoRequest.error);
            yield break;
        }

        JSONNode infoPokemon = JSON.Parse(pokemonInfoRequest.downloadHandler.text);

        string pokeName = infoPokemon["name"];
        string pokeSpriteURL = infoPokemon["sprites"]["front_default"];

        JSONNode poketypes = infoPokemon["types"];
        string[] poketypesNames = new string[poketypes.Count];

        for (int i = 0, j = poketypes.Count -1; i < poketypes.Count; i++, j--)
        {
            poketypesNames[j] = poketypes[i]["type"]["name"];
        }

        JSONNode pokeStats = infoPokemon["stats"];
        string[] pokeStatsNumbers = new string[pokeStats.Count];

        for (int i = 0; i < pokeStats.Count; i++)
        {
            pokeStatsNumbers[i] = pokeStats[i]["base_stat"];
        }

        UnityWebRequest spritePokemonRequest = UnityWebRequestTexture.GetTexture(pokeSpriteURL);
        yield return spritePokemonRequest.SendWebRequest();

        if (spritePokemonRequest.result == webResultConnectionError || spritePokemonRequest.result == webResultPotocolError)
        {
            Debug.Log(spritePokemonRequest.error);
            yield break;
        }

        pokemonRawImage.texture = DownloadHandlerTexture.GetContent(spritePokemonRequest);
        pokemonRawImage.texture.filterMode = FilterMode.Point;

        pokemonName.text = CapitalizeFirstLetter(pokeName);

        for (int i = 0; i < poketypesNames.Length; i++)
        {
            pokeTypeTextArray[i].text = CapitalizeFirstLetter(poketypesNames[i]);
        }
        for (int i = 0; i < pokeStatsNumbers.Length; i++)
        {
            pokemonStats[i].text = CapitalizeFirstLetter(pokeStatsNumbers[i]);
        }

        Debug.Log(pokeName);
    } 

    private string CapitalizeFirstLetter(string str)
    {
        return char.ToUpper(str[0]) + str.Substring(1);
    }

    UnityWebRequest.Result webResultConnectionError = UnityWebRequest.Result.ConnectionError;
    UnityWebRequest.Result webResultPotocolError = UnityWebRequest.Result.ProtocolError;
}
