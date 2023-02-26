using System.Collections;
using UnityEngine;

namespace Utils.ProcessesTool
{
    public interface IProcessRunner
    {
        public Coroutine StartCoroutine(IEnumerator routine);
        public void StopCoroutine(Coroutine coroutine);
    }
}