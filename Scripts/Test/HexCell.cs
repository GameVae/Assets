
using Generic.Singleton;
using UnityEngine;

public class HexCell
{
    public int X            { get; set; }
    public int Y            { get; set; }
    public HexCell Parent   { get; set; }

    public bool IsUsing { get; set; }

    public float F { get { return G + H; } }
    public float G { get; set; }
    public float H { get; set; }

    public HexCell() { }

}

public sealed class PoolHexCell : ISingleton
{
    private const int MaxSize = 1000;

    private HexCell[] pool;

    public int RetainingEmpty { get; private set; }

    private PoolHexCell()
    {
        RetainingEmpty = 0;
        pool = new HexCell[MaxSize];
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = new HexCell();
            Reset(pool[i]);
        }
    }

    public HexCell CreateCell(int initX, int initY)
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].IsUsing)
            {
                RetainingEmpty--;
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
        RetainingEmpty++;
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
