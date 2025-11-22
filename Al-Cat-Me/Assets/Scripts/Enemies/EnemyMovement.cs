using UnityEngine;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    // Track parent state
    private MapTile CurrentTile;
    private MapTile[][] MapTiles;

    // Track movement state
    private bool IsMoving = false;
    [SerializeField] private float moveDuration = 2.5f;//Time in seconds to move between one grid position to the next

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMoving)
        {
            // Calculate path from enemy -> player
            int numRows = this.MapTiles.Length;
            int numCols = this.MapTiles[0].Length;
            List<Point> path = OccupyingEntity.CalculatePathToPlayer(
                map: this.MapTiles,
                currentTile: this.CurrentTile,
                // TODO: get player location instead of assuming map corner
                destination: new Point(numCols - 1, numRows - 1)
            );

            // Check if we need to move
            if (path.Count > 1)
            {
                Point nextPoint = path[path.Count - 2]; // last item is current position
                var delta = nextPoint - new Size(this.CurrentTile.MyPosition);
                Debug.Log($"Moving in direction: {delta}");
                AttemptMovement(nextPoint.Y, nextPoint.X, new Vector2(delta.X, delta.Y));

            }
            this.IsMoving = true;
        }
    }

    public void SetCurrentTile(MapTile tile)
    {
        this.CurrentTile = tile;
    }

    public void SetMap(MapTile[][] map)
    {
        this.MapTiles = map;
    }

    private void AttemptMovement(int nextRow, int nextCol, Vector2 dir)
    {
        if (this.MapTiles[nextRow][nextCol].IsOccupied == true)
        {
            Debug.LogError($"Trying to move into an occupied cell ({nextRow}, {nextCol}). Applying Ostrich algorithm: ignore.");
        }
        else
        {
            StartCoroutine(Move(dir));
            this.CurrentTile = this.MapTiles[nextRow][nextCol];
            gameObject.transform.SetParent(this.MapTiles[nextRow][nextCol].gameObject.transform);
        }
    }

    private IEnumerator Move(Vector2 direction)
    {
        // Recorder that were are moving so we dont accept more input
        this.IsMoving = true;

        // Make a note of where we are and where we want to go
        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + direction;

        float elaspedTime = 0;
        while (elaspedTime < moveDuration)
        {
            elaspedTime += Time.deltaTime;
            float percent = elaspedTime / moveDuration;
            transform.position = Vector2.Lerp(startPos, endPos, percent);
            yield return null;
        }

        // Make sure we go where we want
        transform.position = endPos;

        // We are no longer moving so we can accept another input
        this.IsMoving = false;
    }
}
