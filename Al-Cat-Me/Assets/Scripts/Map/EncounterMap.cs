using UnityEngine;

public class EncounterMap : MonoBehaviour
{
    [SerializeField] private string mapData;
    [SerializeField] private GameObject MapTilePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    Convert mapData string into a 2D integer array for MapTile data
    Parameters:
        - mapData = String of characters to signify what state a tile should be
            - N = No tile
            - U = Base tile with no enemies or player occupying it
            - O = Occupied by an entity
            - B = Has fire on it
            - T = Has terrain that blocks spells and movement
            - M = Muddy tile that reduces movement
            - W = Wet tile that reduces movement
            - E = Has electricity on it 
    Output:
        - N/A
    */
    private void GenerateMap()
    {
        int row = 0, col = 0, maxCol = 0;
        for (int i = 0; i < mapData.Length; i++)
        {
            if (mapData[i] != 'X')
            {
                GameObject newTile = Instantiate(MapTilePrefab, this.gameObject.transform);
                newTile.GetComponent<MapTile>().SetState(mapData[i]);
                switch (mapData[i])
                {
                    case MapTile.TileState.UNOCCUPIED:
                        newTile.GetComponent<SpriteRenderer>().color = Color.green;
                        break;
                    case MapTile.TileState.OCCUPIED:
                        newTile.GetComponent<SpriteRenderer>().color = Color.yellow;
                        break;
                    case MapTile.TileState.BURNING:
                        newTile.GetComponent<SpriteRenderer>().color = Color.red;
                        break;
                    case MapTile.TileState.TERRAIN:
                        newTile.GetComponent<SpriteRenderer>().color = Color.brown;
                        break;
                    case MapTile.TileState.MUDDY:
                        newTile.GetComponent<SpriteRenderer>().color = Color.black;
                        break;
                    case MapTile.TileState.WET:
                        newTile.GetComponent<SpriteRenderer>().color = Color.blue;
                        break;
                    case MapTile.TileState.ELECTRIFIED:
                        newTile.GetComponent<SpriteRenderer>().color = Color.purple;
                        break;
                    default:
                        Destroy(newTile);
                        break;
                }
                newTile.name = "Tile: Col-" + col + " row-" + row;
                newTile.transform.position = new Vector2(col, row);
                col++;
            }
            else if (mapData[i] == 'X')
            {
                row--;
                if (col > maxCol)
                {
                    maxCol = col;
                }
                col = 0;
            }
        }
        float newX;
        float newY;

        if (maxCol % 2 == 0)
        {
            newX = (float)(-maxCol / 2) + .5f;
        }
        else
        {
            newX = (float)(-maxCol / 2);
        }

        if (row + 1 % 2 == 0)
        {
            newY = (float)(-row / 2) + .5f;
        }
        else
        {
            newY = (float)(-row / 2);
        }
        this.gameObject.transform.position = new Vector2(newX, newY);
    }
}
