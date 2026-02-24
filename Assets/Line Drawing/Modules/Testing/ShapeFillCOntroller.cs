using UnityEngine;
using System.Collections.Generic;

public class ShapeFillController : MonoBehaviour
{
    #region Variables
    [SerializeField] private LineRenderer _fillLine; // The Blue "Top" line
    [SerializeField] private Vector2[] _shapePoints; // The corners of our shape

    private int _targetIndex = 1; // We start by moving toward the second point
    private bool _isDrawing = false;
    #endregion

    private void Update()
    {
        // 1. Start if mouse is near the first point
        if (Input.GetMouseButtonDown(0))
        {
            float dist = Vector2.Distance(GetMouseWorldPos(), _shapePoints[0]);
            if (dist < 0.5f) _isDrawing = true;
        }

        // 2. If drawing, "Stretch" the line toward the target corner
        if (_isDrawing && Input.GetMouseButton(0))
        {
            TraceShape();
        }

        // 3. Stop if we let go
        if (Input.GetMouseButtonUp(0))
        {
            _isDrawing = false;
            ResetFill();
        }
    }

    private void TraceShape()
    {
        Vector2 mousePos = GetMouseWorldPos();
        Vector2 start = _shapePoints[_targetIndex - 1];
        Vector2 end = _shapePoints[_targetIndex];

        // This is the "Snap" logic: Find the point on the line closest to the mouse
        Vector2 pathPoint = GetClosestPointOnLine(start, end, mousePos);

        // Update the visual blue line
        _fillLine.positionCount = _targetIndex + 1;
        _fillLine.SetPosition(_targetIndex, pathPoint);

        // If we reach the corner, move to the next target
        if (Vector2.Distance(pathPoint, end) < 0.1f && _targetIndex < _shapePoints.Length - 1)
        {
            _targetIndex++;
        }
    }

    private Vector2 GetMouseWorldPos() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void ResetFill()
    {
        _fillLine.positionCount = 0;
        _targetIndex = 1;
    }

    // Mathematical magic to keep the line "Inside" the shape
    private Vector2 GetClosestPointOnLine(Vector2 a, Vector2 b, Vector2 p)
    {
        Vector2 v = b - a;
        float t = Vector2.Dot(p - a, v) / v.sqrMagnitude;
        return a + v * Mathf.Clamp01(t);
    }
}