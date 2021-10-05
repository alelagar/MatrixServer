using UnityEngine;
using MLAPI;

public class ControladorCanvas : NetworkBehaviour
{
    //Paso la referencia del canvas que quiero controlar
    public GameObject canvasObject; 

    public void Start()
    { 
        //Cuando arranca la simulacion, no quiero que se vea la interfaz grafico para los mensajes
        canvasObject.SetActive(false);
    }
    
    //Activa el canvas. (Lo muestra en pantalla)
    public void ActivarCanvas()
    {
        if(!IsLocalPlayer) {return;}
        canvasObject.SetActive(true);
    }
    //Deasctiva ... 
    public void DesactivarCanvas()
    {
        if(!IsLocalPlayer) {return;}
        canvasObject.SetActive(false);
    }
   
}
