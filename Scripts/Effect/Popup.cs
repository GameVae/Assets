using Generic.Singleton;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class Popup : MonoSingle<Popup>
{
    public CursorPos Cursor;
    public GameObject Panel;
    public Text QualityText;
    public Text CoordText;
    public Text GeneralText;

    private bool enable;

    protected override void Awake()
    {
        base.Awake();
        Panel.gameObject.SetActive(false);
    }

    public void Open(string generalInfo, string quality, string coord)
    {
        if (!enable) return;
        GeneralText.text = generalInfo;
        QualityText.text = quality;
        CoordText.text = coord;
        Panel.gameObject.SetActive(true);
    }

    public void OnOffSwitch()
    {
        enable = !enable;
    }

    public void SetCursor(Vector3Int cell)
    {
        Cursor.updateCursor(Singleton.Instance<HexMap>().CellToWorld(cell));
        Cursor.PositionCursor.SetPosTxt((cell.x).ToString(), (cell.y).ToString());
    }
}
