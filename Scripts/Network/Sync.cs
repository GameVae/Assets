

namespace Network.Sync
{
    public sealed class Sync
    {
        public static Sync Instance
        {
            get { return instance ?? (instance = new Sync()); }
        }

        private static Sync instance;
        private volatile int mainBaseLevel;
        private volatile float upgradeRemainTime;
        private volatile float researchRemainTime;

        public int MainBaseLevel
        {
            get { return mainBaseLevel; }
            private set { mainBaseLevel = value; }
        }

        public float UpgradeRemainTime
        {
            get { return upgradeRemainTime; }
            private set { upgradeRemainTime = value; }
        }

        public float ResearchRemainTime
        {
            get { return researchRemainTime; }
            private set { researchRemainTime = value; }
        }

        public int InfantryLevel { get; set; }
        
        public void Init()
        {
            MainBaseLevel = 10;
            ResearchRemainTime = 1000;
            InfantryLevel = 10;
        }
    }
}