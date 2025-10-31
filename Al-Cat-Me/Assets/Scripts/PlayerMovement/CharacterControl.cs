using System.Linq;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using UnityEngine.Scripting.APIUpdating;

public class CharacterControl : MonoBehaviour
{

    public float movementSpeed= 5f;
    public Transform moveLocation;

    [SerializeField] private float moveDuration = 0.1f;//Time in seconds to move between one grid position to the next
    public bool isMoving = false;   //Helps with checking movement requests
    private bool isLocked = false;

    private EncounterMap encounterMap;
    private int row;
    private int col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //This will only process one move at a time
        if (!isMoving && !isLocked)
        {
            //Accomodate two different types of moving
            System.Func<KeyCode, bool> inputFunction;

            //GetKeyDown gets a value per key that the user presses
            inputFunction = Input.GetKeyDown;


            //Checks for which movement the user is inputting
            if ((inputFunction(KeyCode.UpArrow) || inputFunction(KeyCode.W)) && (row + 1 < encounterMap.GetMapTiles().Length))
            {
                StartCoroutine(Move(Vector2.up));
                gameObject.transform.SetParent(encounterMap.GetMapTiles()[++row][col].gameObject.transform);
            }
            else if ((inputFunction(KeyCode.DownArrow) || inputFunction(KeyCode.S)) && (row > 0))
            {
                StartCoroutine(Move(Vector2.down));
                gameObject.transform.SetParent(encounterMap.GetMapTiles()[--row][col].gameObject.transform);
            }
            else if ((inputFunction(KeyCode.LeftArrow) || inputFunction(KeyCode.A)) && (col > 0))
            {
                StartCoroutine(Move(Vector2.left));
                gameObject.transform.SetParent(encounterMap.GetMapTiles()[row][--col].gameObject.transform);
            }
            else if ((inputFunction(KeyCode.RightArrow) || inputFunction(KeyCode.D)) && (col + 1 < encounterMap.GetMapTiles().Max(tileRow => tileRow.Length)))
            {
                StartCoroutine(Move(Vector2.right));
                gameObject.transform.SetParent(encounterMap.GetMapTiles()[row][++col].gameObject.transform);
            }

        }
        //Still need to take into account collsions (Tiles )
        //Might want to consider a confirm button

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
}
