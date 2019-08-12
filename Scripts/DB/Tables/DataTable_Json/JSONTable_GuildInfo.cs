using DataTable.Row;
using System.Linq;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New Guild Table", menuName = "DataTable/JsonTable/Guild JSONTable", order = 11)]
    public class JSONTable_GuildInfo : JSONTable<GuildRow>
    {
        public GuildRow FindByName(string gName)
        {
            return ReadOnlyRows.FirstOrDefault(g => g.GuildName == gName);
        }        
    }
}