using UnityEngine;

public class Player : MovingObject
{
    public int wallDmg = 1;
    public int pointsPerFood = 10;
    public int pointsPerSode = 20;
    public float restartDelay = 1f;

    private Animator animator;
    private int food;
    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        base.Start();
    }

    void OnDisabled()
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
                other.gameObject.SetActive(false);
                break;
            case "Soda":
                food += pointsPerSode;
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
