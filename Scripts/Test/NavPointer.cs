using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPointer : MonoBehaviour
{  
    private HexMap HexMap;
    private AStartAlgorithm AStarCalculator;
    private List<Vector3Int> path;

    public float        Speed;
    public Camera       CameraRaycaster;
    public Vector3Int   StartPos;
    public Vector3Int   EndPos;

    public bool IsMoving { get; private set; }

    private void Start()
    {
        HexMap = HexMap.Instance;
        AStarCalculator = AStartAlgorithm.Instance;       
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
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

                EndPos      = selectCell;
                StartPos    = HexMap.WorldToCell(transform.position);
                IsMoving    = AStarCalculator.FindPath(StartPos, EndPos);
                path        = AStarCalculator.Path;
            }
        }

        MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (IsMoving)
        {
            if (path.Count > 0)
            {
                Vector3 currentTarget = HexMap.CellToWorld(path[path.Count - 1]);
                transform.position = Vector3.MoveTowards(
                    current: transform.position,
                    target: currentTarget,
                    maxDistanceDelta: Time.deltaTime * Speed);
                if ((transform.position - currentTarget).magnitude <= 0.1f)
                {
                    path.RemoveAt(path.Count - 1);
                }
            }
            else
            {
                IsMoving = false;
            }
        }
    }
}
