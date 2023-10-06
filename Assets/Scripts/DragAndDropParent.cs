using UnityEngine;

public class DragAndDropParent : MonoBehaviour
{
    public static DragAndDropParent Instance = null;
    public static InventoryGrid SelectedGrid = null;

    InventoryGrid _selectedItemOwner = null;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SelectedGrid != null)
        {
            InventoryGrid.SelectedItem = SelectedGrid.GetItemOnCell(SelectedGrid.WorldToGrid(Input.mousePosition));
            if (InventoryGrid.SelectedItem == null)
                return;
            _selectedItemOwner = SelectedGrid;
            InventoryGrid.Blanked = Instantiate(InventoryGrid.SelectedItem, transform);
            InventoryGrid.Blanked.SetReference(ref InventoryGrid.SelectedItem);
        }

        if (Input.GetMouseButtonDown(1) && SelectedGrid != null)
        {
            InventoryGrid.SelectedItem = SelectedGrid.GetItemOnCell(SelectedGrid.WorldToGrid(Input.mousePosition));
            if (InventoryGrid.SelectedItem == null)
                return;
            SelectedGrid.RemoveItem(InventoryGrid.SelectedItem);
        }

        if (Input.GetMouseButton(0))
        {
            if (InventoryGrid.Blanked == null)
                return;
            InventoryGrid.Blanked.transform.position = Input.mousePosition;
            if (Input.GetKeyDown(KeyCode.R))
                InventoryGrid.Blanked.Rotated = !InventoryGrid.Blanked.Rotated;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (InventoryGrid.Blanked == null)
                return;
            if (SelectedGrid != null && SelectedGrid.PlaceItem(InventoryGrid.Blanked, SelectedGrid.WorldToGrid(Input.mousePosition)))
            {
                InventoryGrid.Blanked = null;
                if (_selectedItemOwner != null)
                    _selectedItemOwner.RemoveItem(InventoryGrid.SelectedItem);
            }
            else
            {
                Destroy(InventoryGrid.Blanked.gameObject);
                InventoryGrid.Blanked = null;
            }
        }
    }
}
