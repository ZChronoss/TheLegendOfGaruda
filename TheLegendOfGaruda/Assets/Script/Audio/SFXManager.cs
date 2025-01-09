using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance { get; private set; }

    [SerializeField] private AudioSource sfxObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlaySFXClip(AudioClip clip, Transform spawnTransform, float volume, float duration = 0)
    {
        //spawn gameobject
        AudioSource audioSource = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);

        //assign audioclip
        audioSource.clip = clip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sound fx clip
        float clipLength = audioSource.clip.length;

        //destroy the clip after done playing
        Destroy(audioSource.gameObject, duration == 0 ? clipLength : duration);
    }
}
