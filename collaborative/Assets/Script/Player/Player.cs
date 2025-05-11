using UnityEngine;
using UnityEditor.AI;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using UnityEngine.Windows;
using System.Collections;
using static UnityEngine.UI.Image;
using System.Drawing;
using UnityEditor.PackageManager;

public class Player : MonoBehaviour, Entity
{
    private int maxHp=10;
    private int hp=10;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    private int damageAmount = 5;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction interactAction;
    private Coroutine co;
    private Vector3 moveInput;
    private float maxSightAngle = 60f;
    private float sensitivity = 10;

    [Header("Attack")]
    [SerializeField] private Flashlight flash;

    [Header("Look")]
    PlayerInteraction interaction;
    private void Awake()
    {
        interaction = GetComponent< PlayerInteraction>();

        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look"); 
        interactAction = InputSystem.actions.FindAction("Interact"); 
        moveAction.performed += OnMovePerformed;
        moveAction.performed += interaction.GetLookTarget;
        moveAction.canceled += OnMoveCanceled;
        lookAction.performed += OnRotatePerformed;
        lookAction.performed += interaction.GetLookTarget;
        interactAction.performed += OnInteracted;
    }
    private void Start()
    {
        flash.Init(damageAmount);
        flash.TurnOn(true);
    }
    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.performed -= interaction.GetLookTarget;
        moveAction.canceled -= OnMoveCanceled;
        lookAction.performed -= OnRotatePerformed;
        lookAction.performed -= interaction.GetLookTarget;
        interactAction.performed -= OnInteracted;
    }
    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"triggerEnter - {other.gameObject.name}");
    }*/
    #region move&look
    private float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
    private void OnRotatePerformed(InputAction.CallbackContext context)
    {
        #region 시야회전
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
        #endregion

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
    private void OnInteracted(InputAction.CallbackContext context)
    { 
        interaction.OnInteract();
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


    // 이게 필요없는거 같음. 
    // 상호작용 하면서 플레시 드니까 그때 같이 해주는게 맞고
    // 플레이어자체에는  attack이 없어야할듯
    /*private void Attack()
    {
        flash.TurnOn(true);
    }*/
    void Entity.Attacked(int damageAmount)
    {
        ModifyHealth(-damageAmount);
    }
    void Entity.Dead()
    {
        GameManager.Instance.GameOver("player");
    } 
    public void ModifyHealth(int amount=1)
    { 
        hp += amount;
        GameManager.Instance.UIManager.UpdatePlayerHP (hp);
        if (hp > maxHp)
            hp = maxHp;
        if (hp <= 0)
            ((Entity)this).Dead();
    }
    public void ModifyHealth( )
    {
        hp ++;
        GameManager.Instance.UIManager.UpdatePlayerHP(hp);
        if (hp > maxHp)
            hp = maxHp;
        if (hp <= 0)
            ((Entity)this).Dead();
    }

}
