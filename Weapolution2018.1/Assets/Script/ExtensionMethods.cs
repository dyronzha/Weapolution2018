using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {
    public static void SetVectorX(this Transform trans, float Xvalue) {
        trans.position = new Vector3(Xvalue, trans.position.y, trans.position.z); 
    }
    public static void SetVectorY(this Transform trans, float Yvalue)
    {
        trans.position = new Vector3( trans.position.x, Yvalue, trans.position.z);
    }
    public static void SetVectorZ(this Transform trans, float Zvalue)
    {
        trans.position = new Vector3(trans.position.x, trans.position.y, Zvalue);
    }

    public static void SetScaleX(this Transform trans, float Xvalue) {
        trans.localScale = new Vector3(Xvalue, trans.localScale.y, trans.localScale.z);
    }

    public static Vector3 V3NormalizedtoV2(this Vector3 v3) {
        Vector2 v2 = new Vector2(v3.x, v3.y).normalized;
        v3 = new Vector3(v2.x, v2.y, 0.0f);
        return v3;
    }

    public static void SetPosV2(this Transform trans, Vector2 value)
    {
        trans.position = new Vector3(value.x, value.y, trans.position.z);
    }


}
