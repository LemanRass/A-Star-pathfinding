using System;
using UnityEngine;

namespace Utils.InputTool
{
    public class InputManager : MonoBehaviour
    {
        public static event Action<InputEvents.SingleTap> onSingleTap;
        public static event Action<InputEvents.KeyDown> onKeyDown;
        public static event Action<InputEvents.KeyHold> onKeyHold;
        public static event Action<InputEvents.KeyUp> onKeyUp;
        public static event Action<InputEvents.SingleBeginDrag> onSingleBeginDrag;
        public static event Action<InputEvents.SingleDragging> onSingleDragging;
        public static event Action<InputEvents.SingleEndDrag> onSingleEndDrag;
        public static event Action<InputEvents.DoubleBeginDrag> onDoubleBeginDrag;
        public static event Action<InputEvents.DoubleDragging> onDoubleDragging;
        public static event Action<InputEvents.DoubleEndDrag> onDoubleEndDrag;
        public static event Action<InputEvents.ScrollBegin> onScrollBegin; 
        public static event Action<InputEvents.Scrolling> onScrolling;
        public static event Action<InputEvents.ScrollEnd> onScrollEnd;

        private static bool _isBlocked;

        private SingleTapInputChecker _singleTap;
        private SingleDragInputChecker _singleLMBDrag;
        private SingleDragInputChecker _singleRMBDrag;
        private DoubleDragInputChecker _doubleDrag;
        private KeyHoldInputChecker _rmbHold;
        private KeyUpInputChecker _lmbKeyUp;
        private KeyDownInputChecker _lmbKeyDown;
        private ScrollMouseInputChecker _scrollMouse;
        private ScrollTouchesInputChecker _scrollTouches;
    

        static InputManager()
        {
            var instance = new GameObject("InputManager")
                .AddComponent<InputManager>();
            DontDestroyOnLoad(instance.gameObject);
        }

        public static void SetBlockedStatus(bool status) => _isBlocked = status;
        
        private void Awake()
        {
            _singleTap = new SingleTapInputChecker();
            _singleLMBDrag = new SingleDragInputChecker(KeyCode.Mouse0);
            _singleRMBDrag = new SingleDragInputChecker(KeyCode.Mouse1);
            _doubleDrag = new DoubleDragInputChecker();
            _rmbHold = new KeyHoldInputChecker(KeyCode.Mouse1);
            _lmbKeyUp = new KeyUpInputChecker(KeyCode.Mouse0);
            _lmbKeyDown = new KeyDownInputChecker(KeyCode.Mouse0);
            _scrollMouse = new ScrollMouseInputChecker();
            _scrollTouches = new ScrollTouchesInputChecker();
        }

        private bool _isDouble;
        
        private void Update()
        {
            if (_isBlocked) return;
            
            _isDouble = false;
            if (_doubleDrag.Update())
            {
                if (!_isDouble)
                    _isDouble = true;
            }
            
            if (_scrollMouse.Update())
            {
                if (!_isDouble)
                    _isDouble = true;
            }
            
            if (_scrollTouches.Update())
            {
                if (!_isDouble)
                    _isDouble = true;
            }
            
            if(_isDouble)
                return;

            _singleTap.Update();
            _singleLMBDrag.Update();
            _singleRMBDrag.Update();
            _rmbHold.Update();
            _lmbKeyUp.Update();
            _lmbKeyDown.Update();
        }
    
        private class KeyDownInputChecker
        {
            private readonly KeyCode _keyCode;

            public KeyDownInputChecker(KeyCode keyCode)
            {
                _keyCode = keyCode;
            }
        
            public bool Update()
            {
                if (Input.GetKeyDown(_keyCode))
                {
                    onKeyDown?.Invoke(new InputEvents.KeyDown
                    {
                        keyCode = _keyCode
                    });
                    return true;
                }

                return false;
            }
        }
        private class KeyHoldInputChecker
        {
            private readonly KeyCode _keyCode;

            public KeyHoldInputChecker(KeyCode keyCode)
            {
                _keyCode = keyCode;
            }

            public bool Update()
            {
                if (Input.GetKey(_keyCode))
                {
                    onKeyHold?.Invoke(new InputEvents.KeyHold
                    {
                        keyCode = _keyCode
                    });
                    return true;
                }

                return false;
            }
        }
        private class KeyUpInputChecker
        {
            private readonly KeyCode _keyCode;

            public KeyUpInputChecker(KeyCode keyCode)
            {
                _keyCode = keyCode;
            }
        
            public bool Update()
            {
                if (Input.GetKeyUp(_keyCode))
                {
                    onKeyUp?.Invoke(new InputEvents.KeyUp
                    {
                        keyCode = _keyCode
                    });
                    
                    return true;
                }

                return false;
            }
        }
        private class SingleTapInputChecker
        {
            public bool Update()
            {
                if (Input.GetMouseButtonUp(0))
                {
                    onSingleTap?.Invoke(new InputEvents.SingleTap
                    {
                        position = Input.mousePosition
                    });

                    return true;
                }

                return false;
            }
        }
        private class SingleDragInputChecker
        {
            private readonly KeyCode _keyCode;
            private Vector3 _startDragPoint;
            private Vector3 _previousDragPoint;
            private bool _isDragging;

            public SingleDragInputChecker(KeyCode keyCode)
            {
                _keyCode = keyCode;
            }
        
