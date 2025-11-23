using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Cursor : MonoBehaviour
{
    private CursorController controller;
    private Camera mainCamera;
    private void Awake()
    {
        controller = new CursorController();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        controller.Enable();   
    }

    private void OnDisable()
    {
        controller.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        controller.mouse.Click.started += _ => StartedClick();
        controller.mouse.Click.performed += _ => EndedClick();
    }

    private void StartedClick()
    {

    }

    private void EndedClick()
    {
        DetectObject();
    }
    
    private void DetectObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(controller.mouse.Position.ReadValue<Vector2>());
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
        if (hit2D.collider != null) { 
            hit2D.collider.gameObject.GetComponent<MapTile>().Attack();
        }
    }
}
