using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracks : MonoBehaviour
{
    static List<Vector3> originToSplit = new List<Vector3>();
    static List<Vector3> splitToA = new List<Vector3>();
    static List<Vector3> splitToB = new List<Vector3>();

    public static List<Vector3> OriginToSplit { get { return originToSplit; } }
    public static List<Vector3> SplitToA { get { return splitToA; } }
    public static List<Vector3> SplitToB { get { return splitToB; } }
    public static void GenerateSplitTracks(Vector3 destinationA, Vector3 destinationB, Vector3 origin)
    {
        ClearTracks();
        Vector3 pointToSplitFrom = origin + ((destinationA - origin) + (destinationB - origin)) * 0.25f;
        Debug.Log("origin = " + origin);
        Vector3 halfwaypointBetweenDestinations = destinationA + (destinationB - destinationA) * 0.5f;

        Vector3 halfwayFromSplitToA = pointToSplitFrom + ((destinationA - pointToSplitFrom) / 2f);
        Vector3 halfwayFromHalfwayAToMiddleOfDestinations = halfwayFromSplitToA + ((halfwaypointBetweenDestinations - halfwayFromSplitToA) / 2f);

        Vector3 halfwayFromSplitToB = pointToSplitFrom + ((destinationB - pointToSplitFrom) / 2f);
        Vector3 halfwayFromHalfwayBToMiddleOfDestinations = halfwayFromSplitToB + ((halfwaypointBetweenDestinations - halfwayFromSplitToB) / 2f);

        originToSplit = GenerateVectorsBetweenTwoPoints(origin, pointToSplitFrom, 10);
        splitToA = BezierCurveHandler.GenerateQuadraticCurve(pointToSplitFrom, halfwayFromHalfwayAToMiddleOfDestinations, destinationA, 20);
        splitToB = BezierCurveHandler.GenerateQuadraticCurve(pointToSplitFrom, halfwayFromHalfwayBToMiddleOfDestinations, destinationB, 20);

    }

    public static List<Vector3> GenerateVectorsBetweenTwoPoints(Vector3 from, Vector3 to, int amountOfVectors)
    {
        Vector3 direction = to - from;
        List<Vector3> vectors = new List<Vector3>();
        for (int i = 0; i < amountOfVectors; i++)
        {
            vectors.Add(from + (direction * ((float)i / (float)amountOfVectors)));
        }
        return vectors;
    }

    static void ClearTracks()
    {
        originToSplit.Clear();
        splitToA.Clear();
        splitToB.Clear();
    }
}
