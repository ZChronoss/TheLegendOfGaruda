using System.Runtime.InteropServices;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Material hitShader;
    SpriteRenderer rend;
    private Material originalMaterial;
    private Material outlineShader;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        originalMaterial = rend.material;
        hitShader = Resources.Load<Material>("Materials/FlashMaterial");
        outlineShader = Resources.Load<Material>("Materials/OutlineMaterial");
    }

    public void TriggerFlash(float duration)
    {
        StartCoroutine(FlashWhite(duration));
    }

    public void giveOutline()
    {
        if (rend.material != outlineShader){
            rend.material = outlineShader;
        }
    }

    public void removeOutline()
    {
        if (rend.material != originalMaterial){
            rend.material = originalMaterial;
        }
    }

    // Update is called once per frame
    private System.Collections.IEnumerator FlashWhite(float duration)
    {
        rend.material = hitShader;

        yield return new WaitForSeconds(duration);

        rend.material = originalMaterial;
    }
}
