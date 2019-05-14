using DataTable.Row;
using Generic.Pooling;
using UnityEngine;

public enum Flag
{
    Owner = 4,
    Enemy = 5,
    Guild = 6,
}

public enum RssType
{
    Farm = 1,
    Wood,
    Metal,
    Rock,
}

public class NaturalResource : MonoBehaviour, IPoolable
{
    private RSS_PositionRow data;
    private GameObject rss;
    private GameObject flag;

    public int Id
    {
        get
        {
            return Data.ID;
        }
    }
    public int ManagedId
    {
        get;
        private set;
    }
    public Vector3Int Position
    {
        get;
        private set;
    }
    public RSS_PositionRow Data
    {
        get
        {
            return data;
        }
    }

    public void OpenPopup(Popup popupIns)
    {
        RssType type = (RssType)Data.RssType;
        string general = string.Format("{0}: Lv {1}", type.ToString(), Data.Level);

        popupIns.Open(general, Data.Quality.ToString(), Data.Position);
        popupIns.SetCursorText(Position);
    }

    public void SetResourceData(RSS_PositionRow _data, Flag group, Vector3 worldPosition)
    {
        data = _data;
        if (Data != null)
        {
            rss = transform.GetChild(Data.RssType - 1).gameObject;
            rss?.SetActive(true);

            flag = transform.GetChild((int)group).gameObject;
            flag?.SetActive(true);

            // parse position
            Position = Data.Position.Parse3Int().ToClientPosition();

            transform.position = worldPosition;
            LookAtCamera();
        }
    }

    public void FirstSetup(int insId)
    {
        ManagedId = insId;
    }

    public void Dispose()
    {
        data = null;
        rss.SetActive(false);
        flag.SetActive(false);
        Position = Vector3Int.one * -1;
        gameObject.SetActive(false);
    }

    private void LookAtCamera()
    {
        Transform Target = Camera.main.transform;
        flag.transform.LookAt(Vector3.ProjectOnPlane(flag.transform.position, Target.right));
    }
}
