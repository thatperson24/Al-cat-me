using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private RecipeSlots recipeSlots;

    void Start()
    {

    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItems draggableItem = dropped.GetComponent<DraggableItems>();
            draggableItem.parentAfterDrag = transform;
        }
    }
    public RecipeSlots GetRecipeSlots()
    {
        return recipeSlots;
    }
}
