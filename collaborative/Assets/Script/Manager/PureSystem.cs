using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class PureSystem : MonoBehaviour
{
    public int pure { get; private set; }

    [SerializeField] private float interval;
    private void Start()
    {
        AddPureOverTime();
    }
    public void GetPure()
    {
        pure ++;
        GameManager.Instance.UIManager.UpdatePureCount(pure);
    }
    public void GetPure(int amount)
    {
        pure += amount;
        GameManager.Instance.UIManager.UpdatePureCount (pure);
    }
    public void LosePure(int amount)
    {
        pure -= amount;
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
