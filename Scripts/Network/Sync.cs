using ManualTable;
using ManualTable.Row;
using UnityEngine;

namespace Network.Sync
{
    [CreateAssetMenu(fileName = @"Sync", menuName = @"Conn/Sync", order = 1)]
    public sealed class Sync : ScriptableObject
    {
        public int User_ID = 0;
        public int CurBaseIndex = 0;

        public UserInfoRow MainUser;

        public BaseUpgradeJSONTable CurrentBaseUpgrade
        {
            get { return BaseUpgrade[CurBaseIndex]; }
        }
        public BaseDefendJSONTable CurrentBaseDefend
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

        public RSS_PositionJSONTable RSS_Position;
        public PositionJSONTable Position;

        public UserInfoJSONTable UserInfos;
        public BaseInfoJSONTable BaseInfos;

        public BaseUpgradeJSONTable[] BaseUpgrade;
        public BaseDefendJSONTable[] BaseDefends;

        public UnitJSONTable UnitTable;
        public BasePlayerJSONTable BasePlayerTable;

        public void SyncUpdate()
        {
            for (int i = 0; i < BaseInfos?.Rows?.Count; i++)
            {
                UpdateBaseInfo(i);
            }
        }

        private void UpdateBaseInfo(int i)
        {
            BaseInfoRow baseInfo = BaseInfos.Rows[i];
            baseInfo.Update(this);      
        }

        public void LoadUserInfo(JSONObject serMsg)
        {
            serMsg[0].GetField(ref User_ID, "ID_User");

            UserInfos.LoadTable(serMsg);
            MainUser = UserInfos.GetUser(User_ID);
        }
    }

}