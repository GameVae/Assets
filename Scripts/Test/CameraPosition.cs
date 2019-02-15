using Generic.Singleton;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public float Height = 25;
    public Camera Cam;
    public Connection Conn;

    private void Start()
    {
        if (Conn == null)
            Conn = Singleton.Instance<Connection>();

        SetStartupPosition();
    }

    /// <summary>
    /// Cell in Real map 522 - 522
    /// </summary>
    /// <param name="cell"></param>
    public void Set(Vector3Int cell)
    {
        Vector3 worldPoint = Singleton.Instance<HexMap>().CellToWorld(cell);
        worldPoint.y = Height;         // const height
        Cam.transform.position = worldPoint;

        AlignCamera(worldPoint);
    }

    private void SetStartupPosition()
    {
        Vector3Int cellIndex = Conn.Sync.CurrentMainBase.Position.Parse3Int() + new Vector3Int(5, 5, 0);
        Set(cellIndex);
    }

    /// <summary>
    /// </summary>
    /// <param name="projection">Z of camera </param>
    /// <returns></returns>
    private float HaftCrossLineViewFustum(float projection)
    {
        if(Physics.Raycast(
            origin: Cam.transform.position,
            direction: Cam.transform.forward,
            maxDistance: Cam.farClipPlane,
            hitInfo: out RaycastHit hitInfo))
        {
            return hitInfo.point.z - projection;
        }
        return 0;
    }

    private void AlignCamera(Vector3 worldPoint)
    {
        worldPoint.z -= HaftCrossLineViewFustum(worldPoint.z);
        Cam.transform.position = worldPoint;
    }
}
