using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Attack")]
    public List<AudioClip> attackSFX;

    [Header("Heal")]
    public AudioClip healSFX;

    [Header("Footsteps")]
    public List<AudioClip> grassFootstepsFX;
    public List<AudioClip> rockFootstepsFX;
    public List<AudioClip> woodFootstepsFX;
    public List<AudioClip> soilFootstepsFX;

    [Header("Jump")]
    public List<AudioClip> grassJumpFX;
    public List<AudioClip> rockJumpFX;
    public List<AudioClip> woodJumpFX;
    public List<AudioClip> soilJumpFX;

    [Header("Land")]
    public List<AudioClip> grassLandFX;
    public List<AudioClip> rockLandFX;
    public List<AudioClip> woodLandFX;
    public List<AudioClip> soilLandFX;

    [Header("Fallback Clips")]
    public AudioClip fallbackClip;

    private TouchingDirections touchDir;

    private void Start()
    {
        touchDir = GetComponent<TouchingDirections>();
    }

    private enum FSMaterial
    {
        Grass, Wood, Rock, Soil, Empty
    }

    public void PlayAttackSFX()
    {
        if (attackSFX != null && attackSFX.Count > 0)
        {
            AudioClip clip = attackSFX[Random.Range(0, attackSFX.Count)];
            SFXManager.instance?.PlaySFXClip(clip, transform, 0.5f);
        }
        else
        {
            Debug.LogWarning("Attack SFX list is empty or not assigned.");
        }
    }

    public void PlayHealSFX()
    {
        if (healSFX != null)
        {
            SFXManager.instance?.PlaySFXClip(healSFX, transform, 0.5f);
        }
        else
        {
            Debug.LogWarning("Heal SFX is not assigned.");
        }
    }

    private FSMaterial SurfaceSelect()
    {
        if (touchDir == null || touchDir.groundHits == null || !touchDir.groundHits.Any())
        {
            Debug.LogWarning("No ground detected. Defaulting to Empty.");
            return FSMaterial.Empty;
        }

        var curGround = touchDir.groundHits.First().transform?.tag;

        return curGround switch
        {
            "Ground" => FSMaterial.Grass,
            "Platform" => FSMaterial.Wood,
            "Rock" => FSMaterial.Rock,
            "Soil" => FSMaterial.Soil,
            _ => FSMaterial.Empty,
        };
    }

    private void PlaySound(FSMaterial surface, List<AudioClip> grassFX, List<AudioClip> woodFX, List<AudioClip> rockFX, List<AudioClip> soilFX)
    {
        AudioClip clip = null;

        switch (surface)
        {
            case FSMaterial.Grass:
                if (grassFX != null && grassFX.Count > 0)
                    clip = grassFX[Random.Range(0, grassFX.Count)];
                break;
            case FSMaterial.Wood:
                if (woodFX != null && woodFX.Count > 0)
                    clip = woodFX[Random.Range(0, woodFX.Count)];
                break;
            case FSMaterial.Rock:
                if (rockFX != null && rockFX.Count > 0)
                    clip = rockFX[Random.Range(0, rockFX.Count)];
                break;
            case FSMaterial.Soil:
                if (soilFX != null && soilFX.Count > 0)
                    clip = soilFX[Random.Range(0, soilFX.Count)];
                break;
        }

        if (clip == null && fallbackClip != null)
        {
            clip = fallbackClip;
            Debug.LogWarning($"Using fallback clip for surface: {surface}");
        }

        if (clip != null && SFXManager.instance != null)
        {
            SFXManager.instance.PlaySFXClip(clip, transform, 1f);
        }
        else
        {
            Debug.LogWarning($"No valid clip found or SFXManager instance is null for surface: {surface}");
        }
    }

    public void PlayFootstep()
    {
        FSMaterial surface = SurfaceSelect();
        PlaySound(surface, grassFootstepsFX, woodFootstepsFX, rockFootstepsFX, soilFootstepsFX);
    }

    public void PlayJump()
    {
        FSMaterial surface = SurfaceSelect();
        PlaySound(surface, grassJumpFX, woodJumpFX, rockJumpFX, soilJumpFX);
    }

    public void PlayLand()
    {
        FSMaterial surface = SurfaceSelect();
        PlaySound(surface, grassLandFX, woodLandFX, rockLandFX, soilLandFX);
    }
}
