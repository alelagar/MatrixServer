using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PostMessageJson {
    public string message;
    public string sender;
}

[Serializable]
public class ReceiveMessageJson {
    public string recipient_id;
    public string text;
 
}

public class NetworkManagerRasa : MonoBehaviour {


    public string url = "";

    private const string rasa_url = "http://localhost:5005/webhooks/rest/webhook";

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
        //ReceiveMessageJson mensaje = JsonUtility.FromJson<ReceiveMessageJson>("{"+response+"}");

            if (Physics.Raycast(transform.position + transform.up * 0.75f,
                transform.TransformDirection(Vector3.forward), out RaycastHit hit, 2.5f))
            {
                var player = hit.collider.GetComponent<Chat>();

                if (player != null)
                {
                    player.mensajeEnPantalla(response);
                }
            }
    }
    
}