using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SQLiteConnectProvider;

[CreateAssetMenu(fileName = "Local Link",menuName = "DataTable/LocalLink",order = 0)]
public class SQLiteLocalLink : ScriptableObject
{
    [SerializeField] private List<Link> links;
    public List<Link> Links
    {
        get { return links ?? (links = new List<Link>()); }
    }

    public string this[SQLiteLinkType link]
    {
        get
        {
            return Links.FirstOrDefault(l => l.LocalLink == link)?.DBPath;
        }
    }
}
