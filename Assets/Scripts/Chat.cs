using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;

public class Mensaje 
{
    public string text;
    public Text textObject; 
}

// Sirve para manejar el canvas que aparece cuando quiero hablar con alguien
// Muestra los mensajes por pantalla y tiene un input text para ingresar el mensaje deseado

public class Chat : NetworkBehaviour
{
    public GameObject chatPanel, textObject;
    public InputField chat;
    List<Mensaje> listaMensajes = new List<Mensaje>();
    public int maxMensajes = 10;
    private bool chatActivo;
    public string nombre;

    // Si aprieto click derecho, entonces puedo mostrar o hacer desaparecer la 
    // lista de mensajes. 
    private void Update() 
    {

        if(!IsLocalPlayer) {return;}
        
        if(Input.GetMouseButtonDown(1))
        {
           // Si el chat no esta activo, entonces, lo activo (mostrar en pantalla el cuadro con mensajes)
           // Dejo quieto el movimiento del jugador
           if (!chatActivo)
           {
               GetComponent<Jugador>().movimientoApagado();
               GetComponent<ControladorCanvas>().ActivarCanvas();
               chatActivo = true;
           }
           
           // Quiero dejar de hablar y desctivo todo
           else
           {
               GetComponent<Jugador>().movimientoActivo();
               GetComponent<ControladorCanvas>().DesactivarCanvas();
               chatActivo = false;
           }
           
        }
        // Si hay un mensaje para enviar
        if(chat.text != "")
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                // Obtengo la referencia del jugador y activo el metodo para mandar el mensaje
                var jugador = GetComponent<Jugador>();
                jugador.MandarMensaje(chat.text);
                
                // Ademas debo mostrat el mensaje por pantalla
                MensajeEnPantalla(nombre +": " +chat.text);
                chat.text = "";
            }
        }
        
    }

    // Voy agregando los mensajes en una lista, hasta una cierta cantidad
    public void MensajeEnPantalla(string text)
    {
        if(listaMensajes.Count >= maxMensajes)
        {
            Destroy(listaMensajes[0].textObject.gameObject);
            listaMensajes.Remove(listaMensajes[0]);
        }
        Mensaje msj = new Mensaje();

        msj.text = text;
        GameObject texto = Instantiate(textObject, chatPanel.transform);
        msj.textObject = texto.GetComponent<Text>();
        msj.textObject.text = msj.text;
        listaMensajes.Add(msj);
    }
}

