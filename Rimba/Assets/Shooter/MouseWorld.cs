using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseWorld
{
    public static Vector3 GetPosition()
    {
        Vector3 vec = GetPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetPositionWithZ()
    {
        return GetPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetPositionWithZ(Camera worldCamera)
    {
        return GetPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
