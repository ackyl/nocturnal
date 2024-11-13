using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class SnapToGrid : MonoBehaviour
{
    private float gridSize = 6f; // Set the grid size to 6 for 6x6 snapping

    void LateUpdate()
    {
        // Ensure this runs only in edit mode, not in play mode
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            SnapToPixelGrid(transform);
        }
#endif
    }

    private void SnapToPixelGrid(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Get RectTransform for UI elements
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Calculate width and height in grid units
                float widthOffset = (rectTransform.rect.width / 2) % gridSize;
                float heightOffset = (rectTransform.rect.height / 2) % gridSize;

                // Calculate the snapped global position based on grid and offset by half-size to align to top-left
                Vector3 worldPosition = rectTransform.position;
                float snappedX = Mathf.Round((worldPosition.x - widthOffset) / gridSize) * gridSize + widthOffset;
                float snappedY = Mathf.Round((worldPosition.y - heightOffset) / gridSize) * gridSize + heightOffset;

                rectTransform.position = new Vector3(snappedX, snappedY, worldPosition.z);
            }

            // If the child has children, recursively apply grid snapping
            if (child.childCount > 0)
            {
                SnapToPixelGrid(child);
            }
        }
    }
}
