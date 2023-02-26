using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Utils.UITool.ScreensArgs;

namespace Utils.UITool
{
    public class ScreensManager : MonoBehaviour
    {
        public static ScreensManager instance { get; private set; }
        
        private GameObject _blockObject;
        
        private Dictionary<ScreenId, Screen> _screens;
        private List<Screen> _cachedScreens;
        private List<Screen> _openedScreens;
        private List<Screen> _activeScreens;

        public Screen currentScreen => _openedScreens.Count > 0 ? _openedScreens[^1] : null;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Init()
        {
            _blockObject = new GameObject("BlockUIObject", 
                typeof(RectTransform),
                                typeof(Image));
            _blockObject.layer = LayerMask.NameToLayer("UI");
            _blockObject.transform.SetParent(instance.transform);
            var blockRT = _blockObject.GetComponent<RectTransform>();
            blockRT.anchorMin = new Vector2(0.0f, 0.0f);
            blockRT.anchorMax = new Vector2(1.0f, 1.0f);
            blockRT.anchoredPosition = Vector2.zero;
            blockRT.sizeDelta = Vector2.zero;
            var blockImage = _blockObject.GetComponent<Image>();
            blockImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            _blockObject.SetActive(false);
            
            
            _screens = new Dictionary<ScreenId, Screen>();
            _cachedScreens = new List<Screen>();
            _openedScreens = new List<Screen>();
            _activeScreens = new List<Screen>();
            
            var screenPrefabs = Resources.LoadAll<Screen>("Prefabs/UI/Screens");
            for (int i = 0; i < screenPrefabs.Length; i++)
            {
                var screen = screenPrefabs[i];
                _screens.Add(screen.screenId, screen);
            }

            Debug.Log($"Loaded {_screens.Count} screens.");
        }

        public async Task Open(ScreenId screenId, ScreenArgs args = null)
        {
            var nextScreen = GetScreen(screenId);
            nextScreen.transform.SetAsLastSibling();
            nextScreen.gameObject.SetActive(true);
            SetBlockingStatus(true);
            await nextScreen.Open(args);
            _openedScreens.Add(nextScreen);
            nextScreen.Activate();
            _activeScreens.Add(nextScreen);

            //Hide previous screens
            if (nextScreen.screenType == ScreenType.SCREEN)
            {
                if (_activeScreens.Count > 1)
                {
                    for (int i = _activeScreens.Count - 2; i >= 0; i--)
                    {
                        _activeScreens[i].Deactivate();
                        _activeScreens[i].gameObject.SetActive(false);
                        _activeScreens.Remove(_activeScreens[i]);
                    }
                }
            }
            
            if (nextScreen.isMain)
                Trim();
            
            SetBlockingStatus(false);
        }
        
        public async void OpenInstant(ScreenId screenId)
            => await Open(screenId, new ScreenArgs { isInstant = true });

        public async Task CloseLast(ScreenArgs args = null)
        {
            SetBlockingStatus(true);
            var currScreen = _openedScreens[^1];
            await CloseScreen(currScreen);
            SetBlockingStatus(false);
        }

        public async void CloseLastInstant()
            => await CloseLast(new ScreenArgs { isInstant = true });

        public async Task CloseAll()
        {
            while (_openedScreens.Count > 0)
                await CloseLast();
        }
        public void CloseAllInstant()
        {
            while (_openedScreens.Count > 0)
                CloseLastInstant();
        }

        
        public async void Trim(int skipCount = 1)
        {
            if (_openedScreens.Count > skipCount)
            {
                for (int i = _openedScreens.Count - (skipCount + 1); i >= 0; i--)
                {
                    var screen = _openedScreens[i];
                    await CloseScreen(screen, new ScreenArgs { isInstant = true });
                }
            }
        }

        public void SetBlockingStatus(bool status)
        {
            _blockObject.SetActive(status);
            if (status)
                _blockObject.transform.SetAsLastSibling();
            else
                _blockObject.transform.SetAsFirstSibling();
        }
        
        private async Task CloseScreen(Screen currScreen, ScreenArgs args = null)
        {
            if (currScreen.screenType == ScreenType.SCREEN)
            {
                //Show previous screens
                if (_openedScreens.Count > 1)
                {
                    for (int i = _openedScreens.Count - 2; i >= 0; i++)
                    {
                        var screen = _openedScreens[i];
                        screen.gameObject.SetActive(true);
                        screen.Activate();
                        _activeScreens.Add(screen);

                        if (screen.screenType == ScreenType.SCREEN)
                            break;
                    }
                }
            }

            currScreen.Deactivate();
            _activeScreens.Remove(currScreen);
            await currScreen.Close(args);
            _openedScreens.Remove(currScreen);
            currScreen.gameObject.SetActive(false);

            DisposeScreen(currScreen);
        }
        
        private Screen GetScreen(ScreenId screenId)
        {
            var screen = _cachedScreens.Find(n => n.screenId == screenId);
            if (screen != null)
            {
                _cachedScreens.Remove(screen);
                return screen;
            }

            screen = Instantiate(_screens[screenId], instance.transform);
            screen.Init();
            return screen;
        }

        private void DisposeScreen(Screen screen)
        {
            if (screen.isDestroyable)
            {
                Destroy(screen.gameObject);
            }
            else
            {
                screen.gameObject.SetActive(false);
                _cachedScreens.Add(screen);

                var split = screen.name.Split("_");
                if (split.Length > 1)
                    screen.name = split[0];
                screen.name += "_CACHED";
            }
        }
    }
}