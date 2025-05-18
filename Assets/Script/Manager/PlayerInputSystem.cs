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
        //Debug.Log("input¿¡¼­µµ Àß ¸ÔÇû½À´Ï´ç");
        interaction.OnInteract();
    } 
    private void OnSwitchAbilityPerformed(InputAction.CallbackContext context)
    {
        var keyControl = context.control as UnityEngine.InputSystem.Controls.KeyControl;
        var buttonControl = context.control as UnityEngine.InputSystem.Controls.ButtonControl;
        var axisControl = context.control as UnityEngine.InputSystem.Controls.Vector2Control; 
        if (keyControl != null)
        { 
            if(keyControl.keyCode==Key.Escape)
            {
                GameManager.Instance.UIManager.ActiveESCPopUp(true);
                return;
            }
            if (keyControl.keyCode == Key.Digit1)
            { 
                if(ActiveAbility == ActiveAbility.flash)
                {
                    flash.Lower();
                    ActiveAbility = ActiveAbility.none;
                }
                else
                {
                    ActiveAbility = ActiveAbility.flash;
                    flash.Raise();
                }
            }
            else if (keyControl.keyCode == Key.Digit2)
            {
                if (ActiveAbility == ActiveAbility.structure)
                {
                    ActiveAbility = ActiveAbility.none;
                }
                else
                {
                    ActiveAbility = ActiveAbility.structure;
                    flash.Lower();
                }
            }
            switch(activeAbility)
            {
                case ActiveAbility.none:
                    GameManager.Instance.UIManager.UpdateToolSlot(0);
                    GameManager.Instance.UIManager.ActiveDescriptionStructure(false);
                    GameManager.Instance.StructureSystem.DeselectStructure();
                    break;
                case ActiveAbility.flash:
                    GameManager.Instance.UIManager.UpdateToolSlot(1);
                    GameManager.Instance.UIManager.ActiveDescriptionStructure(false);
                    GameManager.Instance.StructureSystem.DeselectStructure();
                    break;
                case ActiveAbility.structure:
                    GameManager.Instance.UIManager.UpdateToolSlot(2);
                    GameManager.Instance.UIManager.ActiveDescriptionStructure(true);
                    GameManager.Instance.StructureSystem.SelectStructure();
                    break;
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
