using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Lantern : MonoBehaviour, Entity, Interaction
{
    private int hp=25;
    private float range;
    [SerializeField] private int playerHealTime;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject canvasParent;

    [SerializeField] private Transform rangeTrf;

    // canvas input 탐지
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    void Awake()
    {
        canvas.SetActive(false);
        range = rangeTrf.localScale.x;
        eventSystem = EventSystem.current;
        raycaster = canvas.GetComponent<GraphicRaycaster>();
    }
    bool Entity.Attacked(int damageAmount)
    { 
        hp -= damageAmount;
        GameManager.Instance.UIManager.UpdateLanternHP(hp);
        if (hp <= 0)
        {
            ((Entity)this).Dead();
            return true;
        }
        else 
            return false;
    }
    void Entity.Dead()
    {
        GameManager.Instance.GameOver("lantern");
    }
    void Interaction.interaction()
    {
        Debug.Log("상호작용 호출 하긴 했음");
        // 강화창 출력
        CanvasSetActive(true);
    }
    void CanvasLookAt()
    {
        canvasParent.transform.LookAt(Camera.main.transform);
        canvasParent.transform.Rotate(0, 180f, 0);
    }
    void CanvasOnClick(InputAction.CallbackContext context)
    { 
        if (GameManager.Instance.PlayerInputSystem.ActiveAbility != ActiveAbility.none)
        {
            Debug.Log("빈손으로 접근하세요.");
            return;
        }
        var buttonControl = context.control as UnityEngine.InputSystem.Controls.ButtonControl;
        if (buttonControl != null && buttonControl.name == "leftButton")
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = screenCenter
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {  
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                    //CanvasSetActive(false);
                    break; 
                }

            }
        }
    }
    
    public void CanvasSetActive(bool active)
    {
        if (canvas.activeSelf == active) return;

        canvas.SetActive(active);
        if(active)
        {
            GameManager.Instance.PlayerInputSystem.abilityAction.performed += CanvasOnClick;
            InvokeRepeating("CanvasLookAt", 0, Time.deltaTime);   
        }
        else
        {
            GameManager.Instance.PlayerInputSystem.abilityAction.performed -= CanvasOnClick;
            CancelInvoke("CanvasLookAt");
        }
    }
    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        { 
            other.GetComponent<Player>().InvokeRepeating("ModifyHealth", playerHealTime, playerHealTime);
            
        }
    }
    private void OnTriggerExit(Collider other)
    { 
        if (other.CompareTag("Player"))
        { 
            other.GetComponent<Player>().CancelInvoke("ModifyHealth");
            CanvasSetActive(false);
        }
    } 
}
