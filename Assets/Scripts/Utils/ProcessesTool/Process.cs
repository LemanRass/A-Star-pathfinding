using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.ProcessesTool
{
    public abstract partial class Process
    {
        public bool isRunning { get; private set; }
        
        protected IProcessRunner _runner;
        private Coroutine _coroutine;
        

        public Process Run()
        {
            if (isRunning) throw new UnityException("Process already running!");
            _coroutine = _runner.StartCoroutine(AwaitProcessing());
            isRunning = true;
            return this;
        }
        
        protected virtual void Dispose()
        {
            if (_coroutine != null)
            {
                _runner.StopCoroutine(_coroutine);
                SetCancelled();
            }
            isRunning = false;
        }

        protected virtual void SetCompleted() { }
        protected virtual void SetCancelled() { }

        protected IEnumerator AwaitProcessing()
        {
            yield return Processing();
            SetCompleted();
            Release(this);
        }
        protected abstract IEnumerator Processing();
    }
    
    public abstract partial class Process
    {
        private static readonly List<Process> _cache;

        static Process() => _cache = new List<Process>(15);

        public static T CreateLoop<T>() where T : LoopProcess, new()
            => Create<T>();

        public static T CreateSingle<T>() where T : SingleProcess, new()
            => Create<T>();
        
        private static T Create<T>() where T : Process, new()
        {
            for (int i = 0; i < _cache.Count; i++)
            {
                if (_cache[i] is T tProcess)
                {
                    _cache.Remove(tProcess);
                    return tProcess;
                }
            }

            return new T();
        }

        public static void Release(Process process)
        {
            process.Dispose();
            _cache.Add(process);
        }
    }
}