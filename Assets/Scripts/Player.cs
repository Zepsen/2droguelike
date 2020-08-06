using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int wallDmg = 1;
    public int pointsPerFood = 10;
    public int pointsPerSode = 20;
    public float restartDelay = 1f;
    public Text foodText;

    private Animator animator;
    private int food;
    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        foodText = GameObject.Find("FoodText").GetComponent<Text>();

        food = GameManager.instance.playerFoodPoints;
        foodText.text = $"Food: {food}";

        base.Start();
    }

    void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    public void CheckIfGameOver()
    {
        if (food <= 0)
            GameManager.instance.GameOver();
    }

    protected override void AttemtMove<T>(int xdir, int ydir)
    {
        food--;
        foodText.text = $"Food: {food}";

        base.AttemtMove<T>(xdir, ydir);
        RaycastHit2D hit2D;

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitwall = component as Wall;
        hitwall.DamageWall(wallDmg);
        animator.SetTrigger("playerChop");
    }

    private void Restart() =>
        Application.LoadLevel(Application.loadedLevel);

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = $"-{pointsPerFood} Food: {food}";
        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch(other.tag) {
            case "Exit": 
                Invoke("Restart", restartDelay);
                enabled = false;
                break;
            case "Food":
                food += pointsPerFood;
                foodText.text = $"+{pointsPerFood} Food: {food}";
                other.gameObject.SetActive(false);
                break;
            case "Soda":
                food += pointsPerSode;
                foodText.text = $"+{pointsPerSode} Food: {food}";
                other.gameObject.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            AttemtMove<Wall>(horizontal, vertical);
        }

    }


}
