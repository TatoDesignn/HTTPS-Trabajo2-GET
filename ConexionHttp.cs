using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class ConexionHttp : MonoBehaviour
{
    [SerializeField] private RawImage[] imagen;
    [SerializeField] private TextMeshProUGUI nombreArriba;
    [SerializeField] private TextMeshProUGUI[] nombreUsers;
    [SerializeField] private TextMeshProUGUI[] nombreEspecie;

    private string urlJson = "https://my-json-server.typicode.com/TatoDesign/jsonDB-Actividad2-SID/users";
    string urlAPI = "https://rickandmortyapi.com/api/character";
    private int numero = 0;
    private int conteo = 0;

    private void Start()
    {
        numero = 1;

        StartCoroutine("GetDeck", numero);
    }

    public void Siguiente()
    {
        if (numero == 5)
        {
            numero = 0;
        }

        numero += 1;

        StartCoroutine("GetDeck", numero);
    }

    public void Atras()
    {
        if (numero == 1)
        {
            numero = 6;
        }

        numero -= 1;

        StartCoroutine("GetDeck", numero);
    }

    IEnumerator GetDeck(int numero)
    {
        UnityWebRequest www = UnityWebRequest.Get(urlJson + "/" + numero);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.responseCode == 200)
            {
                Deck decks = JsonUtility.FromJson<Deck>(www.downloadHandler.text);

                nombreArriba.text = decks.username;

                for (int i = 0; i < decks.deck.Length; i++)
                {
                    yield return StartCoroutine("GetRickandMorty", decks.deck[i]);
                }
            }
            else
            {
                Debug.Log($"Status: {www.responseCode} \n Error: {www.error}");
            }
        }
    }

    IEnumerator GetRickandMorty(int id)
    {
        if(conteo == 5)
        {
            conteo = 0;
        }

        UnityWebRequest www = UnityWebRequest.Get(urlAPI + "/" + id);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.responseCode == 200)
            {
                Character character = JsonUtility.FromJson<Character>(www.downloadHandler.text);

                nombreUsers[conteo].text = character.name;
                nombreEspecie[conteo].text = character.species;

                yield return StartCoroutine("GetImageTexture", character.image);
            }
            else
            {
                Debug.Log($"Status: {www.responseCode} \n Error: {www.error}");
            }
        }

        conteo += 1;
    }

    IEnumerator GetImageTexture(string imageUrl)
    {
        UnityWebRequest wwwTex = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return wwwTex.SendWebRequest();

        if (wwwTex.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(wwwTex.error);
        }
        else
        {
            imagen[conteo].texture = ((DownloadHandlerTexture)wwwTex.downloadHandler).texture;
        }
    }

    class Character
    {
        public int id;
        public string name;
        public string species;
        public string image;
    }

    class Deck
    {
        public int id;
        public string username;
        public int[] deck;
    }
}
