using DataTable.Row;
using EnumCollect;
using System.Linq;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New Guild Table", menuName = "DataTable/JsonTable/Guild JSONTable", order = 11)]
    public class JSONTable_GuildInfo : JSONTable<GuildMemberRow>
    {
        public GuildMemberRow FindMember(int userId)
        {
            return ReadOnlyRows.FirstOrDefault(m => m.ID_User == userId);          
        }

        private GuildMemberRow master;
        public GuildMemberRow Master
        {
            get
            {
                return master ?? 
                    (master = ReadOnlyRows.FirstOrDefault(m => m.GuildPosition == GuildPosition.Master));
            }
        }
    }
}