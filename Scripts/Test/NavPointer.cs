using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPointer : MonoBehaviour
{
    private int selectCount;

    public HexMap HexMap;
    public Camera CameraRaycaster;

    public Vector3Int StartPos;
    public Vector3Int EndPos;

    public AStartAlgorithm AStarCalculator;
    private void Start()
    {
        AStarCalculator = AStartAlgorithm.Instance;
    }
    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;

            bool raycastHitted = Physics.Raycast(
                CameraRaycaster.ScreenPointToRay(mousePos),
                out RaycastHit hitInfo,
                int.MaxValue);

            if (raycastHitted)
            {
                Vector3Int selectCell = HexMap.WorldToCell(hitInfo.point);
                if (!HexMap.IsValidCell(selectCell.x, selectCell.y)) return;
                if (selectCount < 1)
                {
                    StartPos = selectCell;
                    selectCount++;
                }
                else
                {
                    EndPos = selectCell;
                    AStarCalculator.FindPath(StartPos, EndPos);

                }
            }
        }
    }
}
