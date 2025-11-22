
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Drawing;


public class EncounterMap : MonoBehaviour
{
    [SerializeField] private string mapData;
    [SerializeField] private GameObject MapTilePrefab;
    [SerializeField] private GameObject CharacterPrefab;

    public MapTile[][] Tiles;
    private GameObject character;
    private List<MapTile> indicated;
    private SpellCard card;

    private const char END_ROW_CHAR = 'X';

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.Tiles = GenerateMap();
        this.CenterMap();
        this.PlaceEnemies(3);

        MapTile newTile = Tiles[0][0];
        indicated = new List<MapTile>();
        character = SpawnCharacter();
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
        - 2-D array of tiles
    */
    private MapTile[][] GenerateMap()
    {
        return mapData
            .Split(END_ROW_CHAR)
            .Select((rowData, rowIndex) =>
                rowData.ToCharArray().Select((tileCode, colIndex) =>
                {
                    GameObject newTile = Instantiate(MapTilePrefab, this.gameObject.transform);
                    MapTile tile = newTile.GetComponent<MapTile>();

                    tile.SetInitialState(tileCode);
                    tile.MyPosition = new Point(colIndex, rowIndex);
                    newTile.name = "Tile: Col-" + colIndex + " row-" + rowIndex;
                    newTile.transform.position = new Vector2(colIndex, rowIndex);

                    return tile;
                }).ToArray()
            ).ToArray();

    }

    private GameObject SpawnCharacter()
    {
        int maxColumns = this.Tiles.Max(row => row.Length);
        int newX = (maxColumns % 2 == 0)
            ? (maxColumns / 2) + 1
            : (maxColumns / 2);
        
        GameObject newCharacter = Instantiate(CharacterPrefab, Tiles[0][newX].gameObject.transform);
        newCharacter.name = "Character";
        newCharacter.GetComponent<CharacterControl>().SetEncounterMap(this);
        newCharacter.GetComponent<CharacterControl>().SetRow(0);
        newCharacter.GetComponent<CharacterControl>().SetCol(newX);

        return newCharacter;
    }

    private void CenterMap()
    {
        int maxColumns = this.Tiles.Max(row => row.Length);
        int numRows = this.Tiles.Length;

        float newX = (maxColumns % 2 == 0)
            ? (float)(-maxColumns / 2) + .5f
            : (float)(-maxColumns / 2);
        float newY = (numRows % 2 == 0)
            ? (float)(-numRows / 2) + .5f
            : (float)(-numRows / 2);

        this.gameObject.transform.position = new Vector2(newX, newY);
    }

    public MapTile[][] GetMapTiles()
    {
        return this.Tiles; 
    }

    public void AddIndicated(MapTile tile)
    {
        indicated.Add(tile);
    }

    public void ClearIndicated()
    {
        foreach (MapTile tile in indicated)
        {
            tile.RemoveAttack();
        }
        indicated.Clear();
    }

    
    /*
    Place enemies on the map
    */
    private void PlaceEnemies(int numEnemies)
    {
        // TODO: Create dynamic logic and orientations for where to place enemies / enemy type
        int numRows = this.Tiles.Length;
        int numCols = this.Tiles[0].Length;
        while (numEnemies > 0)
        {
            int targetRow = Random.Range(0, numRows);
            int targetCol = Random.Range(0, this.Tiles[targetRow].Length);

            MapTile tile = this.Tiles[targetRow][targetCol];
            if (!tile.IsOccupied && tile.CanBeOccupied)
            {
                // TODO: do something useful with enemies
                Debug.Log($"Placing enemy at ({targetRow}, {targetCol})");
                OccupyingEntity entity = new OccupyingEntity();
                tile.SetEntity(entity);

                // Calculate path from enemy -> player
                // TODO: get player location instead of assuming map corner
                entity.CalculatePathToPlayer(
                    map: this.Tiles,
                    destination: new Point(numCols - 1, numRows - 1)
                );
                numEnemies--;
            }
        }
    }
}
