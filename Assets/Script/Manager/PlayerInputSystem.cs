using Unity.VisualScripting;
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
    public ActiveAbility ActiveAbility { get { return activeAbility; } private set { activeAbility = value; } }
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

        ActiveAbility = ActiveAbility.none;
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
            if (keyControl.keyCode == Key.Digit1)
            { 
                if(ActiveAbility == ActiveAbility.flash)
                {
                    flash.TurnOn(false);
                    ActiveAbility = ActiveAbility.none; 
                }
                else
                {
                    ActiveAbility = ActiveAbility.flash; 
                    GameManager.Instance.StructureSystem.DeselectStructure();
                }
            }
            else if (keyControl.keyCode == Key.Digit2)
            {
                if (ActiveAbility == ActiveAbility.structure)
                {
                    ActiveAbility = ActiveAbility.none;
                    GameManager.Instance.StructureSystem.DeselectStructure();
                }
                else
                {
                    ActiveAbility = ActiveAbility.structure;
                    flash.TurnOn(false);
                    GameManager.Instance.StructureSystem.SelectStructure();
                }
            }
        }
        else if (buttonControl != null)
        {
            if (buttonControl.name == "leftButton"  )
            {
                if (ActiveAbility == ActiveAbility.flash)
                {
                    flash.Turn();

                }
                if (ActiveAbility == ActiveAbility.structure)
                {
                    if (GameManager.Instance.StructureSystem.DeployStructure())
                        ActiveAbility = ActiveAbility.none;
                }
            }
        }
        else if (axisControl != null && context.control.name == "scroll" && ActiveAbility == ActiveAbility.structure)
        {
            Vector2 scrollValue = context.ReadValue<Vector2>();
            GameManager.Instance.StructureSystem.SelectIndex =
                GameManager.Instance.StructureSystem.SelectIndex + Mathf.RoundToInt(Mathf.Sign(scrollValue.y));
            Debug.Log($"selectIndex : {GameManager.Instance.StructureSystem.SelectIndex}");
        }
    }

}
