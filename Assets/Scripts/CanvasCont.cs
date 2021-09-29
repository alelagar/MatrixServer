using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;


public class CanvasCont : NetworkBehaviour
{
    public GameObject canvasObject; 

    public void Start()
    { 
        canvasObject.SetActive(false);
    }

    public void ActivateCanvas()
    {
        if(!IsLocalPlayer) {return;}
        canvasObject.SetActive(true);
    }
    
    public void DesactivateCanvas()
    {
        if(!IsLocalPlayer) {return;}
        canvasObject.SetActive(false);
    }
   
}
