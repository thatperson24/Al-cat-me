using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        if (parentAfterDrag.gameObject.GetComponent<InventorySlot>().GetRecipeSlots() != null)
        {
            parentAfterDrag.gameObject.GetComponent<InventorySlot>().GetRecipeSlots().UpdateRecipe();
        }
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        if(transform.parent.GetComponent<InventorySlot>().GetRecipeSlots() != null)
        {
            transform.parent.GetComponent<InventorySlot>().GetRecipeSlots().UpdateRecipe();
        }
        image.raycastTarget = true;
    }
}
