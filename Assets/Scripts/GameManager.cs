using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager board;

    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private int level = 3;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) {
            instance = this;            
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        board = GetComponent<BoardManager>();
        InitGame();   
    }

    public void GameOver() {
        enabled = false;
    }

    private void InitGame()
    {
        board.SetupScene(level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
