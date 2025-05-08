using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
public class YSortTilemap : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    void LateUpdate()
    {
        // Get the bottom Y position of the Tilemap
        float bottomY = tilemapRenderer.bounds.min.y;

        // Set the sorting order based on the Y position (same logic as SpriteRenderer)
        tilemapRenderer.sortingOrder = Mathf.RoundToInt(-bottomY * 100);

        // Debugging output
        Debug.Log("Tilemap Sorting Order: " + tilemapRenderer.sortingOrder + " at Y-position: " + bottomY);
    }
}