            public bool Update()
            {
                if (Input.GetKeyDown(_keyCode))
                    OnDragBegin();

                if (Input.GetKey(_keyCode) && _isDragging)
                    OnDragging();

                if (Input.GetKeyUp(_keyCode) && _isDragging)
                    OnDragEnd();

                return _isDragging;
            }

            private void OnDragBegin()
            {
                _startDragPoint = Input.mousePosition;
                _previousDragPoint = _startDragPoint;
                _isDragging = true;
                onSingleBeginDrag?.Invoke(new InputEvents.SingleBeginDrag
                {
                    keyCode = _keyCode,
                    position = _startDragPoint
                });
            }

            private void OnDragging()
            {
                var currentDragPoint = Input.mousePosition;
                onSingleDragging?.Invoke(new InputEvents.SingleDragging
                {
                    keyCode = _keyCode,
                    startPosition = _startDragPoint,
                    previousPosition = _previousDragPoint,
                    currentPosition = currentDragPoint
                });
                _previousDragPoint = currentDragPoint;
            }

            private void OnDragEnd()
            {
                _isDragging = false;
                onSingleEndDrag?.Invoke(new InputEvents.SingleEndDrag
                {
                    keyCode = _keyCode,
                    startPosition = _startDragPoint,
                    previousPosition = _previousDragPoint
                });
            }
        }
        private class DoubleDragInputChecker
        {
            private Vector3 _startDragPoint;
            private Vector3 _previousDragPoint;
            private bool _isDragging;
        
            public bool Update()
            {
                if (Input.touchCount == 2)
                {
                    var touch2 = Input.GetTouch(1);
                
                    switch (touch2.phase)
                    {
                        case TouchPhase.Began:
                            OnDragBegin();
                            break;
                    
                        case TouchPhase.Moved:
                            OnDragging();
                            break;
                    }
                }
                else
                {
                    if (_isDragging)
                        OnDragEnd();
                }

                return _isDragging;
            }

            private void OnDragBegin()
            {
                _startDragPoint = Input.GetTouch(1).position;
                _previousDragPoint = _startDragPoint;
                _isDragging = true;
                onDoubleBeginDrag?.Invoke(new InputEvents.DoubleBeginDrag
                {
                    position = _startDragPoint
                });
            }

            private void OnDragging()
            {
                var currentDragPoint = Input.GetTouch(1).position;
                onDoubleDragging?.Invoke(new InputEvents.DoubleDragging
                {
                    startPosition = _startDragPoint,
                    previousPosition = _previousDragPoint,
                    currentPosition = currentDragPoint
                });
                _previousDragPoint = currentDragPoint;
            }
        
            private void OnDragEnd()
            {
                _isDragging = false;
                onDoubleEndDrag?.Invoke(new InputEvents.DoubleEndDrag
                {
                    startPosition = _startDragPoint,
                    previousPosition = _previousDragPoint
                });
            }
        }

        private class ScrollMouseInputChecker
        {
            private bool _isScrolling;

            public bool Update()
            {
                if (Input.mouseScrollDelta.magnitude > 0.01f)
                {
                    if (!_isScrolling)
                        OnScrollBegin();

                    OnScrolling();
                }
                else
                {
                    if (_isScrolling)
                        OnScrollEnd();
                }

                return _isScrolling;
            }

            private void OnScrollBegin()
            {
                _isScrolling = true;
                onScrollBegin?.Invoke(new InputEvents.ScrollBegin());
            }

            private void OnScrolling()
            {
                onScrolling?.Invoke(new InputEvents.Scrolling
                {
                    delta = Input.mouseScrollDelta.y * 50.0f * -1.0f
                });
            }

            private void OnScrollEnd()
            {
                _isScrolling = false;
                onScrollEnd?.Invoke(new InputEvents.ScrollEnd());
            }
        }

        private class ScrollTouchesInputChecker
        {
            private float _initialScrollDistance;
            private bool _isScrolling;
            
            public bool Update()
            {
                if (Input.touchCount == 2)
                {
                    var touch1 = Input.GetTouch(0);
                    var touch2 = Input.GetTouch(1);

                    if (!_isScrolling)
                        OnScrollBegin(touch1, touch2);

                    switch (touch1.phase)
                    {
                        case TouchPhase.Moved:
                            OnScrolling(touch1, touch2);
                            break;
                        
                        case TouchPhase.Canceled:
                        case TouchPhase.Ended:
                            _isScrolling = false;
                            break;
                    }

                    switch (touch2.phase)
                    {
                        case TouchPhase.Moved:
                            OnScrolling(touch1, touch2);
                            break;
                        
                        case TouchPhase.Canceled:
                        case TouchPhase.Ended:
                            _isScrolling = false;
                            break;
                    }
                }
                else
                {
                    OnScrollEnd();
                }

                return _isScrolling;
            }
            
            private void OnScrollBegin(Touch touch1, Touch touch2)
            {
                _initialScrollDistance = Vector2.Distance(touch1.position, touch2.position);
                _isScrolling = true;
                onScrollBegin?.Invoke(new InputEvents.ScrollBegin());
            }

            private void OnScrolling(Touch touch1, Touch touch2)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                float delta = _initialScrollDistance - currentDistance;
                onScrolling?.Invoke(new InputEvents.Scrolling { delta = delta });
            }

            private void OnScrollEnd()
            {
                //_isScrolling = false;
                onScrollEnd?.Invoke(new InputEvents.ScrollEnd());
            }
        }
    }
}