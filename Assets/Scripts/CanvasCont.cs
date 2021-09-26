using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCont : MonoBehaviour
{
    public GameObject canvasObject; // drag your canvas object to this variable in the editor
    // make your canvas active from a disables state by calling this method
    public void Start()
    { 
        canvasObject.SetActive(false);
    }

    public void ActivateCanvas()
    {
        canvasObject.SetActive(true);
    }
    
    public void DesactivateCanvas()
    {
        canvasObject.SetActive(false);
    }
   
}
