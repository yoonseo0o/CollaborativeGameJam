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
        // 오브젝트 위치 실시간 반영
        while(true)
        {
            selectObj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distanceToCamera;
            yield return null;
        }
    }
    public bool DeployStructure()
    {

        if (StructureList[SelectIndex].GetComponent<Structure>().pureCost >
            GameManager.Instance.PureSystem.pure)
        {
            Debug.Log("동심 부족");
            return false;
        }
        //Debug.Log($"{StructureList[SelectIndex].GetComponent<Structure>().pureCost} 동심을 소모해 {StructureList[SelectIndex]}를 배치하였습니다");
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
}
