using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControl : MonoBehaviour
{

    public bool CheckRange1(Vector2 transformCellPosition, Vector2 targetCellPosition)
    {
        bool retCheckBool = false;
        if ((transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
            (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y))
        {
            retCheckBool = true;
        }

        return retCheckBool;
    }

    public bool CheckRange2(Vector2 transformCellPosition, Vector2 targetCellPosition)
    {
        bool retCheckBool = false;
        if ((transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
            (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
            (transformCellPosition.x - 2 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
            (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
            (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
            (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
            (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
            (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
            (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y) ||
            (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y))
        {
            retCheckBool = true;
        }

        return retCheckBool;
    }

    public bool CheckRange3(Vector2 transformCellPosition, Vector2 targetCellPosition)
    {
        bool retCheckBool = false;
        if ((transformCellPosition.x - 2 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
            (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
            (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
            (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
            (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
            (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
            (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y) ||
            (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y) ||
            (transformCellPosition.x - 3 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x + 3 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x - 3 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
            (transformCellPosition.x - 2 == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y) ||
            (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y + 3 == targetCellPosition.y) ||
            (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y + 3 == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y + 3 == targetCellPosition.y) ||
            (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y + 3 == targetCellPosition.y) ||
            (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y) ||
            (transformCellPosition.x + 3 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
            (transformCellPosition.x + 3 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
            (transformCellPosition.x + 3 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
            (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
            (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y - 3 == targetCellPosition.y) ||
            (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y - 3 == targetCellPosition.y) ||
            (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y - 3 == targetCellPosition.y) ||
            (transformCellPosition.x - 2 == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
            (transformCellPosition.x - 3 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y))
        {
            retCheckBool = true;
        }

        return retCheckBool;
    }

    //public void Range1(Vector2 transformCellPosition)
    //{
    //    transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
    //        (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
    //        (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
    //        (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
    //        (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
    //        (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
    //        (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y)
    //}

    //public void Range2(Vector2 transformCellPosition)
    //{
    //    (transformCellPosition.x - 2 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
    //       (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
    //       (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
    //       (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
    //       (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
    //       (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
    //       (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
    //       (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
    //       (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
    //       (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y) ||
    //       (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y) ||
    //       (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y))
    //}

    //public void Range3(Vector2 transformCellPosition){
    //(transformCellPosition.x - 3 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
    //        (transformCellPosition.x + 3 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
    //        (transformCellPosition.x - 3 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
    //        (transformCellPosition.x - 2 == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y) ||
    //        (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y + 3 == targetCellPosition.y) ||
    //        (transformCellPosition.x == targetCellPosition.x && transformCellPosition.y + 3 == targetCellPosition.y) ||
    //        (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y + 3 == targetCellPosition.y) ||
    //        (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y + 3 == targetCellPosition.y) ||
    //        (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y + 2 == targetCellPosition.y) ||
    //        (transformCellPosition.x + 3 == targetCellPosition.x && transformCellPosition.y + 1 == targetCellPosition.y) ||
    //        (transformCellPosition.x + 3 == targetCellPosition.x && transformCellPosition.y == targetCellPosition.y) ||
    //        (transformCellPosition.x + 3 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y) ||
    //        (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
    //        (transformCellPosition.x + 2 == targetCellPosition.x && transformCellPosition.y - 3 == targetCellPosition.y) ||
    //        (transformCellPosition.x + 1 == targetCellPosition.x && transformCellPosition.y - 3 == targetCellPosition.y) ||
    //        (transformCellPosition.x - 1 == targetCellPosition.x && transformCellPosition.y - 3 == targetCellPosition.y) ||
    //        (transformCellPosition.x - 2 == targetCellPosition.x && transformCellPosition.y - 2 == targetCellPosition.y) ||
    //        (transformCellPosition.x - 3 == targetCellPosition.x && transformCellPosition.y - 1 == targetCellPosition.y))
//}
}
