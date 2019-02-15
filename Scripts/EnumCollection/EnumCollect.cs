using CustomAttr;
using UnityEngine;

namespace EnumCollect
{
    public enum EnumLanguage
    {
        Vietnamese,
        English,
    }
    public enum EnumCameraType
    {
        Zoom,
        Rotate,
    }

    public enum ListUpgrade
    {
        [Upgrade] MainBase = 1,
        [Upgrade] Farm = 2,
        FarmHarvesting = 3,
        FarmGathering = 4,
        [Upgrade] Wood = 5,
        WoodHarvesting = 6,
        WoodGathering = 7,
        [Upgrade] Stone = 8,
        StoneHarvesting = 9,
        StoneGathering = 10,
        [Upgrade] Metal = 11,
        MetalHarvesting = 12,
        MetalGathering = 13,
        [Upgrade] Storage = 14,
        [Upgrade] Infantry = 15,
        [Upgrade] Solider = 16,
        [Upgrade] TraninedSolider = 17,
        [Upgrade] ForbiddenGuard = 18,
        [Upgrade] Heroic = 19,
        [Upgrade] Ranged = 20,
        [Upgrade] Slingshot = 21,
        [Upgrade] Sharpshooter = 22,
        [Upgrade] Crossbow = 23,
        [Upgrade] Bomber = 24,
        [Upgrade] Mounted = 25,
        [Upgrade] Buffaloman = 26,
        [Upgrade] Horseman = 27,
        [Upgrade] WarElephant = 28,
        [Upgrade] WarStormer = 29,
        [Upgrade] SiegeEngine = 30,
        [Upgrade] Ballista = 31,
        [Upgrade] JavalinBallista = 32,
        [Upgrade] StoneThrower = 33,
        [Upgrade] WarDestroyer = 34,
        [Upgrade] Market = 35,
        MarketTime = 36,
        MarketCost = 37,
        MarketTransport = 38,
        MarketQuality = 39,
        [Upgrade] PhoHienShip = 40,
        RefreshTime = 41,
        SpecialTrade = 42,
        [Upgrade] Embassy = 43,
        [Upgrade] Infirmary = 44,
        [Upgrade] Shelter = 45,
        [Upgrade] ArrowTower = 46,
        [Upgrade] BaseWall = 47,
    }

    public enum DBType
    {
        TrainningCost,
    }

    public enum AnimState
    {
        Walking = 1,
        Dead,
        Attack1,
        Attack2
    }
}
