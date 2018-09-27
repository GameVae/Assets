using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileMapScripts : MonoBehaviour {
    public BoundsInt AreaTop;
    public BoundsInt AreaLeft;
    public BoundsInt AreaBot;
    public BoundsInt AreaRight;

    Tilemap tilemap;
    Grid grid;
    [SerializeField]
    TileBase tileBase;
    TileBase[] tileArrayTop;
    TileBase[] tileArrayLeft;
    TileBase[] tileArrayBot;
    TileBase[] tileArrayRight;

    
    private void getComponent()
    {
        grid = GetComponentInParent<Grid>();
        tilemap = GetComponent<Tilemap>();
    }
   private void setMapBound()
    {
        setArray(tileArrayTop, AreaTop);
        setArray(tileArrayRight, AreaRight);
        setArray(tileArrayBot, AreaBot);
        setArray(tileArrayLeft, AreaLeft);
    }
    private void setArray(TileBase[] tileBaseArray, BoundsInt Area)
    {
        tileBaseArray = new TileBase[Area.size.x * Area.size.y * Area.size.z];
        for (int i = 0; i < tileBaseArray.Length; i++)
        {
            tileBaseArray[i] = tileBase;
        }
        tilemap.SetTilesBlock(Area, tileBaseArray);
    }

    [ContextMenu("CreateBoundMap")]
    public void SetMap()
    {
        getComponent();
        setMapBound();
    }
    [ContextMenu("ClearMap")]
    public void ClearMap()
    {
        getComponent();
        tilemap.ClearAllTiles();
    }
}
