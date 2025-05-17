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
    [SerializeField] private Image[] playerHpEraser;
    [SerializeField] private GameObject[] toolSlot;

    
    [Header("PopUp")]
    [SerializeField] private GameObject gameClearPopUp;
    [SerializeField] private GameObject gameOverPopUp;
    [SerializeField] private GameObject pureLackPopUp;
    [SerializeField] private GameObject descriptionStructure;

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
        for (int i = 0; i < currentHp; i++)
        {
            playerHpEraser[i].enabled = false;
        }
        for (int i = currentHp; i < playerHpEraser.Length; i++)
        {
            playerHpEraser[i].enabled = true;
        }
    }

    public void UpdateLanternHP(int currentHp)
    {
        hpLantern.value = (float)currentHp / GameManager.Instance.lanternMaxHp;
    }
    public void UpdatePureCount(int count)
    {
        pureCount.text = count.ToString();
    }
    public void UpdateToolSlot(int state)
    {
        if( state <0 && state>= toolSlot.Length )
        {
            Debug.Log("index out range");
            return;
        }
        foreach (var s in toolSlot)
        {
            s.SetActive(false);
        }
        toolSlot[state].SetActive(true);
    }
    public void ActivePureLack(bool IsActive)
    {
        pureLackPopUp.SetActive(IsActive);
    }
    public void ActiveDescriptionStructure(bool IsActive)
    {
        descriptionStructure.SetActive(IsActive);
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
    public void GameClearPopUp(bool IsActive)
    {
        gameClearPopUp.SetActive(IsActive);
    }
    public void GameOverPopUp(bool IsActive)
    {
        gameOverPopUp.SetActive(IsActive);
    }
}
