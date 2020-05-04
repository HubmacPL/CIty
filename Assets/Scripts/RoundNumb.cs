using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoundNumb
{
    public static float RoundToMiddle(float number)
    {
        int whole = (int)number;

        if(number >= 0)
        {
            return whole + 0.5f;
        }
        else
        {
            return whole - 0.5f;
        }
    }

}
