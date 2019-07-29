using UnityEngine;
using Generic.Pooling;
using static MapBound;

public class Bird : MonoBehaviour, IPoolable
{
    public int ManagedId
    {
        get;
        private set;
    }

    private float speed = 1.5f;
    private bool isAlive;
    private Vector3 direction;

    private QuadInfo quad;
    private Pooling<Bird> pool;

    public SpriteRenderer SpriteRenderer;

    private void Update()
    {
        if (isAlive)
        {
            if (quad.IsOutside(this))
            {
                pool.Release(this);
            }
            else
            {
                transform.Translate(direction * speed * Time.deltaTime , Space.World);
            }
        }
    }

    public void LetStart(QuadInfo quad)
    {
        this.quad = quad;

        Vector3 pos = quad.Quad.transform.position;

        pos.y = Random.Range(0.2f, 4.0f);
        pos.z -= 20;

        transform.position = pos;

        isAlive = true;
        direction = Vector3.zero;

        if ((quad.Side & QuadInfo.Left) != 0)
        {           
            direction.z = 1;
        }

        gameObject.SetActive(true);
    }

    public void Flip()
    {
        direction *= -1;
        SpriteRenderer.flipX = !SpriteRenderer.flipX;
    }

    public void Dispose()
    {
        isAlive = false;
        gameObject.SetActive(false);
    }

    public void SetPool(Pooling<Bird> pooling)
    {
        pool = pooling;
    }

    public void FirstSetup(int insId)
    {
        ManagedId = insId;
    }
}

