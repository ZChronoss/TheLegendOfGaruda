using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Attack")]
    public List<AudioClip> attackSFX;

    [Header("Heal")]
    public AudioClip healSFX;

    [Header("Footsteps")]
    public List<AudioClip> grassFX;
    public List<AudioClip> rockFX;
    public List<AudioClip> woodFX;

    enum FSMaterial
    {
        Grass, Wood, Rock
    }


    public void PlayAttackSFX()
    {
        AudioClip clip = attackSFX[Random.Range(0, attackSFX.Count)];
        SFXManager.instance.PlaySFXClip(clip, transform, 0.5f);
    }

    public void PlayHealSFX()
    {
        SFXManager.instance.PlaySFXClip(healSFX, transform, 0.5f);
    }

    public void PlayFootstep()
    {
        AudioClip clip;

    }
}
