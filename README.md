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
    }
```
- Verificamos si la conexion no fue exitosa en caso de que no, si el codigo obtenido es un 200 ("Encontrado") convertimos los datos json con `JsonUtility` en la ***clase*** previamente creada, seguido modificaremos un texto en la escena por el `username`.
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
- Para cada usuario seran 5 cartas por la tonto debemos de iterar para que se muestren cada una, por medio de un ciclo for iniciando desde 0 hasta la cantidad total del arreglo `deck[]`, llamamos el metodo para obtener las peticion de la ***API de RickAndMorty***.
```C#
      for (int i = 0; i < decks.deck.Length; i++)
      {
          yield return StartCoroutine("GetRickandMorty", decks.deck[i]);
      }
```
- De la misma manera que declaramos la clase para la *API falsa* debemos de hacerlo para la de *Rick and Morty* con sus nombres respectivos.
```C#
    class Character
    {
        public int id;
        public string name;
        public string species;
        public string image;
    }
```
- El metodo `GRickandMorty` es muy similar al explicado anteriormente, se agrega una condicion de conteo el cual tiene como funcion modificar la posicion del arreglo `TextMeshProUGUI`, y asi mostrar en pantalla su nombre y especie.
```C#
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
                Character character = JsonUtility.FromJson<Character>    (www.downloadHandler.text);

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
```
- En el metodo anterior se llama a `GetImageTexture`, este funciona de la misma manera que el anterior, su cambio se ve en que la peticion es por medio de `GetTexture`, para obtener la textura y mostrar la imagen.
```C#
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
```
## Mas Información 💾
<ul>
  <li><a href="https://tatodesign.github.io/jsonDB-Actividad2-SID/">Link de GitHub pages</a></li>
</ul>
