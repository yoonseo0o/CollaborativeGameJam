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
            Debug.LogError("parameter structure�� null��");
            return;
        }
        if(!sketchbook.activeSelf)
        {
            Debug.LogError("����ġ���� ��Ȱ��ȭ�ε� UI update�� �õ���");
            return;
        }
        
        structureName.text = s.name;
        structureDescription.text = $"{s.description}\\n �Ҹ� ���� {s.pureCost}";
        //structureImg.sprite = s.img;
    }
}
