using UnityEngine;
using System.Collections.Generic;

public class LineController : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _linePrefab = null; // The blueprint for our line
    [SerializeField] private float _minDistance = 0.1f;    // Min move before adding a point

    private LineRenderer _currentLine = null;              // Reference to the active line
    private List<Vector2> _points = new List<Vector2>();   // List of captured touch points
    #endregion

    #region Unity Methods
    private void Update()
    {
        // 1. Detect Initial Touch
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewLine();
        }

        // 2. Detect Dragging
        if (Input.GetMouseButton(0) && _currentLine != null)
        {
            UpdateLine();
        }

        // 3. Detect Release
        if (Input.GetMouseButtonUp(0))
        {
            EraseLine();
        }
    }
    #endregion

    #region Logic
    private void CreateNewLine()
    {
        // Instantiate the prefab and get its LineRenderer component
        GameObject newLineObj = Instantiate(_linePrefab, transform);
        _currentLine = newLineObj.GetComponent<LineRenderer>();

        // Clear previous points and add the first touch position
        _points.Clear();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        AddPoint(mousePos);
    }

    private void UpdateLine()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Only add a point if the finger has moved far enough (Optimization)
        if (Vector2.Distance(mousePos, _points[_points.Count - 1]) > _minDistance)
        {
            AddPoint(mousePos);
        }
    }

    private void AddPoint(Vector2 point)
    {
        _points.Add(point);

        // Update the LineRenderer visual positions
        _currentLine.positionCount = _points.Count;
        _currentLine.SetPosition(_points.Count - 1, point);
    }

    private void EraseLine()
    {
        if (_currentLine != null)
        {
            Destroy(_currentLine.gameObject);
            _currentLine = null;
            _points.Clear();
        }
    }
    #endregion
}