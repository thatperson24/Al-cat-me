using UnityEngine;

public class MapTile : MonoBehaviour
{
    public static class TileState
    {
        public const char UNOCCUPIED = 'U';
        public const char OCCUPIED = 'O';
        public const char BURNING = 'B';
        public const char TERRAIN = 'T';
        public const char MUDDY = 'M';
        public const char WET = 'W';
        public const char ELECTRIFIED = 'E';
    }

    [SerializeField] private char state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetState(char newState)
    {
        state = newState;
    }
}
