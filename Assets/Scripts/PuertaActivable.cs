using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaActivable : MonoBehaviour
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

    void OnCollisionEnter(Collision other)
    {
        {transform.Rotate(0,90,0);}
    }
}
