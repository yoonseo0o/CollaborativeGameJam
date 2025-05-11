using TMPro;
using UnityEngine; 
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider hpLantern;
    [SerializeField] private Slider hpPlayer;
    [SerializeField] private TMP_Text pureCount; 
    private void Start()
    {
        hpPlayer.value =   GameManager.Instance.playerMaxHp;
        hpLantern.value = GameManager.Instance.lanternMaxHp;
    }
    public void UpdatePlayerHP(int currentHp)
    {
        hpPlayer.value = (float)currentHp / GameManager.Instance.playerMaxHp;
    }

    public void UpdateLanternHP(int currentHp)
    {
        hpLantern.value = (float)currentHp / GameManager.Instance.lanternMaxHp;
    }
    public void UpdatePureCount(int count)
    {
        pureCount.text = count.ToString();
    }
}
