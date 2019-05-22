using DataTable;
using DataTable.Row;
using UnityEngine;

namespace Network.Sync
{
    [CreateAssetMenu(fileName = @"Sync", menuName = @"Conn/Sync", order = 1)]
    public sealed class Sync : ScriptableObject
    {
        public int User_ID = 0;
        public int CurBaseIndex = 0;

        public UserInfoRow MainUser;

        public JSONTable_BaseUpgrade CurrentBaseUpgrade
        {
            get { return BaseUpgrade[CurBaseIndex]; }
        }
        public JSONTable_BaseDefend CurrentBaseDefend
        {
            get { return BaseDefends[CurBaseIndex]; }
        }
        public BaseInfoRow CurrentMainBase
        {
            get { return (BaseInfoRow)BaseInfos[CurBaseIndex]; }
        }
        public BaseUpgradeRow CurrentUpgrade
        {
            get
            {
                return CurrentBaseUpgrade[CurrentMainBase.UpgradeWait_ID];
            }
        }
        public BaseUpgradeRow CurrentResearch
        {
            get
            {
                return CurrentBaseUpgrade[CurrentMainBase.ResearchWait_ID];
            }
        }

        public JSONTable_RSSPosition RSS_Position;
        public JSONTable_Position Position;

        public JSONTable_UserInfo UserInfos;
        public JSONTable_BaseInfo BaseInfos;

        public JSONTable_BaseUpgrade[] BaseUpgrade;
        public JSONTable_BaseDefend[] BaseDefends;

        public JSONTable_Unit UnitTable;
        public JSONTable_BasePlayer BasePlayerTable;

        public JSONTable_Friends FriendTable;

        public void SyncUpdate()
        {
            for (int i = 0; i < BaseInfos.Count; i++)
            {
                UpdateBaseInfo(i);
            }
        }

        private void UpdateBaseInfo(int i)
        {
            BaseInfoRow baseInfo = BaseInfos.ReadOnlyRows[i];
            baseInfo.Update(this);      
        }

        public void LoadUserInfo(JSONObject serMsg)
        {
            serMsg[0].GetField(ref User_ID, "ID_User");

            UserInfos.LoadTable(serMsg);
            MainUser = UserInfos.GetUserById(User_ID);
        }
    }

}