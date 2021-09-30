using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaActivable : MonoBehaviour
{
    bool abierta = false;

    public void Activar()
    {

            if (!abierta)
                {
                    transform.Rotate(0,-90,0);
                    abierta= true;
                }

            else
                {
                    transform.Rotate(0,90,0);
                    abierta=false;    
                }

    }

    void OnCollisionEnter(Collision other)
    {
        {transform.Rotate(0,90,0);}
    }
}
