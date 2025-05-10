using UnityEngine;
using UnityEditor.AI;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using UnityEngine.Windows;
using System.Collections; 
public class Player : MonoBehaviour
{
    private float hp;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
     
    private InputAction moveAction;
    private InputAction lookAction;
    private Coroutine co;
    private Vector3 moveInput;
    private float maxSightAngle =60f;
    private float sensitivity=10;
     
    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        lookAction.performed += OnLookPerformed; 
    }
    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"triggerEnter - {other.gameObject.name}");
    }
    #region move&look
    private float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>(); 

        // 플레이어 y축 회전
        float yaw = mouseDelta.x * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, yaw,0);

        // 카메라 x축 회전
        float cameraX = -mouseDelta.y * rotateSpeed * Time.deltaTime;
        float currentAngle = NormalizeAngle(Camera.main.transform.localEulerAngles.x);
        float nextAngle = currentAngle + cameraX;

        if (nextAngle > -maxSightAngle && nextAngle < maxSightAngle)
        {
            Camera.main.transform.Rotate(cameraX, 0, 0);
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    { 
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector3(input.x, 0, input.y);
        if (co == null)
        {
            co = StartCoroutine(move());
        }
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    { 
        if(co!=null)
        {
            StopCoroutine(co);
            co = null;
        }

    }
    private IEnumerator move ()
    {
        while (true)
        {
            Vector3 moveDir = transform.right * moveInput.x + transform.forward * moveInput.z;
            transform.localPosition += moveDir.normalized * moveSpeed * Time.deltaTime;
             
            yield return null;
        }
    }
    private void jump()
    {

    }
    #endregion

    private void Attack()
    {

    }
    public void Attacked(float damageAmount)
    {
        if (IsDead())
            { return; }
        hp-=damageAmount;
    }
    private bool IsDead()
    {
        return hp <= 0;
    }
}
