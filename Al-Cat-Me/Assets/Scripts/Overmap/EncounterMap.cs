
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Drawing;
using static UnityEngine.Rendering.DebugUI.Table;


public class EncounterMap : MonoBehaviour
{
    [SerializeField] private string mapData;
    [SerializeField] private GameObject MapTilePrefab;
    [SerializeField] private GameObject CharacterPrefab;
    [SerializeField] private List<GameObject> EnemyPrefabs;

    public MapTile[][] Tiles;
    private GameObject character;
    private List<MapTile> indicated;
    private SpellCard card;

    private List<string> dungeonLevels;
    private List<string> elementLevels;
    private List<string> tomfooleryLevels;
    private System.Random rng = new System.Random();

    private const char END_ROW_CHAR = 'X';

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Tomfoolery levels
        tomfooleryLevels = new List<string>
        {
            // Smiley Face level
            "UUUUUUUUUUUUUUUXUUUUUUUUUUUUUUUXUUUUUUUUUUUUUUUXUUUUUUUUUUUUUUUXUUUUTUUUUUTUUUUXUUUUUUUUUUUUUUUXUUUUUUUUUUUUUUUXUUUUUUUUUUUUUUUXUUUUUUUUUUUUUUUXUUUUUUUUUUUUUUUXUUUUUUTTTUUUUUUXUUUUTTTTTTTUUUUXUUUUUTTTTTUUUUUXUUUUUUUUUUUUUUUXUUUUUUUUUUUUUUUX",
            
            // Death box
            "TTTTTTTTTTTTTTTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTMMMMMMMMMMMMMTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTBBBBBBBBBBBBBTXTUUUUUUUUUUUUUTXTWWWWWWWWWWWWWTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTEEEEEEEEEEEEETXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTTTTTTTTTTTTTTTX"
        };

        // Dungeon levels
        dungeonLevels = new List<string>
        {
            // Dungeon
            "TTTTTTUTTTTTTTTXTUUUUUUUUUUUUUTXTUWWUUUUUUUUMUUXTUUUUUUUUUUUUUTXTTTTTUTTTTTTTTTXTUUUUUUUUUUUUUTXTUUUUTTTUTUUUUUXTUUUUTUUUUTUUUUXTUUUUTUUUUTUUUUXTUUUUTTTUTUUUUUXTUUUUUUUUUUUUUTXTUUUUUWUUUUUUMUXTUUUUUUUUUUUUUTXTTTTTTUTTTTTTTTXUUUUUUUUUUUUUUUX",

            // Maze 1
            "TUTTTUTTTUTTTUTXTUUUUUTTTUTTTUTXTUTTTUTTTUTTTUTXTUTTTUUUUUTTTUTXTUTTTUTTTUTTTUTXTUTTTUTTTUUUUUTXTUTTTUTTTUTTTUTXTUUUUUTTTUTTTUTXTUTTTUTTTUTTTUTXTUTTTUUUUUTTTUTXTUTTTUTTTUTTTUTXTUTTTUTTTUUUUUTXTUTTTUTTTUTTTUTXTUUUUUUUUUUUUUTXTUTTTUTTTUTTTUTX",

            // Maze 2
            "TUUUUUUUUUUTUUUXTUTTTUTTTUTTTUUXTUUUUUUUUTUUUTUXTUTTUTTTUTTUTTUXTUUTUTUUUUUUUUUXTUTTTUTUTUTUTUUXTUUTUUUUUTUUUTUXTUTTUTTTUTTTTTUXTUUUUUUUUUUUUUUXTUTTTUTTTUTTTUUXTUUTUUUUUTUUUUUXTUTTUTTTUTTTTTUXTUUUUTUUUTUUUUUXTTTTUTUUUTTTTTTXTTTUUTUUUTTTTTTX",

            // Courtyard
            "TTTTTTTTTTTTTTTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTUUBBUUUUUBBUUTXTUUBBUUUUUBBUUTXTUUUUUUUUUUUUUTXTUUUUUUWUUUUUUTXTUUUUUWWWUUUUUTXTUUUUUUWUUUUUUTXTUUUUUUUUUUUUUTXTUUBBUUUUUBBUUTXTUUBBUUUUUBBUUTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXUUUUUUUUUUUUUUUX",

            // Two houses
            "TTTTTTUUUTTTTTTXTWUUUTUUUTUUUTTXTUUUUTUUUTUUUUTXTUUUUUUUUTUUTUTXTUUUUUUUUTUUUUTXTUTUUTUUUTUTUBTXTUUUUTUUUTUTUBTXTUUUUTUUUTUUUUTXTUUUUTUUUTUUUUTXTUUUUTUUUTUUUUTXTUUUUTUUUUUUUUTXTUTTUTUUUUUUUUTXTUUUUTUUUTUUUUTXTUBBUTUUUTWUUUTXTTTTTTUUUTTTTTTX",

            // Altar
            "TTTTTTTTTTTTTTTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTUUUUUTTTUUUUUTXTUUUUUTTTUUUUUTXTUUUUUTTTUUUUUTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTTTTTTTTTTTTTTTX",

            // Prison
            "TTTTTTTTTTTTTTTXTWUUTWUUTWUUTWTXTUUUTUUUTUUUTWTXTUUUTUUUTUUUTUTXTTUTTTUTTTUTTUTXTUUUBUUUBUUUBUTXTUUUUUUUUUUUUUTXUUUUUUUUUUUUUUUXTUUUUUUUUUUUUUTXTUUUBUUUBUUUBUTXTTUTTTUTTTUTTUTXTUUUTUUUTUUUTUTXTUUUTUUUTUUUTWTXTUUWTUUWTUUWTWTXTTTTTTTTTTTTTTTX",

            // Throne room
            "TTTTTTUUUTTTTTTXTUUUUBUUUBUUUUTXTUUUUUUUUUUUUUTXTUUTTUUUUUTTUUTXTUUTTUUUUUTTUUTXTUUUUUUUUUUUUUTXTUUTTUUUUUTTUUTXTUUTTUUUUUTTUUTXTUUUUUUUUUUUUUTXTUUUUUUUUUUUUUTXTUUTTUUUUUTTUUTXTUUTTBUTUBTTUUTXTUUUUUTTTUUUUUTXTUUUUUTTTUUUUUTXTTTTTTTTTTTTTTTX"
        };

