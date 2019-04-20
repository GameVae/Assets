using Generic.Pooling;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ConstructIcon : MonoBehaviour, IPoolable
{
    private List<Vector3Int> holdingPositions;
    private RectTransform rectTrans;
    private Rect rect;
    private Vector3Int center;

    private RectTransform RectTransform
    {
        get { return rectTrans ?? (rectTrans = transform as RectTransform); }
    }

    public Rect Rect
    {
        get
        {
            rect.size = RectTransform.Size();
            rect.position = (Vector2)RectTransform.localPosition - (RectTransform.Size() / 2.0f);
            return rect;
        }
    }

    public ReadOnlyCollection<Vector3Int> HoldingPositions
    {
        get
        {
            return holdingPositions?.AsReadOnly();
        }
    }
    public Vector3Int Center
    {
        get
        {
            return center;
        }
    }

    public int ManagedId { get; private set; }

    public void SetHoldingPositions(RangeWayPointManager wayPointManager, Vector3Int constructCenter)
    {
        center = constructCenter;
        holdingPositions = wayPointManager.GetSiblings(constructCenter);
    }

    public void FirstSetup(int insId)
    {
        ManagedId = insId;
    }

    public void Dispose()
    {
        center = Vector3Int.one * -1;
        holdingPositions?.Clear();
        gameObject.SetActive(false);
    }

    public bool IsPointerOver(Vector2 localPos)
    {
        return Rect.Contains(localPos, true);
    }
}
