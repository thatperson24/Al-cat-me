using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    private EncounterMap encounterMap;
    private MapTile mapTile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetEncounterMap(EncounterMap em)
    {
        encounterMap = em;
    }

    public void SetMapTile(MapTile mapTile)
    {
        this.mapTile = mapTile;
    }

    public void ChangeHealth(int delta)
    {
        health += delta;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) { Die(); }
    }

    public void Heal(int heal)
    {
        health += heal;
    }

    private void Die()
    {
        encounterMap.ReduceEnemies();
        mapTile.SetState('U');
        Destroy(gameObject);
    }
}
