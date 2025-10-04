using UnityEngine;

public class MapTile : MonoBehaviour
{
    public enum TileState
    {
        UNOCCUPIED,
        OCCUPIED,
        BURNING,
        TERRAIN,
        MUDDY,
        WET,
        ELECTRIFIED
    }

    [SerializeField] private TileState state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetState(TileState newState)
    {
        state = newState;
    }
}
