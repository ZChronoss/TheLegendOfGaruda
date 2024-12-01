using UnityEngine;

// Salah bikin, gw kira belom ada :)
// Didiemin aja disini, siapa tau kepake nanti

public class Damageable : MonoBehaviour
{
    //Animator animator;

    [SerializeField]
    private int _maxHealth= 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;

            // If health drops below 0, character is no longer alive
            if(_health < 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvincible = false;

    public bool IsAlive {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
        }
    }

    public void Hit(int damage)
    {
        if(IsAlive && !isInvincible)
        {
            Health -= damage;
        }
    }
}
