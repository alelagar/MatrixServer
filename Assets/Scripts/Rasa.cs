using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Reflection;

// Clases para tratar el JSON
[Serializable]
public class PostMessageJson {
    public string message;
    public string sender;
}

[Serializable]
public class mensajeChatbot 
{
    public string recipient_id;
    public string text;
    
}

[Serializable]
public class RootmensajeEnviadoPorChatbot {
    public mensajeChatbot[] messages;
}

// Ref : https://medium.com/analytics-vidhya/integrating-rasa-open-source-chatbot-into-unity-part-1-the-connection-9ba582c804cd

public class Rasa : MonoBehaviour {

    public const string host = "localhost";

    private const string rasa_url = "http://" + host + ":5005/webhooks/rest/webhook";

    public string nombre;
    
    //Metodo para comunicarse con rasa
    public void MensajeARasa (string msj) {
        
        // Se crea un un objeto para luego parsear el JSON
        PostMessageJson postMessage = new PostMessageJson {
            sender = "user",
            message = msj
        };

        string jsonBody = JsonUtility.ToJson(postMessage);
        
        // Se realiza el envio de los datos a rasa
        StartCoroutine(PostRequest(rasa_url, jsonBody));
    }
    
    //Metodo asincrono que hace un POST al server de rasa 
    private IEnumerator PostRequest (string url, string jsonBody) {

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] rawBody = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawBody);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Respuesta(request.downloadHandler.text);
    }
    
    // Cuando el servidor rasa nos da respuesta
    public void Respuesta (string response) {

        RootmensajeEnviadoPorChatbot recieveMessages = JsonUtility.FromJson<RootmensajeEnviadoPorChatbot>("{\"messages\":" + response + "}");
        
        foreach (mensajeChatbot message in recieveMessages.messages) {
            FieldInfo[] fields = typeof(mensajeChatbot).GetFields();

            foreach (FieldInfo field in fields) {
                string data = null;
                try {
                    data = field.GetValue(message).ToString();
                } catch (NullReferenceException) { }

                if (data != null && field.Name != "recipient_id") {
                    // tenemos el mensaje de respuesta
                    // Usamos el raycast nuevamente para pegarle al jugador que hizo la consulta
                    if (Physics.Raycast(transform.position + transform.up * 0.75f, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 2.5f))
                    {
                        var player = hit.collider.GetComponent<Chat>();
                        if (player != null)
                        {
                            player.MensajeEnPantalla(nombre +": " + data);
                        }
                    }
                }
            }
        }
    }
}
