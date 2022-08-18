using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frameout{
/// <summary> アスペクト比を調整するクラス</summary>
public class ScreenAspect
{
    static Camera _camera;

    public static bool Init()
    {
        _camera = Camera.main;
        return _camera != null;
    }

    public static Vector3 VtoW(float x, float y, float z = 0f)
    {
        return _camera.ViewportToWorldPoint(new Vector3(x, y, z));
    }

    public static Vector3 VtoW(Vector3 vec)
    {
        return _camera.ViewportToWorldPoint(vec);
    }

    public static Vector3 WtoV(float x, float y, float z)
    {
        return _camera.WorldToViewportPoint(new Vector3(x, y, z));
    }

    public static Vector3 WtoV(Vector3 vec)
    {
        return _camera.WorldToViewportPoint(vec);
    }
}
}