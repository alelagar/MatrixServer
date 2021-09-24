using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class DiaNoche : NetworkBehaviour
{
   
    NetworkVariableFloat rotationScale = new NetworkVariableFloat();

    private void Start() {
        rotationScale.Value = 10;
    }

    void Update()
    {
        Debug.Log("Dia noche, valor "+ rotationScale.Value);
      transform.Rotate(rotationScale.Value * Time.deltaTime,0,0); 
    }

}
