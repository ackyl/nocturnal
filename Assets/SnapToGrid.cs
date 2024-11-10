using UnityEngine;

[ExecuteAlways]
public class SnapToGrid : MonoBehaviour
{
    private float gridSize = 10f; // Set the grid size to 6 for 6x6 snapping

    void LateUpdate()
    {
        SnapToPixelGrid(transform);
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

                // Calculate the snapped position based on grid and offset by half-size to align to top-left
                Vector3 localPosition = rectTransform.localPosition;
                float snappedX = Mathf.Round((localPosition.x - widthOffset) / gridSize) * gridSize + widthOffset;
                float snappedY = Mathf.Round((localPosition.y - heightOffset) / gridSize) * gridSize + heightOffset;

                rectTransform.localPosition = new Vector3(snappedX, snappedY, localPosition.z);
            }

            // If the child has children, recursively apply pixel snapping
            // if (child.childCount > 0)
            // {
            //     SnapToPixelGrid(child);
            // }
        }
    }
}
