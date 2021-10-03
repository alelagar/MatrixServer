using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;

public class Chat : NetworkBehaviour
{
    public GameObject chatPanel, textObject;

    public InputField chatBox;

    [SerializeField]
    List<Mensaje> listaMensajes = new List<Mensaje>();

    public int max = 10;

    private bool act = false;
    
    private void Update() 
    {

        if(!IsLocalPlayer) {return;}
        
        if(Input.GetMouseButtonDown(1))
        {
           
           if (!act)
           {
               GetComponent<PlayerMove>().movApagado();
               GetComponent<CanvasCont>().ActivateCanvas();
               act = true;
           }
           else
           {
               GetComponent<PlayerMove>().movActivo();
               GetComponent<CanvasCont>().DesactivateCanvas();
               act = false;
           }
           
        }
        if(chatBox.text != "")
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                var jugador = GetComponent<PlayerMove>();
                jugador.MandarMensaje(chatBox.text);

                mensajeEnPantalla(chatBox.text);
                chatBox.text = "";
            }
        }
        
    }

    public void mensajeEnPantalla(string text)
    {

        if(listaMensajes.Count >= max)
        {
            Destroy(listaMensajes[0].textObject.gameObject);
            listaMensajes.Remove(listaMensajes[0]);
        }

        Mensaje newMessage = new Mensaje();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
     
        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;

        listaMensajes.Add(newMessage);
    }
    
}

[System.Serializable]
public class Mensaje 
{
    public string text;
    public Text textObject; 
}
