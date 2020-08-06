using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float lvlStartDelay = 2f;    

    public static GameManager instance = null;
    public BoardManager board;

    public float turnDelay = .1f;

    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private Text lvlText;
    private GameObject lvlImage;
    private bool doingSetup;

    private int level = 0;
    private List<Enemy> enemies;
    private bool enemiesMoving;

    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    void Awake()
    {
        SceneManager.sceneLoaded += this.OnLevelLoaded;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        enemies = new List<Enemy>();
        board = GetComponent<BoardManager>();
        //InitGame();
    }

    private void OnLevelLoaded (Scene scene, LoadSceneMode sceneMode) 
    {
        level++;
        InitGame();
    }

    public void GameOver()
    {
        lvlText.text = $"After {level} days, you starved";
        lvlImage.SetActive(true);
        enabled = false;
    }

    private void InitGame()
    {
        doingSetup = true;

        lvlImage = GameObject.Find("LevelImage");
        lvlText = GameObject.Find("LevelText").GetComponent<Text>();
        lvlText.text = $"Day {level}";
        lvlImage.SetActive(true);

        Invoke("HideLevelImage", lvlStartDelay);

        enemies.Clear();
        board.SetupScene(level);
    }

    private void HideLevelImage() {
        lvlImage.SetActive(false);
        doingSetup = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesMoving || playersTurn || doingSetup) 
            return;
        
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script) => enemies.Add(script);

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if(enemies.Count == 0)
            yield return new WaitForSeconds(turnDelay);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
