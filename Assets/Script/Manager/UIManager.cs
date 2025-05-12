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
    [SerializeField] private TMP_Text structureName;
    [SerializeField] private TMP_Text structureDescription;
    [SerializeField] private Image structureImg;

    [Header("Object")]
    [SerializeField] private GameObject sketchbook;


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
    public void ActiveSketchbook(bool IsActive)
    {
        sketchbook.SetActive(IsActive);
    }
    public void UpdateStructure(Structure s)
    {
        if(s == null)
        {
            Debug.LogError("parameter structure가 null임");
            return;
        }
        if(!sketchbook.activeSelf)
        {
            Debug.LogError("스케치북이 비활성화인데 UI update를 시도함");
            return;
        }
        
        structureName.text = s.name;
        structureDescription.text = $"{s.description}\\n 소모 동심 {s.pureCost}";
        //structureImg.sprite = s.img;
    }
}
