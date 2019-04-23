using System;
using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New BaseInfo Table", menuName = "DataTable/JsonTable/BaseInfo JSONTable", order = 2)]
    public sealed class JSONTable_BaseInfo : JSONTable<BaseInfoRow>
    {
        protected override void Add(BaseInfoRow obj)
        {
            base.Add(obj);            
            obj.Initalize();
        }
    }
}

