using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private LineRenderer backgroundLinePrefab = null;
    [SerializeField] private DrawingController drawingController = null; // NEW LINE
    #endregion

    #region Private Variables
    private Transform _activeContainer = null; // We store the runtime container here
    #endregion

    #region Logic
    public void GenerateLevelShape(LevelShapeConfiguration config)
    {
        // 1. Ensure a container exists
        if (_activeContainer == null)
        {
            // Create a new empty GameObject and name it
            GameObject containerObj = new GameObject("Runtime_Level_Container");
            
            // Set this Level Manager as the parent to keep hierarchy clean
            containerObj.transform.SetParent(this.transform);
            _activeContainer = containerObj.transform;
        }
        else
        {
            // 2. Cleanup old shapes if the container already existed
            foreach (Transform child in _activeContainer) 
            {
                Destroy(child.gameObject);
            }
        }

        if (config.shapePoints.Count < 2) return;

        // 3. Instantiate the Grey Background Line inside our new container
        LineRenderer ghostLine = Instantiate(backgroundLinePrefab, _activeContainer);
        
        // Apply settings from our config
        ghostLine.startWidth = config.lineThickness;
        ghostLine.endWidth = config.lineThickness;
        
        ghostLine.positionCount = config.shapePoints.Count;
        for (int i = 0; i < config.shapePoints.Count; i++)
        {
            ghostLine.SetPosition(i, config.shapePoints[i]);
        }
        if (drawingController != null)
        {
            drawingController.SetupTracing(config.shapePoints);
        }
    }
    #endregion
}
