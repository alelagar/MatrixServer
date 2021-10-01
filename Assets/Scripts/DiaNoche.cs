using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class DiaNoche : NetworkBehaviour
{
    float rotationScale = 5.0f;
    private NetworkVariableQuaternion posicion = new NetworkVariableQuaternion();

    void Update()
    {
 
        if (IsHost)
        {
            ControlarDia();
        }
        
    }
 
    private void ControlarDia()
    {
        transform.Rotate(rotationScale * Time.deltaTime, 0, 0);
        posicion.Value = transform.rotation;
    }

    private void OnEnable()
    {
        posicion.OnValueChanged += actualizarRotacion;
    }
    
    void actualizarRotacion(Quaternion old, Quaternion newValue)
    {
        if(IsHost){return;}
        transform.rotation = newValue;
    }
}
