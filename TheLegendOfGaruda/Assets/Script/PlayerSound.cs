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

    TouchingDirections touchDir;

    private void Start()
    {
        touchDir = GetComponent<TouchingDirections>();
    }

    enum FSMaterial
    {
        Grass, Wood, Rock, Soil, Empty
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

    private FSMaterial SurfaceSelect()
    {
        var curGround = touchDir.groundHits.First().transform.tag;

        if (curGround.Equals("Ground"))
        {
            return FSMaterial.Grass;
        }
        else if (curGround.Equals("Platform"))
        {
            return FSMaterial.Wood;
        }
        else if (curGround.Equals("Stone"))
        {
            return FSMaterial.Rock;
        }
        else if (curGround.Equals("Soil"))
        {
            return FSMaterial.Soil;
        }
        else
        {
            return FSMaterial.Empty;
        }
    }

    public void PlayFootstep()
    {
        AudioClip clip = null;

        FSMaterial surface = SurfaceSelect();

        switch (surface)
        {
            case FSMaterial.Grass:
                clip = grassFootstepsFX[Random.Range(0, grassFootstepsFX.Count)]; 
                break;
            case FSMaterial.Wood:
                clip = woodFootstepsFX[Random.Range(0, woodFootstepsFX.Count)];
                break;
            case FSMaterial.Rock:
                clip = rockFootstepsFX[Random.Range(0, rockFootstepsFX.Count)];
                break;
            case FSMaterial.Soil:
                clip = soilFootstepsFX[Random.Range(0, soilFootstepsFX.Count)];
                break;
            default:
                break;
        }

        if(surface != FSMaterial.Empty)
        {
            SFXManager.instance.PlaySFXClip(clip, transform, 1f);
        }
    }

    public void PlayJump()
    {
        AudioClip clip = null;

        FSMaterial surface = SurfaceSelect();

        switch (surface)
        {
            case FSMaterial.Grass:
                clip = grassJumpFX[Random.Range(0, grassJumpFX.Count)];
                break;
            case FSMaterial.Wood:
                clip = woodJumpFX[Random.Range(0, woodJumpFX.Count)];
                break;
            case FSMaterial.Rock:
                clip = rockJumpFX[Random.Range(0, rockJumpFX.Count)];
                break;
            case FSMaterial.Soil:
                clip = soilJumpFX[Random.Range(0, soilJumpFX.Count)];
                break;
            default:
                break;
        }

        if (surface != FSMaterial.Empty)
        {
            SFXManager.instance.PlaySFXClip(clip, transform, 1f);
        }
    }

    public void PlayLand()
    {
        AudioClip clip = null;

        FSMaterial surface = SurfaceSelect();

        switch (surface)
        {
            case FSMaterial.Grass:
                clip = grassLandFX[Random.Range(0, grassLandFX.Count)];
                break;
            case FSMaterial.Wood:
                clip = woodLandFX[Random.Range(0, woodLandFX.Count)];
                break;
            case FSMaterial.Rock:
                clip = rockLandFX[Random.Range(0, rockLandFX.Count)];
                break;
            case FSMaterial.Soil:
                clip = soilLandFX[Random.Range(0, soilLandFX.Count)];
                break;
            default:
                break;
        }

        if (surface != FSMaterial.Empty)
        {
            SFXManager.instance.PlaySFXClip(clip, transform, 1f);
        }
    }
}
