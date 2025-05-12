using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class StructureSystem : MonoBehaviour
{
    [Header("structures")] 
    [SerializeField] private int _selectIndex;
    public int SelectIndex
    {
        get => _selectIndex;
        set { _selectIndex = ((value % StructureList.Length) + StructureList.Length) % StructureList.Length; SelectStructure(); }
    }

    [Header("Deploy")]
    [SerializeField] private float distanceToCamera;
    private GameObject selectObj;
    private Coroutine co;
    [SerializeField] private GameObject[] EmptyStructureList;
    [SerializeField] private GameObject[] StructureList;
    private LayerMask groundLayer;
    [SerializeField] private float maxDistance;
    private void Awake()
    {
        groundLayer = 1 << 8;
    } 
    public void SelectStructure()
    {
        if(SelectIndex<0 && SelectIndex>= StructureList.Length) {
            Debug.LogError("selectIndex index out range");
            return; }
        if(selectObj != null) 
        { 
            Destroy(selectObj); 
            if(co != null)
            {
                StopCoroutine(co);
                co = null;
            }
        }
        selectObj = Instantiate(EmptyStructureList[SelectIndex],transform);
        CheckBuyability();
        co = StartCoroutine(ReflectionLocation());
        GameManager.Instance.UIManager.ActiveSketchbook(true); 
        GameManager.Instance.UIManager.UpdateStructure(StructureList[SelectIndex].GetComponent<Structure>());
    }
    public void DeselectStructure()
    {
        if (co != null)
        {
            StopCoroutine(co);
            Destroy(selectObj);
            co = null;
        }
        selectObj = null;
        GameManager.Instance.UIManager.ActiveSketchbook(false);
    }
    private IEnumerator ReflectionLocation()
    {
        // ������Ʈ ��ġ �ǽð� �ݿ�
        while(true)
        { 
            Vector3 direction = Camera.main.transform.forward;
            Ray ray = new Ray(Camera.main.transform.position, direction.normalized);   

            if (Physics.Raycast(ray, out RaycastHit hit,maxDistance,groundLayer))
            { 
                selectObj.transform.position = hit.point;
            } 
            yield return null;
        }
    }
    public bool DeployStructure()
    {
        DeploymentPossibility possibility = selectObj.GetComponent<StructDeploySupport>().possibility;
        if (possibility == DeploymentPossibility.impossible|| possibility == DeploymentPossibility.cantBuy )
        { 
            return false;
        }
        GameManager.Instance.PureSystem.LosePure(
            StructureList[SelectIndex].GetComponent<Structure>().pureCost);
        if (co != null)
        {  
            GameObject obj = Instantiate(StructureList[SelectIndex], transform);
            obj.transform .position = selectObj.transform.position;  
        }
        DeselectStructure();
        return true;
    }
    public void CheckBuyability()
    {
        Debug.Log("check pure");
        if (selectObj == null) return;
        // ���� �������� ���� ���� ���¶��
        if(StructureList[SelectIndex].GetComponent<Structure>().pureCost
            <= GameManager.Instance.PureSystem.pure)
        {
            selectObj.GetComponent<StructDeploySupport>().
                ChangePossibility(DeploymentPossibility.canBuy);
        }
        else
        {
            selectObj.GetComponent<StructDeploySupport >().
                ChangePossibility(DeploymentPossibility.cantBuy);
        }
    }
}
