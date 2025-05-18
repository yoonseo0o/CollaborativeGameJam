using UnityEngine;
using UnityEngine.SceneManagement;

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
    public MonsterManager MonsterManager;
    public StreetLampManager StreetLampManager;

    [Header("System")]
    public PureSystem PureSystem;
    public StructureSystem StructureSystem;
    public PlayerInputSystem PlayerInputSystem;
    public SunSystem SunSystem;

    [Header("")]
    public MonsterSpawner MonsterSpawner;
    public Transform playerTrf;
    public Transform lanternTrf; 

    [Header("Data")]
    public FlashData flashData;
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
        
       /* Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
*/
        playerMaxHp = 10;
        lanternMaxHp = 25;
        Time.timeScale = 0;
    }
    public void GameStart()
    {
        CursorVisible(false);
        Time.timeScale = 1;
        //MonsterSpawner.StartSpawn();
    }
    public void ReStart()
    {
        SceneManager.LoadScene(0);
    }
    public void Pause(bool isPause  )
    {

        Time.timeScale = isPause?0:1;
    }
    public void CursorVisible(bool visible)
    {

        Cursor.visible = visible;
        Cursor.lockState = visible? CursorLockMode.None : CursorLockMode.Locked;
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    public void GameOver(string str)
    {
        Debug.Log("GameOver by "+str);
        UIManager.GameOverPopUp(true);
        CursorVisible(true);
        Time.timeScale = 0;
    }
    public void GameClear()
    {
        Debug.Log("GameClear!");
        UIManager.GameClearPopUp(true);
        CursorVisible(true);
        Time.timeScale = 0;
    }
}
