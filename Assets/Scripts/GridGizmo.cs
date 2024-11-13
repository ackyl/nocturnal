using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class GridGizmo : MonoBehaviour
{
    public float cellSize = 6f; // Custom grid cell size
    public Color gridColor = new Color(0, 1, 0, 0.2f); // Color of the grid lines

    private RectTransform rectTransform;

    void OnDrawGizmos()
    {
        rectTransform = GetComponent<RectTransform>();

        // Set gizmo color
        Gizmos.color = gridColor;

        // Calculate the dimensions of the RectTransform
        Vector2 size = rectTransform.rect.size;
        Vector2 position = rectTransform.position;

        // Offset to start drawing from the top-left corner of the RectTransform
        Vector2 startPos = new Vector2(position.x - size.x / 2, position.y - size.y / 2);

        // Draw vertical grid lines
        for (float x = startPos.x; x <= startPos.x + size.x; x += cellSize)
        {
            Gizmos.DrawLine(new Vector3(x, startPos.y, 0), new Vector3(x, startPos.y + size.y, 0));
        }

        // Draw horizontal grid lines
        for (float y = startPos.y; y <= startPos.y + size.y; y += cellSize)
        {
            Gizmos.DrawLine(new Vector3(startPos.x, y, 0), new Vector3(startPos.x + size.x, y, 0));
        }
    }
}