        // Element levels
        elementLevels = new List<string>
        {
            // Trench
            "TTTTTTTTTTTTTTTXTMUUUUUUUUUUMMTXTUUUUUUUUUUUMMTXTUTTTTTTUUUUUMTXTMTTTTTTUMMMMUTXTMUUUUUUUUUUUMTXTMUMMMMMMMMMMMTXTUMMMMMMMMMMUUTXTUUUUUUUUUUUMUTXTUUUUUUUUUUUMUTXTMMTTTTTTUUUMUTXTUUTTTTTTUUUUMTXTUUUUUUUUUUUUUTXTMMMMUUUUUUMMMTXTTTTTTTTTTTTTTTX",

            // Burning maze
            "TTTTTTTTTTTTTTTXTUBUUUBUUUBUBUTXTUTTTUTTTUTTTUTXUUTUBUBUTUBUBUTXTUTTTUTTTUTTTUTXTUTUBBBUBUBUBUTXTBTTTUTTTUTTTBTXTUTUBUBUTTTUBUTXTUTTTUTTTUTTTUTXTUBUUBUUTUBUBUTXTUTTTTTUTUTTTUTXTUBUTUTUBUBUBUUXTUTTTUTTTUTTTUTXTUTUBUBUBUBUBUTXTTTTTTTTTTTTTTTX",

            // Lightning temple
            "TTTTTTTTTTTTTTTXTUUUUUUUUUUUUUTXTUUUEUEUEUEUUUUXTUUUTUUUTUUUTUUXTUUUEUEUEUEUUUTXTUUUTUUUTUUUTUTXTUUUEUEUEUEUUUTXTUUUTUUUTUUUTUTXTUUUEUEUEUEUUUTXTUUUTUUUTUUUTUTXTUUUEUEUEUEUUUTXTUUUTUUUTUUUTUUXTUUUEUEUEUEUUUUXTUUUUUUUUUUUUUTXTTTTTTTTTTTTTTTX",

            // Sunken city
            "TTTTTTTTTTTTTTTXWWWWWWWWWWWWWWWXWUUUWWWWWWWUUUWXWUUUUWWWWWUUUUWXTWWWWWWWWWWWWWTXWUUUUUWWWUUUUUWXWUUUUUWWWUUUUUWXTWWWWWWWWWWWWWTXWWUUUUUWUUUUUWWXWWUUUUUWUUUUUWWXTWWTTTTWTTTTWWTXWUUTTTUUUTTTUUWXWUUWWWUUUWWWUUWXWUUTTTUUUTTTUUWXTTTTTTTTTTTTTTTX",

            // Swamp
            "WWMTMWMMMWWMMWWXWMWWMUUUUUUMMWWXMMMMMUUUUUUMMWWXMUTTUUWWTTTWWWTXWUTTUUMMTTTUUUWXWUUUUUUUUUUUUUWXTUUUUUUUUUUMWWWXTUTUUTTUWWUMTTTXWUTWMTTMMWUUTTMXWUUWMUTMMWUUUUWXMUUWMUUUUUTUUMWXTUUTTUUUUUTTWMMXWUUTTUUUWWMUUUTXWWMUUUUUMWMUUUWXWMMWWTTMWWWMWWMX"
        };

        mapData = GetRandomLevel();

        this.Tiles = GenerateMap();
        this.CenterMap();
        this.PlaceEnemies(3);

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

        MapTile destTile = Tiles[0][newX];
        GameObject newCharacter = Instantiate(CharacterPrefab, destTile.gameObject.transform);
        newCharacter.name = "Character";
        newCharacter.GetComponent<CharacterControl>().SetEncounterMap(this);
        newCharacter.GetComponent<CharacterControl>().SetTile(destTile);

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

    public CharacterControl GetCharacterControl()
    {
        return this.character.GetComponent<CharacterControl>();
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
                Debug.Log($"Placing enemy at ({targetRow}, {targetCol})");
                GameObject newEnemy = Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)]);
                newEnemy.transform.SetParent(Tiles[targetRow][targetCol].gameObject.transform);
                newEnemy.transform.localPosition = new Vector3(0, 0, -1);
                newEnemy.GetComponent<EnemyMovement>().SetMap(this);

                tile.SetEntity(newEnemy);
                numEnemies--;
            }
        }
    }

    /*
    Level Pooling
    */

    public string GetRandomLevel()
    {
        int roll = rng.Next(100); // 0–99

        if (roll < 70) // 70% chance
        {
            return dungeonLevels[rng.Next(dungeonLevels.Count)];
        }
        else if (roll < 95) // 25% chance
        {
            return elementLevels[rng.Next(elementLevels.Count)];
        }
        else // 5% chance
        {
            return tomfooleryLevels[rng.Next(tomfooleryLevels.Count)];
        }
    }

}
