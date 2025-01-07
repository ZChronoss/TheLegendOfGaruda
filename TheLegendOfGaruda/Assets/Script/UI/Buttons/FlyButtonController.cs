using UnityEngine;

public class FlyButtonController : MonoBehaviour
{
    public GameObject flyButton;
    [SerializeField] TouchingDirections touchingDirections;
    
    // Update is called once per frame
    void Update()
    {
        if (!touchingDirections.isGrounded)
        {
            flyButton.SetActive(true);
        }
        else
        {
            flyButton.SetActive(false);
        }
    }
}
