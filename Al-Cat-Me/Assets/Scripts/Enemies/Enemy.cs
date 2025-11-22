using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeHealth(int delta)
    {
        health += delta;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Heal(int heal)
    {
        health += heal;
    }
}
