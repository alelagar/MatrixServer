using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine.EventSystems;

public class Jugador : NetworkBehaviour
{
    [SerializeField] private CharacterController controller = null;

    public float velocidad = 2.0F;
    public float velocidadRotacion=250f;
    public float velocidadSalto = 8.0F;
    public float gravity = 20.0F;
    public Animator animator;
    public Camera cam;

    public EventSystem eventSystem;

    private Vector3 moveDirection = Vector3.zero;

    private float distancia = 2.5f;

    private float  x, y;

    private bool movimiento = true;
                                    
    public void Start() 
    {
        if (IsLocalPlayer)
        {
            gameObject.tag = "Local";
        }
        // Si es mi script, retorno
        if (IsOwner) return;
 
        // Apago la camara y sonido
        cam.GetComponent<AudioListener>().enabled  =  false;
        cam.enabled = false;
        eventSystem.enabled = false;

    }
    
    public void movimientoActivo()
    {
        movimiento = true;
    }
    
    public void movimientoApagado()
    {
        movimiento = false;
    }

    private void Update()
    {
        if(!IsOwner) {return;}

        if (movimiento)
        {
            Movimiento();
        }
        activarObjeto();
    }
                            
    private void Movimiento()
    {
        if (controller.isGrounded) {
            y = Input.GetAxis("Vertical");
            x = Input.GetAxis("Horizontal");
            moveDirection = new Vector3(0, 0, y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= velocidad;
            animator.SetFloat("VelX", x);
            animator.SetFloat("VelY", y);

            if (Input.GetButton("Jump"))
                moveDirection.y = velocidadSalto;
            
        }
        
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        transform.Rotate(0, x * velocidadRotacion * Time.deltaTime, 0);
        
    }
    
    // Metodo que se ejecuta en el servidor
    // Recibe como parametro el cliente al cual se le debe enviar un mensaje
    [ServerRpc]
    private void MensajeServerRpc(ulong clientId, string msj)
    { 
        if (!IsServer) return;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]{clientId}
            }
        };

        MensajeClientRpc(msj, clientRpcParams);
    }
    
    //Metodo que se ejecuta en el cliente
    [ClientRpc]
    private void MensajeClientRpc(string  msj, ClientRpcParams clientRpcParams = default)
    {
        if (IsOwner) return;
        
        // Busco al clone local
        // Desactivo su movimiento porque le esta por llegar un mensaje
        // Activo su metodo para mostrar el mensaje por pantalla
        GameObject player = GameObject.FindGameObjectWithTag ("Local");
        player.GetComponent<Jugador>().movimientoApagado();
        player.GetComponent<Chat>().MensajeEnPantalla(msj);
        player.GetComponent<ControladorCanvas>().ActivarCanvas();
        
    }
    
    public void MandarMensaje(string mensaje)
    {
        // Tiro el raycast
        if (Physics.Raycast(transform.position + transform.up * 0.75f, transform.TransformDirection(Vector3.forward), out RaycastHit hit, distancia))
        {
            var jugador = hit.collider.GetComponent<NetworkObject>();
            
            // Si le pegue a un objeto que tiene un NetworkObject no nulo, es porque es un cliente
            if(jugador != null)
            {
                //Obtengo su id para comunicarme con el y llamo al metodo "MensajeServerRpc" para enviar el mensaje
                ulong jugadorId = hit.collider.GetComponent<NetworkObject>().OwnerClientId;
                MensajeServerRpc(jugadorId, mensaje);
            }
            else
            {
                var bot = hit.collider.GetComponent<Rasa>();
                
                // Si el objeto tiene un componente Rasa, es el chatbot
                if(bot != null)
                {
                    //le envio el mensaje por su metodo correspondiente
                    bot.MensajeARasa(mensaje);
                }
            }
        }
    }

    public void activarObjeto()
    {
        if (Physics.Raycast(transform.position + transform.up * 0.75f, transform.TransformDirection(Vector3.forward), out RaycastHit hit, distancia))
        {
            var puerta = hit.collider.GetComponent<PuertaActivable>();
            if(Input.GetKeyDown(KeyCode.E))
            {
                if (puerta != null)
                {
                    puerta.Activar();
                } 
                
            }
        }
    }
}