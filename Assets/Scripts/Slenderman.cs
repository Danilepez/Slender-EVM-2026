using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Slenderman : MonoBehaviour
{
    private NavMeshAgent navMeshAgentSlender;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private SkinnedMeshRenderer slenderMeshRenderer;
    private Animator slenderAnimator;

    private float baseSpeed = 0.5f;
    private float catchDistance = 2f;
    private bool isGameOver = false;


    void Start()
    {
        navMeshAgentSlender = GetComponent<NavMeshAgent>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerLook = FindAnyObjectByType<PlayerLook>();
        slenderMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        slenderAnimator = GetComponent<Animator>();

        navMeshAgentSlender.speed = baseSpeed;

    }

    void Update()
    {
        if (isGameOver) return;
        if (navMeshAgentSlender.enabled)
        {
            navMeshAgentSlender.destination = playerMovement.transform.position;
            float currentVelocity = navMeshAgentSlender.velocity.magnitude;
            slenderAnimator.SetFloat("speed", currentVelocity);
            VerificarDistanciaConJugador();
        }
        CambiarDificultad();

    }

    public void VerificarDistanciaConJugador()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerMovement.transform.position);
        if (distanceToPlayer <= catchDistance)
        {
            AtraparJugador();
        }
    }

    public void AtraparJugador()
    {
        isGameOver = true;
        // Detener a Slender
        navMeshAgentSlender.isStopped = true;
        navMeshAgentSlender.velocity = Vector3.zero;

        // Hacer que Slender mire al jugador
        Vector3 slenderlookAt = new Vector3(playerMovement.transform.position.x, transform.position.y, playerMovement.transform.position.z);
        transform.LookAt(slenderlookAt);
        slenderAnimator.SetTrigger("jumpscare");

        // Desactivar controles del jugador
        playerMovement.enabled = false;
        playerLook.enabled = false;

        // Forzar a la c�mara del jugador a mirar a la cara de Slender
        Vector3 playerLookAtSlender = transform.position + (Vector3.up * 2f);
        playerLook.playerCamera.LookAt(playerLookAtSlender);

        LoadGameOverScene();

    }

    private void LoadGameOverScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CambiarDificultad()
    {
        int notas = GameManager.Instance.GetNotesCount();
        if (notas < 1)
        {
            navMeshAgentSlender.enabled = false;
            slenderMeshRenderer.enabled = false;
        }
        else
        {
            navMeshAgentSlender.enabled = true;
            slenderMeshRenderer.enabled = true;
        }
        navMeshAgentSlender.speed = baseSpeed + (notas * 0.5f);
        if (notas >= 5)
        {
            slenderAnimator.SetBool("isRunning", true);
            navMeshAgentSlender.acceleration = 12f;
        }

    }
}
