using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed = 5f;
    public InputActionReference sprintAction;

    private Vector2 movementInput;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
    }

    //el nombre del metodo tiene que coincidir con el nombre en el System Input Actions
    public void OnMovement(InputValue data)
    {
        movementInput = data.Get<Vector2>();
    }

    public void MovePlayer()
    {
        //se simula que el eje z es el eje y
        Vector3 direction = transform.right * movementInput.x + transform.forward * movementInput.y;
        Debug.Log("Direction: " + direction);

        if(sprintAction.action.IsPressed())
        {
            movementSpeed = 7.5f;
        }
        else
        {
            movementSpeed = 5f;
        }
        Debug.Log("Movement Speed: " + movementSpeed);
        Debug.Log("Is Pressed: " + sprintAction.action.IsPressed());

        //si ponemos 0 en el eje y, el jugador no podrá saltar
        rb.linearVelocity = new Vector3(direction.x * movementSpeed, rb.linearVelocity.y, direction.z * movementSpeed);
    }
}
