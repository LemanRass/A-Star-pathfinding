using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Utils.UITool.Appearances;
using Utils.UITool.ScreensArgs;

namespace Utils.UITool
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class Screen : MonoBehaviour
    {
        [SerializeField] private Appearance _appearance;
        
        public ScreenId screenId;
        public ScreenType screenType;
        public bool isDestroyable;
        public bool isMain;

        /// <summary>
        /// Executes once per screen prefab.
        /// </summary>
        public virtual void Init()
        {
            //Debug.Log($"{name} Init");
        }

        /// <summary>
        /// Executes once per opening screen.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual Task Open(ScreenArgs args = null)
        {
            if (args != null)
                return _appearance.PlayShow(args.isInstant);
            
            //Debug.Log($"{name} Open");
            return _appearance.PlayShow();
        }

        /// <summary>
        /// Executes once per closing screen (destroying or moving to cache).
        /// </summary>
        /// <returns></returns>
        public virtual Task Close(ScreenArgs args = null)
        {
            if (args != null)
                return _appearance.PlayHide(args.isInstant);
            
            //Debug.Log($"{name} Close");
            return _appearance.PlayHide();
        }
        
        /// <summary>
        /// Executes each time screen is activated.
        /// </summary>
        public virtual void Activate()
        {
            //Debug.Log($"{name} Show");
            var split = name.Split("_");
            if (split.Length > 1)
                name = split[0];
            
            //gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Executes each time screen is deactivated.
        /// </summary>
        public virtual void Deactivate()
        {
            //Debug.Log($"{name} Hide");
            name += "_DEACTIVATED";
            //gameObject.SetActive(false);
        }
    }
}