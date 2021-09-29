using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Reflection;

public class PostMessageJson {
    public string message;
    public string sender;
}

[Serializable]
public class mensajeEnviadoPorChatbot 
{
    public string recipient_id;
    public string text;
    
}

[Serializable]
public class RootmensajeEnviadoPorChatbot {
    public mensajeEnviadoPorChatbot[] messages;
}


public class NetworkManagerRasa : MonoBehaviour {

    public const string host = "localhost";

    private const string rasa_url = "http://" + host + ":5005/webhooks/rest/webhook";

    public void SendMessageToRasa (string msj) {
        
        PostMessageJson postMessage = new PostMessageJson {
            sender = "user",
            message = msj
        };

        string jsonBody = JsonUtility.ToJson(postMessage);
       
        StartCoroutine(PostRequest(rasa_url, jsonBody));
    }

    private IEnumerator PostRequest (string url, string jsonBody) {

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] rawBody = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawBody);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Respuesta(request.downloadHandler.text);
    }

    public void Respuesta (string response) {

        RootmensajeEnviadoPorChatbot recieveMessages = JsonUtility.FromJson<RootmensajeEnviadoPorChatbot>("{\"messages\":" + response + "}");

        
        foreach (mensajeEnviadoPorChatbot message in recieveMessages.messages) {

            FieldInfo[] fields = typeof(mensajeEnviadoPorChatbot).GetFields();

            foreach (FieldInfo field in fields) {
                string data = null;

                try {
                    data = field.GetValue(message).ToString();
                } catch (NullReferenceException) { }

                if (data != null && field.Name != "recipient_id") {
                    
                    ////////////////////////////////////////////////////////////
                    if (Physics.Raycast(transform.position + transform.up * 0.75f, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 2.5f))
                    {
                        var player = hit.collider.GetComponent<Chat>();

                        if (player != null)
                        {
                            player.mensajeEnPantalla(data);
                        }
                    }
                    ////////////////////////////////////////////////////////////

                }
            }
        }
         
    }
    
}
