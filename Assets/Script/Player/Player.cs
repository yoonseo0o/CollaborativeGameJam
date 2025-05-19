using UnityEngine; 
using UnityEngine.InputSystem; 
using UnityEngine.Windows;
using System.Collections;
using static UnityEngine.UI.Image;
using System.Drawing; 

public class Player : MonoBehaviour, Entity
{
    [Header("property")]
    private int maxHp=10;
    private int hp=10;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    private int damageAmount = 5;
     
    private Coroutine co;
    private Vector3 moveInput;
    private float maxSightAngle = 60f;
    private float sensitivity = 10;


    [SerializeField] private PlayerInputSystem inputSystem;

    /*[Header("Look")]
    PlayerInteraction interaction;
    [Header("Attack")]
    [SerializeField] private NewFlashlight flash;*/
    private void Awake()
    {
    }
    private void Start()
    { 
        inputSystem.moveAction.performed += OnMovePerformed; 
        inputSystem.moveAction.canceled += OnMoveCanceled;
        inputSystem.lookAction.performed += OnRotatePerformed;  
        //flash.TurnOn(true);
    }
    private void OnDisable()
    {
        inputSystem.moveAction.performed -= OnMovePerformed; 
        inputSystem.moveAction.canceled -= OnMoveCanceled;
        inputSystem.lookAction.performed -= OnRotatePerformed; 
    } 
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
    bool Entity.Attacked(int damageAmount)
    {
        return ModifyHealth(-damageAmount);
    }
    void Entity.Dead()
    {
        GameManager.Instance.GameOver("player");
    } 
    public bool ModifyHealth(int amount=1)
    { 
        hp += amount;
        GameManager.Instance.UIManager.UpdatePlayerHP (hp);
        if (hp > maxHp)
            hp = maxHp;
        if (hp <= 0)
        {
            ((Entity)this).Dead();
            return true;
        }
        else 
            return false;
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
