using UnityEngine;

namespace Utils.InputTool
{
    public abstract class InputEvents
    {
        public struct SingleTap
        {
            public Vector3 position;
        }

        public struct KeyDown
        {
            public KeyCode keyCode;
        }
        public struct KeyHold
        {
            public KeyCode keyCode;
        }
        public struct KeyUp
        {
            public KeyCode keyCode;
        }
        

        public struct SingleBeginDrag
        {
            public KeyCode keyCode;
            public Vector3 position;
        }
        public struct SingleDragging
        {
            public KeyCode keyCode;
            public Vector3 startPosition;
            public Vector3 previousPosition;
            public Vector3 currentPosition;
        }
        public struct SingleEndDrag
        {
            public KeyCode keyCode;
            public Vector3 startPosition;
            public Vector3 previousPosition;
        }
        
        
        public struct DoubleBeginDrag
        {
            public Vector3 position;
        }
        public struct DoubleDragging
        {
            public Vector3 startPosition;
            public Vector3 previousPosition;
            public Vector3 currentPosition;
        }
        public struct DoubleEndDrag
        {
            public Vector3 startPosition;
            public Vector3 previousPosition;
        }

        public struct ScrollBegin
        {
        }
        
        public struct Scrolling
        {
            public float delta;
        }

        public struct ScrollEnd
        {
        }
    }
}