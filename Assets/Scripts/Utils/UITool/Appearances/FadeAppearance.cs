using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.UITool.Appearances
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeAppearance : Appearance
    {
        [SerializeField] private float _showDuration;
        [SerializeField] private float _hideDuration;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        private TaskCompletionSource<bool> _playPromise;

        public override async Task PlayShow(bool isInstant = false)
        {
            if (isInstant)
            {
                SetVisible();
                return;
            }

            _playPromise = new TaskCompletionSource<bool>();
            StartCoroutine(PlayFade(1.0f, _showDuration));
            await _playPromise.Task;
            SetVisible();
        }

        public override async Task PlayHide(bool isInstant = false)
        {
            if (isInstant)
            {
                SetHidden();
                return;
            }
            
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _playPromise = new TaskCompletionSource<bool>();
            StartCoroutine(PlayFade(0.0f, _hideDuration));
            await _playPromise.Task;
            SetHidden();
        }

        private void SetVisible()
        {
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void SetHidden()
        {
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private IEnumerator PlayFade(float targetAlpha, float duration)
        {
            float initialAlpha = _canvasGroup.alpha;
            float progress = 0.0f;
            
            while (progress < 0.99f)
            {
                progress += Time.deltaTime / duration;
                progress = Mathf.Clamp01(progress);
                
                _canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, progress);
                yield return new WaitForEndOfFrame();
            }

            _playPromise.SetResult(true);
        }
    }
}