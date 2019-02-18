using Generic.CustomInput;
using Generic.Singleton;
using System.Collections;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public CameraOption Option;

    public float Height = 25;
    public Camera Cam;
    public Connection Conn;
    public CrossInput CrossInput;

    public Vector2 Direction;
    public float Velocity;

    private void Start()
    {
        if (Conn == null)
            Conn = Singleton.Instance<Connection>();
        if (CrossInput == null)
            CrossInput = Singleton.Instance<CrossInput>();

        SetStartupPosition();
    }

    private void Update()
    {
        float swipe = CrossInput.DeltaSwipe().magnitude;
        if (swipe > 0 && Input.GetMouseButtonDown(0))
        {
            Direction = CrossInput.DeltaSwipe().normalized;
            Velocity = CrossInput.SwipeSpeed > Option.SwipeMaxSpeed ? Option.SwipeMaxSpeed : CrossInput.SwipeSpeed;

        }
        Vector3 vel = new Vector3(Direction.x, 0, Direction.y) * Velocity * Time.deltaTime;
        Velocity -= Option.SwipeDecelerate * Time.deltaTime;
        Cam.transform.position += vel;

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
        if (Physics.Raycast(
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
