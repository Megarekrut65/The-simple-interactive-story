using SimpleStory.Data;
using UnityEngine;
using UnityEngine.UI;
using Rect = UnityEngine.Rect;

namespace SimpleStory
{
    public class ImagesConstructor: MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private RectTransform canvasRect;

        public void DrawImage(CanvasImage canvasImage, Sprite sprite)
        {
            if(canvasImage == null || sprite == null) return;
            
            GameObject instance = new GameObject(canvasImage.img);
            instance.transform.SetParent(container.transform, false);
            
            RectTransform rectTransform = instance.AddComponent<RectTransform>();
            Rect rect = canvasRect.rect;
            rectTransform.sizeDelta = new Vector3((float)canvasImage.rect.width , 
                (float)canvasImage.rect.height );
            
            rectTransform.anchoredPosition = new Vector3((float)canvasImage.rect.x- rect.width/2 +
                                                         (float)canvasImage.rect.width/2, 
                -((float)canvasImage.rect.y - rect.height/2 + (float)canvasImage.rect.height/2));

            instance.transform.rotation = Quaternion.Euler(0, 0, -(float)canvasImage.rect.rotation);
            Image image = instance.AddComponent<Image>();
            image.sprite = sprite;
            image.preserveAspect = true;
        }

        public void Clear()
        {
            foreach (Transform child in container.transform) {
                Destroy(child.gameObject);
            }
        }
    }
}