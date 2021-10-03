using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
public class PuertaActivable : NetworkBehaviour
{
    bool abierta = false;
    public GameObject puerta; 

    public void Activar()
    {

            if (!abierta)
            {
                puerta.transform.Rotate(0,-80,0);
                abierta= true;
            }

            else
            {
                puerta.transform.Rotate(0,80,0);
                abierta=false;    
            }

    }


}
