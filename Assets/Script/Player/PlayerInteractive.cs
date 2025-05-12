using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInteraction : MonoBehaviour
{
    public Transform lookTarget;
    int interactionLayer = 1 << 7;
    float maxDistance = 8f; 
    public void GetLookTarget(InputAction.CallbackContext context)
    {
        Vector3 direction = Camera.main.transform.forward; 
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, maxDistance, interactionLayer))
        { 
            lookTarget = hitInfo.transform; 
        }
        else
        {
            lookTarget = null;
        }
    }
    public void OnInteract()
    { 
        if (lookTarget != null)
        {
            lookTarget.GetComponent<Interaction>().interaction();
        } 
    }
}