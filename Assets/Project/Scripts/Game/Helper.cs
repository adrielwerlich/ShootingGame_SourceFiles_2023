using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float GetDistanceBetweenTwoObjects(Vector3 element1, Vector3 element2)
    {
        return Vector3.Distance(element1, element2);
    }
}
