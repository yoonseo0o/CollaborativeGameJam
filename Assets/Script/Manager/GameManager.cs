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
    public CutScene CutScene;
    [Header("Data")]
    public FlashData flashData;

    [Header("")]
    public MonsterSpawner MonsterSpawner;
    public Transform playerTrf;
    public Transform lanternTrf;
    [SerializeField] private bool IsMonsterSpawn;
    public int Difficulty { get; private set; }
    

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
        Difficulty = 1;
    }
    public void ShowStartCutScene()
    {
        CutScene.ActiveImg(true);
    }
    public void GameStart()
    {
        CursorVisible(false);
        Time.timeScale = 1;
        if (IsMonsterSpawn) MonsterSpawner.StartSpawn();
    }
    public void ReStart()
    {    // GameManager 직접 파괴
        Destroy(GameManager.Instance.gameObject);

        // 씬 다시 불러오기 (예: 0번 씬)
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
    public void SetDifficulty(int difficulty)
    {
        if (difficulty < 0 && difficulty > 5) return;
        Difficulty = difficulty;
    } 
}
