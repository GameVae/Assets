using DataTable.Row;
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

    public Vector3Int Position { get; private set; }

    public void OpenPopup(Popup popupIns)
    {
        RssType type = (RssType)Data.RssType;
        string general = string.Format("{0}: Lv {1}", type.ToString(), Data.Level);

        popupIns.Open(general, Data.Quality.ToString(), Data.Position);
        popupIns.SetCursorText(Position);
    }

    public void Initalize(int id,ResourceManager manager)
    {
        Id = id;
        Data = manager.RSSPositionTable.Rows[id - 1];
            
        if (Data != null)
        {
            rss = transform.GetChild(0).gameObject;
            rss?.SetActive(true);

            flag = transform.GetChild(1).gameObject;
            flag?.SetActive(true);

            // parse position
            Position = Data.Position.Parse3Int().ToClientPosition();

            transform.position = manager.MapIns.CellToWorld(Position.ToClientPosition());

            AddLookAtComponent();
        }
    }

    private void AddLookAtComponent()
    {
        LookAt look = gameObject.AddComponent<LookAt>();
        look.GameObject = flag.transform;
        look.Target = Camera.main.transform;
        look.ProjectionDir = ProjectionDir.Right;
    }
}
