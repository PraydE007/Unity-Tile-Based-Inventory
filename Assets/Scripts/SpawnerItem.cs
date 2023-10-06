using UnityEngine.UI;
using UnityEngine;

public class SpawnerItem : MonoBehaviour
{
    [SerializeField] Image _image = null;
    [SerializeField] InventoryItem _inventoryItem = null;

    public void OnClick() 
    {
        float x = _image.sprite.rect.width / 64 * 2;
        float y = _image.sprite.rect.height / 64 * 2;

        if (x < 1f)
            x = 1f;
        else if (x > 10f)
            x = 10f;
        if (y < 1f)
            y = 1f;
        else if (y > 10f)
            y = 10f;
        var item = Instantiate(_inventoryItem, DragAndDropParent.Instance.transform).SetupItem(
            _image.sprite,
            new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y))
        );
        InventoryGrid.Blanked = item;
        InventoryGrid.Blanked.SetReference(ref InventoryGrid.SelectedItem);
    }
}
