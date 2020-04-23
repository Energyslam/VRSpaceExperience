using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveHandler : MonoBehaviour
{
    static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * t;
    }

    static Vector3 QuadraticVector3Curve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p0 = Lerp(a, b, t);
        Vector3 p1 = Lerp(b, c, t);
        return Lerp(p0, p1, t);
    }

    static Vector3 CubicVector3Curve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 p0 = QuadraticVector3Curve(a, b, c, t);
        Vector3 p1 = QuadraticVector3Curve(b, c, d, t);
        return Lerp(p0, p1, t);
    }

    public static List<Vector3> GenerateBezierCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int AmountOfNodes)
    {
        List<Vector3> positions = new List<Vector3>();
        for(int i = 0; i < AmountOfNodes; i++)
        {
            positions.Add(CubicVector3Curve(a, b, c, d, (float) i / (float)AmountOfNodes));
        }
        return positions;
    }

    public static List<Vector3> GenerateQuadraticCurve(Vector3 a, Vector3 b, Vector3 c, int AmountOfNodes)
    {
        List<Vector3> positions = new List<Vector3>();
        for(int i = 0; i < AmountOfNodes; i++)
        {
            positions.Add(QuadraticVector3Curve(a, b, c, (float)i / (float)AmountOfNodes));
        }
        return positions;
    }
}
