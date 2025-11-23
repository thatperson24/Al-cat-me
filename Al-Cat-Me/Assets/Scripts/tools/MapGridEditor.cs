using UnityEngine;
using UnityEditor;
using System.Text;

public class MapGridEditor : EditorWindow
{
    private int width = 15;
    private int height = 15;
    private char[,] grid;
    private char currentTile = 'U'; // tile type to paint
    private string inputString = "";

    private Vector2 scrollPos;

    [MenuItem("Tools/Map Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow<MapGridEditor>("Map Grid Editor");
    }

    private void OnEnable()
    {
        InitGrid();
    }

    private void InitGrid()
    {
        grid = new char[height, width];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                grid[y, x] = 'T'; // default tile
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Forest Maze Builder", EditorStyles.boldLabel);

        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);

        if (grid == null || grid.GetLength(0) != height || grid.GetLength(1) != width)
            InitGrid();

        EditorGUILayout.Space();

        // Tile selection
        EditorGUILayout.LabelField("Current Tile Type:");
        currentTile = EditorGUILayout.TextField(currentTile.ToString())[0];

        EditorGUILayout.Space();

        // Input string field
        EditorGUILayout.LabelField("Import Map String:");
        inputString = EditorGUILayout.TextField(inputString);

        if (GUILayout.Button("Load From String"))
        {
            LoadFromString(inputString);
        }

        EditorGUILayout.Space();

        // Grid drawing
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int y = height - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < width; x++)
            {
                string label = grid[y, x].ToString();
                if (GUILayout.Button(label, GUILayout.Width(25), GUILayout.Height(25)))
                {
                    grid[y, x] = currentTile;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        if (GUILayout.Button("Export Map String"))
        {
            string mapString = ExportMapString();
            Debug.Log("Map String:\n" + mapString);
            EditorGUIUtility.systemCopyBuffer = mapString; // copy to clipboard
        }

        if (GUILayout.Button("Clear Grid"))
        {
            InitGrid();
        }
    }

    private string ExportMapString()
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < height; y++) // bottom to top
        {
            for (int x = 0; x < width; x++)
                sb.Append(grid[y, x]);
            sb.Append('X');
        }
        return sb.ToString();
    }

    private void LoadFromString(string mapString)
    {
        if (string.IsNullOrEmpty(mapString)) return;

        string[] rows = mapString.Split('X');
        int rowCount = Mathf.Min(rows.Length, height);

        for (int y = 0; y < rowCount; y++)
        {
            string row = rows[y];
            for (int x = 0; x < Mathf.Min(row.Length, width); x++)
            {
                grid[y, x] = row[x]; // direct assignment, no flip
            }
        }
    }
}