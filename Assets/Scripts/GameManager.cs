using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager board;
    
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

    private void InitGame()
    {
        board.SetupScene(level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
