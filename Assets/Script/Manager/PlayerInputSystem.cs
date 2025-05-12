using UnityEngine;
using UnityEngine.InputSystem;

public enum ActiveAbility {none,flash,structure};
public class PlayerInputSystem : MonoBehaviour
{
    [Header("InputAction")]
    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction interactAction;
    public InputAction abilityAction;
     
    private ActiveAbility activeAbility;
    //[Header("Look")]
    PlayerInteraction interaction;
    [Header("Attack")]
    [SerializeField] private Flashlight flash;
    private void Awake()
    {
        interaction = GetComponent<PlayerInteraction>();

        moveAction = InputSystem.actions.FindAction("Move"); moveAction.Enable();
        lookAction = InputSystem.actions.FindAction("Look"); lookAction.Enable();
        interactAction = InputSystem.actions.FindAction("Interact"); interactAction.Enable();
        abilityAction = InputSystem.actions.FindAction("Ability"); abilityAction.Enable();
        moveAction.performed += interaction.GetLookTarget;  
        lookAction.performed += interaction.GetLookTarget;  
        interactAction.performed += OnInteracted;
        abilityAction.performed += OnSwitchAbilityPerformed;

        activeAbility = ActiveAbility.none;
    } 
    private void OnDisable()
    { 
        moveAction.performed -= interaction.GetLookTarget; 
        lookAction.performed -= interaction.GetLookTarget;
        interactAction.performed -= OnInteracted;
        abilityAction.performed -= OnSwitchAbilityPerformed;
        moveAction.Disable();
        lookAction.Disable();
        interactAction.Disable(); 
        abilityAction.Disable();
    }
    private void OnInteracted(InputAction.CallbackContext context)
    { 

        interaction.OnInteract();
    } 
    private void OnSwitchAbilityPerformed(InputAction.CallbackContext context)
    {
        var keyControl = context.control as UnityEngine.InputSystem.Controls.KeyControl;
        var buttonControl = context.control as UnityEngine.InputSystem.Controls.ButtonControl;
        var axisControl = context.control as UnityEngine.InputSystem.Controls.Vector2Control; 
        if (keyControl != null)
        {
            activeAbility = ActiveAbility.none;
            flash.TurnOn(false);
            GameManager.Instance.StructureSystem.DeselectStructure();
            if (keyControl.keyCode == Key.Digit1)
            { 
                activeAbility = ActiveAbility.flash;
                flash.TurnOn(true);
            }
            else if (keyControl.keyCode == Key.Digit2)
            { 
                activeAbility = ActiveAbility.structure;
                GameManager.Instance.StructureSystem.SelectStructure();
            }
        }
        else if (buttonControl != null)
        {
            if (buttonControl.name == "leftButton"  )
            {
                //Debug.Log("")
                if(activeAbility == ActiveAbility.structure) {
                if (GameManager.Instance.StructureSystem.DeployStructure())
                    activeAbility = ActiveAbility.none;
                }
            }
        }
        else if (axisControl != null && context.control.name == "scroll" && activeAbility == ActiveAbility.structure)
        {
            Vector2 scrollValue = context.ReadValue<Vector2>();
            GameManager.Instance.StructureSystem.SelectIndex =
                GameManager.Instance.StructureSystem.SelectIndex + Mathf.RoundToInt(Mathf.Sign(scrollValue.y));
            Debug.Log($"selectIndex : {GameManager.Instance.StructureSystem.SelectIndex}");
        }
    }

}
