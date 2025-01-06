using UnityEngine;

public class RestButtonController : MonoBehaviour
{
    public GameObject restButton;
    [SerializeField] BonfireController bonfire;

    // Update is called once per frame
    void Update()
    {
        if (bonfire.canRest)
        {
            restButton.SetActive(true);
        }
        else
        {
            restButton.SetActive(false);
        }
    }
}
