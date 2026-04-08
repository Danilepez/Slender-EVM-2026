using UnityEngine;
using UnityEngine.InputSystem;

public class Note : MonoBehaviour
{
    private MeshRenderer[] meshRenderer;
    private Material[] originalMaterial;
    public Material highlightMaterial;
    public float lookRange = 5f;

    private PlayerLook player;
    private Camera playerCamPosition;
    private bool isLookedAt = false;
    void Start()
    {
        meshRenderer = GetComponentsInChildren<MeshRenderer>();
        originalMaterial = new Material[meshRenderer.Length];
        for (int i = 0; i < meshRenderer.Length; i++)
        {
            originalMaterial[i] = meshRenderer[i].material;
        }
        player = FindAnyObjectByType<PlayerLook>();
        playerCamPosition = player.GetComponentInChildren<Camera>();
    }

    void Update()
    {
        Ray ray = new Ray(playerCamPosition.transform.position, playerCamPosition.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, lookRange))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                isLookedAt = true;
                Debug.Log("Looking at note" + isLookedAt);
                IsLookedAt(isLookedAt);
            }
            return;
        } 
        else
        {
            isLookedAt = false;
            Debug.Log("Looking at note else" + isLookedAt);
            IsLookedAt(isLookedAt);
        }
    }

    public void IsLookedAt(bool isLookAt)
    {
        isLookedAt = isLookAt;
        if (isLookedAt)
        {
            foreach (MeshRenderer mr in meshRenderer)
            {
                mr.material = highlightMaterial;
                Debug.Log("Highlighting note");
            }
            Debug.Log("Highlighting note f" + isLookedAt);

            /*for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].material = highlightMaterial;
            }*/
        }
        else
        {
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].material = originalMaterial[i];
            }
        }
    }

    public void OnCollect(InputValue data)
    {
        if (data.isPressed)
        {
            Debug.Log("OnCollect");
            Destroy(this.gameObject);
        }
    }
}
