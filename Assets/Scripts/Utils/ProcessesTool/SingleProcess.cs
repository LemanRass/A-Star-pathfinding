using System;

namespace Utils.ProcessesTool
{
    public abstract class SingleProcess : Process
    {
        private Action _onCompletedCallback;
        public SingleProcess OnCompleted(Action callback)
        {
            _onCompletedCallback = callback;
            return this;
        }
        protected sealed override void SetCompleted()
        {
            _onCompletedCallback?.Invoke();
        }


        private Action _onCancelledCallback;
        public SingleProcess OnCancelled(Action callback)
        {
            _onCancelledCallback = callback;
            return this;
        }
        protected sealed override void SetCancelled()
        {
            _onCancelledCallback?.Invoke();
        }

        protected override void Dispose()
        {
            _onCompletedCallback = null;
            _onCancelledCallback = null;
            base.Dispose();
        }
    }
}