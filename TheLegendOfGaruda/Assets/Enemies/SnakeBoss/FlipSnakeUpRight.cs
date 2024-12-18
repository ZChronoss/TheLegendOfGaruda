using UnityEngine;

public class SpriteFlipOnRotate : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this GameObject.");
        }
    }

    void Update()
    {
        CheckAndFlipSprite();
    }

    private void CheckAndFlipSprite()
    {
        // Get the current Z rotation of the GameObject
        float zRotation = NormalizeAngle(transform.eulerAngles.z);

        // Check if the rotation is approximately -90 or 90 degrees
        if (zRotation > 90f || zRotation < -90f)
        {
            // Flip the sprite horizontally
            spriteRenderer.flipY = true;
        }
        else
        {
            // Reset the flip state
            spriteRenderer.flipY = false;
        }
    }

    // Normalize an angle to be within -180 to 180 degrees
    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
}
