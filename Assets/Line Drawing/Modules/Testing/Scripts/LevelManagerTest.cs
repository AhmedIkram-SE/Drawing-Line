using UnityEngine;
using System.Collections.Generic;

public class LevelManagerTest : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private LineRenderer backgroundLinePrefab = null;

    [Header("Recorder Settings (Editor Only)")]
    public bool isRecording = false;
    [SerializeField] private float minDistanceBetweenPoints = 0.1f;
    #endregion

    #region Private Variables
    private Transform _activeContainer = null;
    private List<Vector2> _recordedPoints = new List<Vector2>();
    private LineRenderer _previewLine = null;
    #endregion

    #region Logic
    public void GenerateLevelShape(LevelShapeConfiguration config)
    {
        if (_activeContainer == null)
        {
            GameObject containerObj = new GameObject("Runtime_Level_Container");
            containerObj.transform.SetParent(this.transform);
            _activeContainer = containerObj.transform;
        }
        else
        {
            foreach (Transform child in _activeContainer)
            {
                Destroy(child.gameObject);
            }
        }

        if (config.shapePoints.Count < 2) return;

        LineRenderer ghostLine = Instantiate(backgroundLinePrefab, _activeContainer);
        ghostLine.startWidth = config.lineThickness;
        ghostLine.endWidth = config.lineThickness;

        ghostLine.positionCount = config.shapePoints.Count;
        for (int i = 0; i < config.shapePoints.Count; i++)
        {
            ghostLine.SetPosition(i, config.shapePoints[i]);
        }
    }
    #endregion

    #region Recorder Logic
#if UNITY_EDITOR
    private void Update()
    {
        if (!isRecording) return;

        // When mouse is pressed, start/continue recording
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_recordedPoints.Count == 0 || Vector2.Distance(_recordedPoints[_recordedPoints.Count - 1], mousePos) > minDistanceBetweenPoints)
            {
                _recordedPoints.Add(mousePos);
                UpdatePreviewLine();
            }
        }

        // When space is pressed, clear the current drawing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _recordedPoints.Clear();
            if (_previewLine != null) _previewLine.positionCount = 0;
            Debug.Log("Recording Cleared!");
        }
    }

    private void UpdatePreviewLine()
    {
        if (_previewLine == null)
        {
            _previewLine = Instantiate(backgroundLinePrefab, transform);
            _previewLine.name = "Recording_Preview";
        }

        _previewLine.positionCount = _recordedPoints.Count;
        for (int i = 0; i < _recordedPoints.Count; i++)
        {
            _previewLine.SetPosition(i, _recordedPoints[i]);
        }
    }

    // You can see these points in the Console and copy them into your GameManager
    [ContextMenu("Log Recorded Points")]
    public void LogPoints()
    {
        string result = "Copy these into your ShapePoints list:\n";
        foreach (Vector2 p in _recordedPoints)
        {
            result += $"({p.x:F2}, {p.y:F2}), ";
        }
        Debug.Log(result);
    }
#endif
    #endregion
}