using Generic.Contants;
using System.Collections.Generic;
using UnityEngine;

public abstract class Range : MonoBehaviour
{
    [SerializeField] private Transform owner; 
    // y round
    protected Vector3Int[] HexaPatternEven1
    {
        get { return GConstants.NeightbourHexCell.HexaPatternEven1; }
    }
    protected Vector3Int[] HexaPatternEven2
    {
       get { return GConstants.NeightbourHexCell.HexaPatternEven2; }
    }
    protected Vector3Int[] HexaPatternEven3
    {
        get { return GConstants.NeightbourHexCell.HexaPatternEven3; }
    }
    // y odd
    protected Vector3Int[] HexaPatternOdd1
    {
        get { return GConstants.NeightbourHexCell.HexaPatternOdd1; }
    }
    protected Vector3Int[] HexaPatternOdd2
    { get { return GConstants.NeightbourHexCell.HexaPatternOdd2; } }
    protected Vector3Int[] HexaPatternOdd3
    {
        get { return GConstants.NeightbourHexCell.HexaPatternOdd3; }
    }

    protected Transform Owner
    {
        get { return owner ?? (owner = gameObject.transform); }
        private set { owner = value; }
    }

    protected Queue<CellInfomation> cellInfors;

    public abstract Queue<CellInfomation> GetInfo();
}
