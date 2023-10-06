using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] InventoryItem _inventoryItem = null;
    [SerializeField] Transform _grid = null;
    RectTransform _rectTransform = null;

    public Vector2Int Size;

    public static InventoryItem SelectedItem = null;
    public static InventoryItem Blanked = null;

    public readonly List<InventoryItem> StoredItems = new List<InventoryItem>();

    void Awake()
    {
        _grid = transform.Find("Grid");
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = new Vector2(64f * Size.x, 64f * Size.y);
    }

    public bool PlaceItem(InventoryItem item, Vector2Int pos)
    {
        if (!CheckGridBounds(pos, item.Size))
            return false;
        for (int i = 0; i < StoredItems.Count; i++)
        {
            if (item.ReferencedTo == StoredItems[i])
                continue;
            if (CheckItemCollision(pos, item.Size, LocalToGrid(GetItemPosition(StoredItems[i])), StoredItems[i].Size))
                return false;
        }

        item.transform.SetParent(_grid);
        item.SetPosition(pos, Vector2.zero);
        item.ApplySize();
        StoredItems.Add(item);
        return true;
    }

    public bool RemoveItem(InventoryItem item)
    {
        if (StoredItems.Remove(item))
        {
            Destroy(item.gameObject);
            return true;
        }
        return false;
    }

    public Vector2Int WorldToGrid(Vector2 pos)
    {
        float realX = Normalize(pos.x, transform.position.x, transform.position.x + _rectTransform.rect.width);
        float realY = Normalize(pos.y, transform.position.y, transform.position.y + _rectTransform.rect.height);
        return new Vector2Int((int)(realX * Size.x), (int)(-realY * Size.y));
    }

    public static Vector2Int LocalToGrid(Vector2 pos)
    {
        return new Vector2Int((int)(pos.x / 64f), (int)(pos.y / 64f));
    }

    public Vector2 GetItemPosition(InventoryItem item)
    {
        return new Vector2(item.transform.localPosition.x, -item.transform.localPosition.y);
    }

    public InventoryItem GetItemOnCell(Vector2Int pos)
    {
        foreach (var item in StoredItems)
        {
            var cellPos = LocalToGrid(GetItemPosition(item));
            if (cellPos.x <= pos.x && cellPos.x + item.Size.x > pos.x &&
                cellPos.y <= pos.y && cellPos.y + item.Size.y > pos.y)
            {
                return item;
            }
        }
        return null;
    }

    bool CheckItemCollision(Vector2Int posA, Vector2Int sizeA, Vector2Int posB, Vector2Int sizeB)
    {
        Vector2Int[] a = new Vector2Int[]
        {
            posA,
            new Vector2Int(posA.x + sizeA.x - 1, posA.y),
            new Vector2Int(posA.x, posA.y + sizeA.y - 1),
            new Vector2Int(posA.x + sizeA.x - 1, posA.y + sizeA.y - 1)
        };
        Vector2Int[] b = new Vector2Int[]
        {
            posB,
            new Vector2Int(posB.x + sizeB.x - 1, posB.y),
            new Vector2Int(posB.x, posB.y + sizeB.y - 1),
            new Vector2Int(posB.x + sizeB.x - 1, posB.y + sizeB.y - 1)
        };

        // Points check
        for (int i = 0; i < 4; i++)
        {
            if (a[0].x <= b[i].x && b[i].x <= a[3].x &&
                a[0].y <= b[i].y && b[i].y <= a[3].y)
            {
                return true;
            }
            if (b[0].x <= a[i].x && a[i].x <= b[3].x &&
                b[0].y <= a[i].y && a[i].y <= b[3].y)
            {
                return true;
            }
        }
        // Crossing check
        if (a[0].y <= b[0].y && b[0].y <= a[3].y &&
            b[0].x <= a[0].x && a[0].x <= b[3].x)
        {
            if (sizeA.x < sizeB.x &&
                sizeB.y < sizeA.y)
            {
                return true;
            }
        }
        if (b[0].y <= a[0].y && a[0].y <= b[3].y &&
            a[0].x <= b[0].x && b[0].x <= a[3].x)
        {
            if (sizeB.x < sizeA.x &&
                sizeA.y < sizeB.y)
            {
                return true;
            }
        }
        return false;
    }

    bool CheckGridBounds(Vector2Int pos, Vector2Int size)
    {
        if (pos.x < 0 || pos.y < 0)
            return false;
        if (pos.x + size.x > Size.x)
            return false;
        if (pos.y + size.y > Size.y)
            return false;
        return true;
    }

    float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DragAndDropParent.SelectedGrid = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DragAndDropParent.SelectedGrid = this;
    }
}
