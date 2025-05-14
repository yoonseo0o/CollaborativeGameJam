using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
        private set { instance = value; }
    }
    [Header("Manager")]
    public UIManager UIManager;
    public PureSystem PureSystem;
    public StructureSystem StructureSystem;
    public PlayerInputSystem PlayerInputSystem;

    [Header("")]
    public Transform playerTrf;
    public Transform lanternTrf;

    public int playerMaxHp { get; private set; }
    public int lanternMaxHp { get; private set; }
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerMaxHp = 10;
        lanternMaxHp = 25;
    }
    public void GameOver(string str)
    {
        Debug.Log("GameOver by "+str);
        Time.timeScale = 0;
    }
    public void GameClear()
    {
        Debug.Log("GameClear!");
        Time.timeScale = 0;
    }
}
