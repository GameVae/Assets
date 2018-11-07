
using UnityEngine;

public class HexCell
{
    public int X { get; set; }
    public int Y { get; set; }
    public HexCell Parent { get; set; }

    public bool IsUsing { get; set; }

    public float F { get { return G + H; } }
    public float G { get; set; }
    public float H { get; set; }

    public HexCell() { }

}

public class PoolHexCell
{
    public static PoolHexCell Instance = new PoolHexCell();

    private const int MaxSize = 500;
    private HexCell[] pool;

    private PoolHexCell()
    {
        pool = new HexCell[MaxSize];
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = new HexCell();
            Reset(pool[i]);
        }
    }

    public HexCell GetCell(int initX, int initY)
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].IsUsing)
            {
                pool[i].X = initX;
                pool[i].Y = initY;
                pool[i].IsUsing = true;
                return pool[i];
            }
        }
        return null;
    }

    public void ReturnPool(HexCell cell) { Reset(cell); }

    public void Reset(HexCell cell)
    {
        cell.X = -1;
        cell.Y = -1;
        cell.Parent = null;
        cell.G = float.MaxValue / 2;
        cell.H = float.MaxValue / 2;
        cell.IsUsing = false;
    }

    public void ResetAll()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i].IsUsing)
            {
                Reset(pool[i]);
            }
        }
    }
}
