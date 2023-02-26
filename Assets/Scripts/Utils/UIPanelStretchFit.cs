using UnityEngine;

namespace Utils
{
    public class UIPanelStretchFit : MonoBehaviour
    {
        [SerializeField] private RectTransform _referenceRectTransform;
        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();

            float horizontalScale = _referenceRectTransform.rect.width / _rectTransform.rect.width;
            float verticalScale = _referenceRectTransform.rect.height / _rectTransform.rect.height;

            _rectTransform.localScale = new Vector3(horizontalScale, verticalScale, 1.0f);
        }
    }
}