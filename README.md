## Peticiones HTTP 🖥️
<p align="center">
  <img style="width: 700px; height: auto;" src="">
</p> 

Consultas web por medio de unity, utilizando el sistema `UnityWebRequest`, *API falsa* para el gestionamiento de barajas por usuario y *API RICK AND MORTY* el cual gestiona los diseños, nombre y especie de la baraja. 

## Procedimiento 🃏
- Importamos la libreria de Unity Networking:
```C#
using UnityEngine.Networking;
```
- Definimos las direccion de las 2 API (db.json y RickAndMorty)
```C#
private string urlJson = "https://my-json-server.typicode.com/TatoDesign/jsonDB-Actividad2-SID/users";
private string urlAPI = "https://rickandmortyapi.com/api/character";
```
- Definimos una clase, con la cual transformaremos los datos json de la *API falsa* en variables de C#, debe de tener los mismo nombres que la api. 
```C#
    class Deck
    {
        public int id;
        public string username;
        public int[] deck;
    }
```
- Definimos un metodo para realizar las peticiones a nuestro *json* este tiene un parametro entero, con el que cambiaremos entre los diferentes usuarios. Hacemos uso de `UnityWebRequest` por el metodo `Get`, este recibe la *url del json* y el numero entero para obtener un usuario en concreto.
```C#
    IEnumerator GetDeck(int numero)
    {
        UnityWebRequest www = UnityWebRequest.Get(urlJson + "/" + numero);
        yield return www.SendWebRequest();
```
- Verificamos si la conexion no fue exitosa en caso de que no, si el codigo obtenido es un 200 ("Encontrado") convertimos los datos json con `JsonUtility` en la ***clase*** previamente creada, seguido modificaremos un texto en la escena por el `username`
```C#
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
        }
        else
        {
            Debug.Log($"Status: {www.responseCode} \n Error: {www.error}");
        }
    }
```
-
```C#
      for (int i = 0; i < decks.deck.Length; i++)
      {
          yield return StartCoroutine("GetRickandMorty", decks.deck[i]);
      }
```
