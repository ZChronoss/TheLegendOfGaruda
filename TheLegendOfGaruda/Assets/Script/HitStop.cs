using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool waiting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Stop(float duration){
        if (waiting) return;
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    // Update is called once per frame
    IEnumerator Wait(float duration){
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        waiting = false;
    }
}
