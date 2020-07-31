using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemtMove<T>(int xdir, int ydir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemtMove<T>(xdir, ydir);
        skipMove = true;
    }

    protected void MoveEnemy()
    {
        int xdir = 0;
        int ydir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            ydir = target.position.y > transform.position.y ? 1 : -1;
        else
            xdir = target.position.x > transform.position.x ? 1 : -1;

        AttemtMove<Player>(xdir, ydir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        hitPlayer.LoseFood(playerDamage);
    }
}
