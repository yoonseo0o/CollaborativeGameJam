using UnityEngine;
public enum DeploymentPossibility { possible, impossible, canBuy , cantBuy}
public class StructDeploySupport : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshRenderer meshRenderer2;
    public DeploymentPossibility possibility { get; private set; }
    private int enterStructure;
    private void Awake()
    { 
        enterStructure = 0; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Structure"))
        {
            enterStructure++;
            ChangePossibility(DeploymentPossibility.impossible); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Structure"))
        {
            enterStructure--;
            if (enterStructure <= 0)
            {
                ChangePossibility(DeploymentPossibility.possible);
            }
        }
    }
    public void ChangePossibility(DeploymentPossibility input)
    {
        DeploymentPossibility temp = possibility;
        switch (input)
        {
            case DeploymentPossibility.impossible:
                if (possibility == DeploymentPossibility.cantBuy)
                    break;
                possibility = input;
                break;
            case DeploymentPossibility.possible:
                if (possibility == DeploymentPossibility.cantBuy)
                    break;
                possibility = input;
                break;
            case DeploymentPossibility.cantBuy:
                possibility = input;
                break;
            case DeploymentPossibility.canBuy:
                if (enterStructure <= 0)
                    possibility = DeploymentPossibility.possible;
                else
                    possibility = DeploymentPossibility.impossible;
                break;
            default:
                break;
        } 
        if (temp != possibility)
            changeMaterial();
    }
    private void changeMaterial()
    {
        if ((int)possibility >= materials.Length)
            Debug.LogError("materials¿Ã æ¯¿Ω"); 
        meshRenderer.material = materials[(int)possibility];
        meshRenderer2.material = materials[(int)possibility];
    }
}
