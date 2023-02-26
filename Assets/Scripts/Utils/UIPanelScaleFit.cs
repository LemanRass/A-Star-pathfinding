using UnityEngine;

namespace Utils
{
    public class UIPanelScaleFit : MonoBehaviour
    {
        public enum FitType
        {
            INNER,
            OUTER
        }

        [SerializeField] private RectTransform _referenceRectTransform;
        [SerializeField] private FitType _fitType;

        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            Rebuild();
        }

        private void Rebuild()
        {
            var verticalScale = _referenceRectTransform.rect.height / _rectTransform.rect.height;

            var horizontalScale = _referenceRectTransform.rect.width / _rectTransform.rect.width;

            float totalScale = 1.0f;

            switch (_fitType)
            {
                case FitType.INNER:
                    totalScale = Mathf.Min(verticalScale, horizontalScale);
                    break;


                case FitType.OUTER:
                    totalScale = Mathf.Max(verticalScale, horizontalScale);
                    break;
            }

            _rectTransform.localScale = new Vector3(totalScale, totalScale, totalScale);
        }
    }
}