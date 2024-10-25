using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetImagesSprite : MonoBehaviour
{
    [SerializeField] private List<Image> _images;
    [SerializeField] private List<Sprite> _sprites;


    [Button]
    public void SetSprites()
    {
        if (_images.Count != _sprites.Count)
        {
            Debug.LogWarning("Images count != Sprites count!", gameObject);
        }

        int count = Mathf.Clamp(_images.Count, 0, _sprites.Count);
        for (int i = 0; i < count; i++)
        {
            _images[i].sprite = _sprites[i];
        }
    }
}
