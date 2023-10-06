using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] RectTransform _rectTransform = null;
    [SerializeField] Image _image = null;

    public InventoryItem ReferencedTo { get; private set; } = null;
    [SerializeField] bool _rotated = false;
    public bool Rotated
    {
        get
        {
            return _rotated;
        }
        set
        {
            if (value != Rotated)
            {
                _rotated = value;
                ApplyRotation();
                ApplySize();
            }
        }
    }
    [SerializeField] Vector2Int _size = Vector2Int.one;
    public Vector2Int Size
    {
        get
        {
            return Rotated ? new Vector2Int(_size.y, _size.x) : _size;
        }
        private set
        {
            _size = value;
        }
    }

    public InventoryItem SetupItem(Sprite sprite)
    {
        return SetupItem(sprite, Size);
    }

    public InventoryItem SetupItem(Sprite sprite, Vector2Int size)
    {
        _image.sprite = sprite;
        Size = size;
        ApplySize();
        return this;
    }

    public void ApplySize()
    {
        _rectTransform.sizeDelta = new Vector2(Size.x * 64, Size.y * 64);
        _rectTransform.localScale = Vector3.one;
    }

    void ApplyRotation()
    {
        var imageTransform = _image.transform as RectTransform;
        if (Rotated)
            imageTransform.rotation = Quaternion.Euler(0, 0, 90);
        else
            imageTransform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetPosition(Vector2Int pos, Vector2 offset)
    {
        _rectTransform.localPosition = new Vector3(-offset.x + pos.x * 64, offset.y + -pos.y * 64, 0);
    }

    public void SetReference(ref InventoryItem reference)
    {
        ReferencedTo = reference;
    }
}
