using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;


public class PlayerMove : NetworkBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    [SerializeField] private CharacterController controller = null;
    public float speed = 2.0F;
    public float rotationSpeed=250f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public Animator animator;
    public Camera cam;
    private Vector3 moveDirection = Vector3.zero;

    private float  x, y;

        public void Start() 
    {
        // IF I'M THE PLAYER, STOP HERE (DON'T TURN MY OWN CAMERA OFF)
        if (IsOwner) return;
 
        // DISABLE CAMERA HERE (BECAUSE THEY ARE NOT ME)
        cam.enabled = false;
    }


    private void Update()
    {
        if(!IsOwner) {return;}

        if (controller.isGrounded) {
            y= Input.GetAxis("Vertical");
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


        if(Input.GetKeyDown(KeyCode.F))
            {
                MandarMensajeServerRpc();
            }

    }

        [ServerRpc]
    public void MandarMensajeServerRpc()
    {
        MandarMensajeClientRpc();
    }

    [ClientRpc]

    public void MandarMensajeClientRpc()
    {
        if(IsOwner){return;}

        Debug.Log("Me llego un mensaje!");
        
    }

}
