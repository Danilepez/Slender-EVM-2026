using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed = 5f;
    public InputActionReference sprintAction;
    public AudioClip walkClip;
    public AudioClip runClip;

    private Vector2 movementInput;
    private Rigidbody rb;
    private AudioSource footstepSource;
    private bool isMovingLastFrame;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.loop = true;
        footstepSource.playOnAwake = false;
        footstepSource.spatialBlend = 1f;
    }

    void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
        UpdateFootsteps();
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

        if(sprintAction != null && sprintAction.action != null && sprintAction.action.IsPressed())
        {
            movementSpeed = 7.5f;
        }
        else
        {
            movementSpeed = 5f;
        }
        Debug.Log("Movement Speed: " + movementSpeed);
        Debug.Log("Is Pressed: " + (sprintAction != null && sprintAction.action != null && sprintAction.action.IsPressed()));        

        //si ponemos 0 en el eje y, el jugador no podrá saltar
        rb.linearVelocity = new Vector3(direction.x * movementSpeed, rb.linearVelocity.y, direction.z * movementSpeed);
    }

    public void OnJump(InputValue data)
    {
        if (data.isPressed)
        {
            rb.AddForce(Vector3.up * 4f, ForceMode.Impulse);
        }
    }

    private void UpdateFootsteps()
    {
        bool isMoving = movementInput.sqrMagnitude > 0.01f;
        bool isRunning = sprintAction != null && sprintAction.action != null && sprintAction.action.IsPressed();
        AudioClip clipToPlay = isRunning ? runClip : walkClip;

        if (!isMoving || clipToPlay == null)
        {
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }

            isMovingLastFrame = false;
            return;
        }

        if (!isMovingLastFrame || footstepSource.clip != clipToPlay)
        {
            footstepSource.clip = clipToPlay;
            footstepSource.Play();
        }

        isMovingLastFrame = true;
    }
}
