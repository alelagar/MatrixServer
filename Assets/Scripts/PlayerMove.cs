using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;


public class PlayerMove : NetworkBehaviour
{
    
    [SerializeField] private CharacterController controller = null;
    public float speed = 2.0F;
    public float rotationSpeed=250f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public Animator animator;
    public Camera cam;
    private Vector3 moveDirection = Vector3.zero;

    private float distancia = 1.5f;

    private float  x, y;

    public void Start() 
    {
        // Si es mi script, retorno
        if (IsOwner) return;
 
        // Apago la camara y sonido, porque no son mios
        cam.GetComponent<AudioListener> ().enabled  =  false;
        cam.enabled = false;
    }

    private void Update()
    {
        if(!IsOwner) {return;}
        
        Movimiento();

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(IsLocalPlayer)
            {
                MandarMensaje();
            }
        }
    }

    private void Movimiento()
    {
        if (controller.isGrounded) {
            y = Input.GetAxis("Vertical");
            x = Input.GetAxis("Horizontal");
            moveDirection = new Vector3(0, 0, y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            animator.SetFloat("VelX", x);
            animator.SetFloat("VelY", y);

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
            
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        transform.Rotate(0, x * rotationSpeed * Time.deltaTime, 0);

    }

    [ServerRpc]
    private void MensajeServerRpc(ulong clientId)
    { 
        if (!IsServer) return;
        Debug.Log("Server ejecuta");
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]{clientId}
            }
        };
        int n = 8;
        MensajeClientRpc(n, clientRpcParams);
    }
    

    [ClientRpc]
    private void MensajeClientRpc(int  n, ClientRpcParams clientRpcParams = default)
    {
        if (IsOwner) return;

        Debug.Log(n);
    }

    public void MandarMensaje()
    {
        GameObject t = GameObject.FindWithTag("transformCuerpo");

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, distancia))
        {
            var player = hit.collider.GetComponent<NetworkObject>();
    
            if(player != null)
            {
                ulong playerId = hit.collider.GetComponent<NetworkObject>().OwnerClientId;
                Debug.Log("OwnerId "+ playerId);
                string msj = "Este es el mensaje";

                MensajeServerRpc(playerId);
            }
        }
    }

}
