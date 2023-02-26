using System.Threading.Tasks;
using UnityEngine;

namespace Utils.UITool.Appearances
{
    public abstract class Appearance : MonoBehaviour
    {
        public abstract Task PlayShow(bool isInstant = false);
        public abstract Task PlayHide(bool isInstant = false);
    }
}