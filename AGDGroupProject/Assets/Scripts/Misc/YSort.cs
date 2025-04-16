using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Basisoffset om negatieve sortingOrder te voorkomen
    private const int sortingOrderOffset = 10000;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        float bottomY = spriteRenderer.bounds.min.y;

        // Bereken de sortingOrder met behoud van de relatieve volgorde, maar zorg ervoor dat deze altijd positief is
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-bottomY * 100) + sortingOrderOffset;
    }
}
