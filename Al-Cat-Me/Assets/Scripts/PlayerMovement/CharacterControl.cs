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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //This will only process one move at a time
        if (!isMoving)
        {
            //Accomodate two different types of moving
            System.Func<KeyCode, bool> inputFunction;

            //GetKeyDown gets a value per key that the user presses
            inputFunction = Input.GetKeyDown;


            //Checks for which movement the user is inputting
            if (inputFunction(KeyCode.UpArrow) || inputFunction(KeyCode.W))
            {
                StartCoroutine(Move(Vector2.up));
            }
            else if (inputFunction(KeyCode.DownArrow) || inputFunction(KeyCode.S))
            {
                StartCoroutine(Move(Vector2.down));
            }
            else if (inputFunction(KeyCode.LeftArrow) || inputFunction(KeyCode.A))
            {
                StartCoroutine(Move(Vector2.left));
            }
            else if (inputFunction(KeyCode.RightArrow) || inputFunction(KeyCode.D))
            {
                StartCoroutine(Move(Vector2.right));
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

    public void CastSpell ()
    {

    }
}
