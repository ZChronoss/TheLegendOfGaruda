using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;

public class Fader : MonoBehaviour
{
    public TilemapRenderer tilemapRenderer;
    public float fadeDuration = .5f;

    public void fade(bool fadeDirection) {
        float targetAlpha = fadeDirection ? 1f: 0f;
        
        // Access the material's color property
        Material material = tilemapRenderer.material;
        Color color = material.color;

        // Use DOTween to animate the alpha value
        DOTween.To(
            () => color.a,
            alpha => {
                color.a = alpha;
                material.color = color;
            },
            targetAlpha,
            fadeDuration
        );
    }
}
