using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections.Generic;

[RequireComponent(typeof(SplineContainer))]
public class SplineBaker : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How many points to generate. Higher = smoother but more data.")]
    public int resolution = 50;

    [Header("Output")]
    [Tooltip("The generated points will appear here.")]
    public List<Vector2> bakedPoints = new List<Vector2>();

    [ContextMenu("1. Bake Spline to Points")]
    public void BakePoints()
    {
        SplineContainer spline = GetComponent<SplineContainer>();
        bakedPoints.Clear();

        // Walk along the curve from 0% (0.0) to 100% (1.0)
        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / resolution;

            // Get the mathematical position on the curve
            float3 position = spline.EvaluatePosition(t);

            // Save it as a simple 2D point for our game logic
            bakedPoints.Add(new Vector2(position.x, position.y));
        }

        Debug.Log($"Successfully baked {bakedPoints.Count} points! Ready to copy.");
    }
}