using UnityEngine;

public class Flag : MonoBehaviour, Interaction
{
    static int remainFlagCount;
    private bool IsActive;
    [SerializeField]private GameObject light;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        IsActive = false;
        light.SetActive(true); 
    }

    void OnEnable()
    {
        remainFlagCount++;

    }
    void Interaction.interaction()
    {
        TurnOn();
    }
    private void TurnOn()
    { 
        if (IsActive) 
            return;  
        IsActive = true;
        light.SetActive ( false);
        remainFlagCount--; 
        if(remainFlagCount <= 0) {
            GameManager.Instance.GameClear();
        }
    }
}
