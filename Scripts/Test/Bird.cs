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

    private QuadInfo info;
    private Pooling<Bird> pool;

    public SpriteRenderer SpriteRenderer;

    private void Update()
    {
        if (isAlive)
        {
            if (info.IsOutside(this))
            {
                pool.Release(this);
            }
            else
            {
                transform.Translate(direction * speed * Time.deltaTime , Space.World);
            }
        }
    }

    public void LetStart(QuadInfo quadInfo)
    {
        info = quadInfo;

        bool startAtLeft = Random.Range(0, 2) != 0;
        float height = Random.Range(0.2f, 5f);

        Vector3 position = startAtLeft ? quadInfo.EndOfLeft.position : quadInfo.EndOfRight.position;
        position.y = height;
      
        isAlive = true;
        direction = Vector3.zero;

        if ((quadInfo.Side & QuadInfo.Left) != 0)
        {           
            direction.z = 1;
        }

        if (!startAtLeft)
        {
            Flip();
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

