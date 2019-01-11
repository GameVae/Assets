using ManualTable.Row;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New Container", menuName = "SQLiteTable/Container", order = 1)]
    public class TableContainer : ScriptableObject
    {
        [SerializeField] public ScriptableObject Table;
        [SerializeField] public DBRowType RowType;
    }
}