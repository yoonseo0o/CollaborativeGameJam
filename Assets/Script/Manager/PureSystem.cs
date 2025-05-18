using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class PureSystem : MonoBehaviour
{
    public int pure { get; private set; }
    private int maxPure = 999999;
    [SerializeField] private float interval;
    private void Start()
    {
        pure = 0; 
        GameManager.Instance.UIManager.UpdatePureCount(pure);
        AddPureOverTime();
    }
    public void GetPure()
    {
        pure ++;
        GameManager.Instance.StructureSystem.CheckBuyAbility();
        GameManager.Instance.UIManager.UpdatePureCount(pure);
    }
    public void GetPure(int amount)
    {
        pure += amount;
        if (pure >= maxPure) 
            pure = maxPure; 

        GameManager.Instance.StructureSystem.CheckBuyAbility();
        GameManager.Instance.UIManager.UpdatePureCount (pure);
    }
    public void LosePure(int amount)
    {
        pure -= amount;
        GameManager.Instance.StructureSystem.CheckBuyAbility();
        GameManager.Instance.UIManager.UpdatePureCount(pure);
    }
    private void AddPureOverTime()
    {
        InvokeRepeating("GetPure", interval, interval);
    }
    private void OnDestroy()
    {
        CancelInvoke("GetPure");
    }
}
