using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

public class EnemyMovement : MonoBehaviour
{
    // Track parent state
    private MapTile CurrentTile;
    private EncounterMap Map;

    // Track movement state
    private bool IsMoving = false;
    private bool IsCoolingDown = true;

    // Time in seconds to move between one grid position to the next
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private float moveDelay = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsCoolingDown)
        {
            // Create the cooldown
            StartCoroutine(AwaitCooldown(moveDelay));
        }
        else if (!IsMoving && this.Map.turnTracker.IsEnemyTurn())
        {
            CharacterControl characterControl = this.Map.GetCharacterControl();
            List<Point> path = CalculatePathToPlayer();
            // Check if we need to move
            if (path.Count > 1)
            {
                // Move 1 square toward the player
                FollowPathToPlayer(path);
            }
            else
            {
                // Set up attack on the user
                Debug.Log("Attacking the user");
                characterControl.Health -= 2.0f;
                this.Map.turnTracker.EndEnemyTurn();
            }
        }
    }

    public void SetCurrentTile(MapTile tile)
    {
        this.CurrentTile = tile;
    }

    public void SetMap(EncounterMap map)
    {
        this.Map = map;
    }

    private List<Point> CalculatePathToPlayer()
    {
        int numRows = this.Map.Tiles.Length;
        int numCols = this.Map.Tiles[0].Length;
        CharacterControl characterControl = this.Map.GetCharacterControl();

        // TODO: reverse the list to work with it more intuitively
        Debug.Log($"Calculating path to player at {characterControl.GetPosition()}");
        return OccupyingEntity.CalculatePathToPlayer(
            map: this.Map.Tiles,
            currentTile: this.CurrentTile,
            destination: characterControl.GetPosition()
        );
    }

    /**
     * Follows the given path toward the player. Assumes there are at least 2 entries in the list
     */
    private void FollowPathToPlayer(List<Point> path)
    {
        Point nextPoint = path[1]; // first item is current position
        var delta = nextPoint - new Size(this.CurrentTile.MyPosition);
        AttemptMovement(nextPoint.Y, nextPoint.X, new Vector2(delta.X, delta.Y));
    }

    private void AttemptMovement(int nextRow, int nextCol, Vector2 dir)
    {
        MapTile nextTile = this.Map.Tiles[nextRow][nextCol];
        if (nextTile.IsOccupied == true)
        {
            Debug.LogError($"Trying to move into an occupied cell ({nextRow}, {nextCol}). Applying Ostrich algorithm: ignore.");
        }
        else
        {
            StartCoroutine(Move(dir));
            this.CurrentTile.MoveEntityToOtherMapTile(nextTile);

            // Update location of the enemy boi
            gameObject.transform.SetParent(nextTile.gameObject.transform);
            gameObject.transform.localPosition = new Vector3(0, 0, -1);
        }

        this.IsCoolingDown = true;
    }

    private IEnumerator Move(Vector2 direction)
    {
        // Recorder that were are moving so we dont accept more input
        this.IsMoving = true;

        // Make a note of where we are and where we want to go
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(direction.x, direction.y, 0);

        float elaspedTime = 0;
        while (elaspedTime < moveDuration)
        {
            elaspedTime += Time.deltaTime;
            float percent = elaspedTime / moveDuration;
            transform.position = Vector3.Lerp(startPos, endPos, percent);

            yield return null;
        }

        // Make sure we go where we want
        transform.position = endPos;

        // We are no longer moving so we can accept another input
        this.IsMoving = false;
        this.Map.turnTracker.EndEnemyTurn();
    }

    private IEnumerator AwaitCooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.IsCoolingDown = false;
    }
}
