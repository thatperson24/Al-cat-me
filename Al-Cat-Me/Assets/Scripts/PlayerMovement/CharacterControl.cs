using System.Linq;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using UnityEngine.Scripting.APIUpdating;
using Assets.Scripts.StateEffects;
using Unity.VisualScripting;

public class CharacterControl : MonoBehaviour
{

    public float movementSpeed = 5f;
    public Transform moveLocation;

    [SerializeField] private float moveDuration = 0.1f;//Time in seconds to move between one grid position to the next
    public bool isMoving = false;   //Helps with checking movement requests
    private bool isLocked = false;

    private EncounterMap encounterMap;
    private int row;
    private int col;
    private ElementalStateMachine elementalStateMachine;

    //private TileState tileState;

    private OccupyingEntity occupyingEntity;

    private int rowCopy;
    private int colCopy;

    private string stateOfTile = " ";
    private MapTile tile;

    public int movementPoints = 5;  //amount of actions the user can take

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlaceCharacter(); //Currently just placing character at (5,8) but console states otherwise
    }

    // Update is called once per frame
    void Update()
    {
        //This will only process one move at a time
        if (!isMoving && !isLocked && movementPoints != 0)
        {
            //Accomodate two different types of moving
            System.Func<KeyCode, bool> inputFunction;

            //GetKeyDown gets a value per key that the user presses
            inputFunction = Input.GetKeyDown;

            //Checks for which movement the user is inputting        

            //Up Movement
            if ((inputFunction(KeyCode.UpArrow) || inputFunction(KeyCode.W)) && (row + 1 < encounterMap.GetMapTiles().Length))
            {
                AttemptMovement(row + 1, col, Vector2.up);
            }

            //Down Movement
            else if ((inputFunction(KeyCode.DownArrow) || inputFunction(KeyCode.S)) && (row > 0))
            {
                AttemptMovement(row - 1, col, Vector2.down);
            }

            //Left Movement
            else if ((inputFunction(KeyCode.LeftArrow) || inputFunction(KeyCode.A)) && (col > 0))
            {
                AttemptMovement(row, col - 1, Vector2.left);
            }

            //Right Movement
            else if ((inputFunction(KeyCode.RightArrow) || inputFunction(KeyCode.D)) && (col + 1 < encounterMap.GetMapTiles().Max(tileRow => tileRow.Length)))
            {
                AttemptMovement(row, col + 1, Vector2.right);
            }

            //Key press for abilities is M
            else if (inputFunction(KeyCode.M))
            {
                //Create ability cast
                Debug.Log("Character tries to cast ability");
                movementPoints--;
            }

            //Key press for dodge/block is N (no hit??)
            else if (inputFunction(KeyCode.N))
            {
                //Figure out how dodge works
                Debug.Log("Character tries to block");
                movementPoints--;
            }

            //Key press for melee attack is L (lunge)
            else if (inputFunction(KeyCode.L))
            {
                //Figure out how melee differs from abilities
                Debug.Log("Character tries to melee");
                movementPoints--;
            }

            //Debug.Log("Characters on Tile: " + currentTile.GetComponent<MapTile>());

            if (movementPoints == 0)
            {
                StartCoroutine(waitForEnemy(2f));

            }
        }
        //Might want to consider a confirm button
    }

    private void AttemptMovement(int nextRow, int nextCol, Vector2 dir)
    {
        if (encounterMap.GetMapTiles()[nextRow][nextCol].IsOccupied == true)
        {
            Debug.Log("There is a occupied block above");
        }
        else
        {
            StartCoroutine(Move(dir));

            row = nextRow;
            col = nextCol;
            gameObject.transform.SetParent(encounterMap.GetMapTiles()[nextRow][nextCol].gameObject.transform);
            movementPoints--;
        }
    }

    //Method for creating smooth movements between grid positions
    private IEnumerator Move(Vector2 direction)
    {
        //Recorder that were are moving so we dont accept more input
        isMoving = true;

        //Make a note of where we are and where we want to go
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

        //Make sure we go where we want
        transform.position = endPos;

        //We are no longer moving so we can accept another input
        isMoving = false;
    }

    public void SetEncounterMap(EncounterMap em)
    {
        encounterMap = em;
    }
    public void SetRow(int newRow)
    {
        row = newRow;
    }
    public void SetCol(int newCol)
    {
        col = newCol;
    }

    public int GetRow() { return row; }
    public int GetCol() { return col; }

    public void SetIsLocked(bool val)
    {
        isLocked = val;
    }

    public bool GetIsLocked() { return isLocked; }

    private IEnumerator waitForEnemy(float delayForEnemy)
    {
        Debug.Log("Waiting For Opponent to move");
        yield return new WaitForSeconds(delayForEnemy); // Wait for 3 seconds
        movementPoints = 5;
        Debug.Log("Enemies have attacked");
    }

}
