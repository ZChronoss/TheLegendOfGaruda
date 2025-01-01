using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BonfireController : MonoBehaviour
{
    public bool canRest = false;
    float restTime = 1f;

    [SerializeField] InputActionAsset input;

    PlayerHealth pHealth;
    PlayerHealthPotion pHPotion;
    private DataPersistenceManager dataPersistenceManager;

    private void Start()
    {
        input.FindAction("Rest").started += OnRest;
    }

    private void Awake()
    {
        pHealth = FindAnyObjectByType<PlayerController>().GetComponent<PlayerHealth>();
        pHPotion = FindAnyObjectByType<PlayerController>().GetComponent<PlayerHealthPotion>();

        dataPersistenceManager = FindAnyObjectByType<DataPersistenceManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            canRest = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canRest = false;
        }
    }

    private void OnRest(InputAction.CallbackContext context)
    {
        if(canRest)
        {
            StartCoroutine(RestCoroutine());
        }
    }

    private IEnumerator RestCoroutine()
    {
        input.Disable();
        yield return new WaitForSeconds(restTime);

        pHealth.ResetHealth();
        pHPotion.ResetPotions();
        dataPersistenceManager.SaveGame();

        input.Enable();
    }
}
