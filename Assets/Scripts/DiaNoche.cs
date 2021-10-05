using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class DiaNoche : NetworkBehaviour
{
    float rotacion = 5.0f;
    
    // Es networkVariable para que MLAPI se encargue de mantener la sincronizacion
    private NetworkVariableQuaternion posicion = new NetworkVariableQuaternion();

    void Update()
    {
        if (IsServer)
        {
            ControlarDia();
        }
    }
    private void ControlarDia()
    {
        //El servidor actualiza su transform y actualiza el valor de "posicion"
        transform.Rotate(rotacion * Time.deltaTime, 0, 0);
        posicion.Value = transform.rotation;
    }

    //Este metodo es para indicar que me quiero subscribir al evento de "actualizarRotacion"
    private void OnEnable()
    {
        posicion.OnValueChanged += ActualizarRotacion;
    }
    
    // Sirve para actualizar la rotacion en los clientes 
    // La variable "old" no se usa 
    void ActualizarRotacion(Quaternion old, Quaternion newValue)
    {
        if(IsHost){return;}
        transform.rotation = newValue;
    }
}
