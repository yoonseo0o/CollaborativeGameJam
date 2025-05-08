using UnityEngine;
using UnityEditor.AI;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using UnityEngine.Windows;
using System.Collections;
public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
     
    private InputAction moveAction;
    private InputAction lookAction;
    private Coroutine co;
    private float sensitivity=10;
     
    void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        lookAction.performed += OnLookPerformed; 
    } 
    void OnLookPerformed(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        float yaw = mouseDelta.x * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, yaw, 0);
    }

    void OnMovePerformed(InputAction.CallbackContext context)
    { 
        if (co != null) { StopCoroutine(co); }
        
        Vector2 input = context.ReadValue<Vector2>();
        co=StartCoroutine(move(new Vector3(input.x, 0, input.y)));
    }
    void OnMoveCanceled(InputAction.CallbackContext context)
    { 
        StopCoroutine (co); 
    }
    IEnumerator move (Vector3 pos)
    {
        while(true)
        {
            Vector3 moveDir = transform.right * pos.x + transform.forward * pos.z;
            transform.localPosition += moveDir.normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
    }
    
}
