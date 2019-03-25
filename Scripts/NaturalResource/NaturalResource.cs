using Generic.Singleton;
using ManualTable.Row;
using UnityEngine;

public enum Flag
{
    Owner = 0,
    Enemy = 1,
    Guild = 2,
}

public enum RssType
{
    Farm = 1,
    Wood,
    Metal,
    Rock,
}

public class NaturalResource : MonoBehaviour
{
    private GameObject rss;
    private GameObject flag;

    public RSS_PositionRow Data;
    public int Id;

    public Vector3Int CellPos { get; private set; }

    private void Start()
    {
        Data = (RSS_PositionRow)Singleton.Instance<ResourceManager>().Datas[Id - 1];
        Singleton.Instance<ResourceManager>()[Id] = this;

        InitData();

        LookAt look = gameObject.AddComponent<LookAt>();
        look.GameObject = flag.transform;
        look.Target = Camera.main.transform;
        look.ProjectionDir = ProjectionDir.Right;

    }

    private void OnMouseUp()
    {
        RssType type = (RssType)Data.RssType;
        string general = string.Format("{0}: Lv {1}", type.ToString(), Data.Level);

        Singleton.Instance<Popup>().Open(general, Data.Quality.ToString(), Data.Position);
        Singleton.Instance<Popup>().SetCursor(CellPos);

        Debugger.Log("Pointer down");
    }
    public void InitData()
    {
        if (Data != null)
        {
            rss = transform.GetChild(0).gameObject;
            rss?.SetActive(true);

            flag = transform.GetChild(1).gameObject;
            flag?.SetActive(true);

            // parse position
            CellPos = Data.Position.Parse3Int();

            transform.position = Singleton.Instance<HexMap>().CellToWorld(CellPos.ToClientPosition());
        }
    }


}
