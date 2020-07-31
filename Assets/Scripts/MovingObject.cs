using System.Collections;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = .1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidbody2D;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xdir, int ydir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xdir, ydir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    // Main 
    protected virtual void AttemtMove<T>(int xdir, int ydir)
        where T : Component
    {        
        bool canMove = Move(xdir, ydir, out var hit2D);

        if (hit2D.transform == null) return;

        T hitComponent = hit2D.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainDstns = (transform.position - end).sqrMagnitude;
        while (sqrRemainDstns > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(rigidbody2D.position, end, inverseMoveTime * Time.deltaTime);
            rigidbody2D.MovePosition(newPos);
            sqrRemainDstns = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;

    // Start is called before the first frame update
    void Awake()
    {

    }

}
