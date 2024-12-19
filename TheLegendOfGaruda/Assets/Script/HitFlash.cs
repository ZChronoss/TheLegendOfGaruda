using UnityEngine;

public class HitFlash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Material hitShader;
    SpriteRenderer rend;
    private Material originalMaterial;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        originalMaterial = rend.material;
        hitShader = Resources.Load<Material>("Materials/FlashMaterial");
    }

    public void TriggerFlash(float duration)
    {
        StartCoroutine(FlashWhite(duration));
    }

    // Update is called once per frame
    private System.Collections.IEnumerator FlashWhite(float duration)
    {
        rend.material = hitShader;

        yield return new WaitForSeconds(duration);

        rend.material = originalMaterial;
    }
}
