using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class LLuvia : NetworkBehaviour
{

    public ParticleSystem lluvia;

    private bool estado = false; 
    public bool activar = false;

    private void Awake() {
        lluvia.Stop();
    }

    public void Update() {
        if(IsHost)
        {
            //Si estaba prendido y quiero apagarlo
            if(estado && !activar)
            {   
                estado = false;
                DesactivarLluviaClientRpc(); 
                return;
            }
            
            //si esta apagado y quiero prender
            if(!estado && activar)
            {
                estado = true;
                ActivarLluviaClientRpc();    
            }
        }
    }

    [ClientRpc]
    private void ActivarLluviaClientRpc(){
        lluvia.Play();
    }

    [ClientRpc]
    private void DesactivarLluviaClientRpc(){
        lluvia.Stop();
    }


}
