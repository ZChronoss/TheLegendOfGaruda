using System.Collections;
using UnityEngine;

public class PanelFader : MonoBehaviour
{
    // MARK - Class ini cuma FADE OUT
    public float duration = 0.8f;

    public void Fade()
    {
        var canvGroup = GetComponent<CanvasGroup>();

        StartCoroutine(DoFade(canvGroup, 0, 1));
    }

    private IEnumerator DoFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;

        yield return new WaitForSeconds(0.8f);

        while(counter < duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }
    }
}
