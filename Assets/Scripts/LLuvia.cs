using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class LLuvia : NetworkBehaviour
{
    public ParticleSystem lluvia;

    private bool estado; 
    public bool activar;

    
    // Cuando inicio, no quiero que este lloviendo
    private void Awake() {
        lluvia.Stop();
    }

    public void Update() {
        
        if(IsServer)
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
    
    //Se ejecuta en el cliente, activando la lluvia en todos al mismo tiempo
    [ClientRpc]
    private void ActivarLluviaClientRpc(){
        lluvia.Play();
    }

    //Se ejecuta en el cliente, desactivando la lluvia en todos al mismo tiempo
    [ClientRpc]
    private void DesactivarLluviaClientRpc(){
        lluvia.Stop();
    }

}
