using UnityEngine;
using System.Collections.Generic;

public class DrawingController : MonoBehaviour
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private LineRenderer fillLinePrefab = null;

    [Header("Settings")]
    [SerializeField] private float snapDistance = 0.5f; // How close to a point to start
    [SerializeField] private float reachDistance = 0.1f; // How close to the next point to lock it in
    #endregion

    #region Private Variables
    private LineRenderer _currentFillLine = null;
    private List<Vector2> _pathPoints = new List<Vector2>();

    // We need to track where we have been, and where we are right now
    private List<int> _visitedNodeIndices = new List<int>();
    private int _currentNodeIndex = -1;
    private bool _isDrawing = false;
    #endregion

    #region Public API
    public void SetupTracing(List<Vector2> points)
    {
        _pathPoints = points;
        ResetDrawing();
    }
    #endregion

    #region Logic
    private void Update()
    {
        if (_pathPoints == null || _pathPoints.Count < 2) return;

        Vector2 mousePos = GetMouseWorldPos();

        // 1. TOUCH DOWN: Find any node to start from
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing(mousePos);
        }

        // 2. HOLD & DRAG: Extend the line towards connected nodes
        if (_isDrawing && Input.GetMouseButton(0))
        {
            UpdateDrawing(mousePos);
        }

        // 3. RELEASE: Check if we won, otherwise destroy the line
        if (Input.GetMouseButtonUp(0) && _isDrawing)
        {
            if (_visitedNodeIndices.Count >= _pathPoints.Count)
            {
                // We completed the shape!
                _isDrawing = false;
                GameManager.instance.ChangeState(GameState.LevelComplete);
            }
            else
            {
                // We let go too early. Vanish!
                ResetDrawing();
            }
        }
    }

    private void StartDrawing(Vector2 mousePos)
    {
        // Find the closest point in the entire shape to start at
        float closestDist = float.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < _pathPoints.Count; i++)
        {
            float dist = Vector2.Distance(mousePos, _pathPoints[i]);
            if (dist < closestDist && dist <= snapDistance)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        // If we found a valid starting point
        if (closestIndex != -1)
        {
            _isDrawing = true;
            _currentNodeIndex = closestIndex;

            _visitedNodeIndices.Clear();
            _visitedNodeIndices.Add(_currentNodeIndex);

            if (_currentFillLine != null) Destroy(_currentFillLine.gameObject);

            _currentFillLine = Instantiate(fillLinePrefab, transform);
            _currentFillLine.positionCount = 2; // Node 1 and the mouse-following tip
            _currentFillLine.SetPosition(0, _pathPoints[_currentNodeIndex]);
            _currentFillLine.SetPosition(1, _pathPoints[_currentNodeIndex]);
        }
    }

    private void UpdateDrawing(Vector2 mousePos)
    {
        // Get valid connected nodes (the one before, and the one after)
        List<int> validNeighbors = GetUnvisitedNeighbors(_currentNodeIndex);

        if (validNeighbors.Count == 0)
        {
            // We are trapped! Just keep the tip at the current node.
            _currentFillLine.SetPosition(_currentFillLine.positionCount - 1, _pathPoints[_currentNodeIndex]);
            return;
        }

        // Figure out which neighbor the mouse is trying to move towards
        int bestNeighbor = -1;
        Vector2 bestProjection = _pathPoints[_currentNodeIndex];
        float minDistanceToMouse = float.MaxValue;

        foreach (int neighbor in validNeighbors)
        {
            Vector2 projection = GetClosestPointOnSegment(_pathPoints[_currentNodeIndex], _pathPoints[neighbor], mousePos);
            float distToMouse = Vector2.Distance(projection, mousePos);

            if (distToMouse < minDistanceToMouse)
            {
                minDistanceToMouse = distToMouse;
                bestNeighbor = neighbor;
                bestProjection = projection;
            }
        }

        // Move the visible line tip towards the best neighbor
        _currentFillLine.SetPosition(_currentFillLine.positionCount - 1, bestProjection);

        // Did we physically reach the neighbor?
        if (Vector2.Distance(bestProjection, _pathPoints[bestNeighbor]) < reachDistance)
        {
            // Lock it in!
            _currentNodeIndex = bestNeighbor;
            _visitedNodeIndices.Add(_currentNodeIndex);

            // Snap line perfectly to the node
            _currentFillLine.SetPosition(_currentFillLine.positionCount - 1, _pathPoints[_currentNodeIndex]);

            // Add a new point to the LineRenderer for the next segment we will draw
            if (_visitedNodeIndices.Count < _pathPoints.Count)
            {
                _currentFillLine.positionCount++;
                _currentFillLine.SetPosition(_currentFillLine.positionCount - 1, _pathPoints[_currentNodeIndex]);
            }
            else
            {
                // Trigger win immediately if we hit the last node
                _isDrawing = false;
                GameManager.instance.ChangeState(GameState.LevelComplete);
            }
        }
    }

    private List<int> GetUnvisitedNeighbors(int currentIndex)
    {
        List<int> neighbors = new List<int>();

        // Check node behind us
        if (currentIndex - 1 >= 0 && !_visitedNodeIndices.Contains(currentIndex - 1))
            neighbors.Add(currentIndex - 1);

        // Check node ahead of us
        if (currentIndex + 1 < _pathPoints.Count && !_visitedNodeIndices.Contains(currentIndex + 1))
            neighbors.Add(currentIndex + 1);

        return neighbors;
    }

    private void ResetDrawing()
    {
        _isDrawing = false;
        _currentNodeIndex = -1;
        _visitedNodeIndices.Clear();
        if (_currentFillLine != null) Destroy(_currentFillLine.gameObject);
    }

    private Vector2 GetClosestPointOnSegment(Vector2 a, Vector2 b, Vector2 p)
    {
        Vector2 ap = p - a;
        Vector2 ab = b - a;
        float t = Vector2.Dot(ap, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        return a + ab * t;
    }

    private Vector2 GetMouseWorldPos()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Camera.main != null ? Mathf.Abs(Camera.main.transform.position.z) : 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return new Vector2(worldPos.x, worldPos.y);
    }
    #endregion
}