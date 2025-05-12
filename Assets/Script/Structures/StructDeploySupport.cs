using UnityEngine;
public enum DeploymentPossibility { possible, impossible }
public class StructDeploySupport : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    private MeshRenderer meshRenderer;
    public DeploymentPossibility possibility {  get; private set; }
    private int enterStructure;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        enterStructure = 0;
        possibility = DeploymentPossibility.possible;
        changeMaterial();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Structure"))
        {
            enterStructure++;
            possibility = DeploymentPossibility.impossible;
        }
        if ((int)possibility >= materials.Length)
            Debug.LogError("materials¿Ã æ¯¿Ω");
        changeMaterial();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Structure"))
        {
            enterStructure--;
            if (enterStructure <= 0)
            {
                possibility = DeploymentPossibility.possible;
                changeMaterial();
            }
        }
    }
    private void changeMaterial()
    {

        meshRenderer.material = materials[(int)possibility];
    }
}
