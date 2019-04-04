using System;
using System.Collections.Generic;
using Animation;
using EnumCollect;
using Generic.Contants;
using Generic.Singleton;
using Map;
using PathFinding;
using UnityEngine;


namespace Entities.Navigation
{
    [RequireComponent(typeof(LineRenderer))]
    public class NavAgent : AgentMoveability
    {
        private AnimatorController animator;
       
        private NavPathRenderer pathRenderer;
        private SIO_MovementListener moveEvent;

        #region Singleton
        private BreathFirstSearch breathFirstSearch;
        #endregion

        #region A* Pathfinding
        private int maxSearchLevel;
        private Vector3 target;
        private AStarAlgorithm aStar;
        private List<Vector3Int> path;
        #endregion

        public AnimatorController Animator
        {
            get { return animator ?? (animator = GetComponent<AnimatorController>()); }
        }

        private float speed;
        private int curMoveStep;
        private int maxMoveStep;

        public Vector3Int EndPosition { get; private set; }
        public Vector3Int StartPosition { get; private set; }

        #region Initalize
        private void Awake()
        {
            pathRenderer = new NavPathRenderer(GetComponent<LineRenderer>());

            OffsetInit();
        }

        private void OffsetInit()
        {
            speed = Offset.MaxSpeed;
            maxMoveStep = Offset.MaxMoveStep;
            maxSearchLevel = Offset.MaxSearchLevel;
        }

        private void Start()
        {
            breathFirstSearch = Singleton.Instance<BreathFirstSearch>();

            aStar = new AStarAlgorithm(MapIns, maxSearchLevel);

            moveEvent = FindObjectOfType<SIO_MovementListener>();
        }
        #endregion

        protected override void Update()
        {
            base.Update();
     
            pathRenderer.Update();
        }

        private void AStarMoveToTarget()
        {
            if (IsMoving)
            {
                if (path.Count > 0)
                {
                    Vector3 position = Vector3.MoveTowards(
                        current: transform.position,
                        target: target,
                        maxDistanceDelta: Time.deltaTime * speed);                   
                    transform.position = position;

                    if ((transform.position - target).magnitude <= Constants.TINY_VALUE)
                    {
                        path.RemoveAt(path.Count - 1);
                        CalculateCurrentTarget();
                        curMoveStep++;

                        // path render
                        pathRenderer.RemoveForwardPoint(path.Count, GetPerHasGone());
                    }
                }
                else
                {
                    MoveFinish();
                }
            }
        }

        protected override void UpdateMove()
        {
            if (maxMoveStep - 1 < curMoveStep)
            {
                path?.Clear();
            }
            AStarMoveToTarget();
        }

        public bool StartMove(Vector3Int start, Vector3Int end)
        {
            if (Remote.FixedMove.IsMoving)
                Remote.FixedMove.Stop();

            bool foundPath = FindPath(start, end);
            StartMove(foundPath);
            if (foundPath)
            {
                moveEvent.Move(GetMovePath(), GetTimes(), CurrentPosition, Remote.Type, Remote.AgentID);
            }
            return foundPath;
        }

        private bool FindPath(Vector3Int start, Vector3Int end)
        {
            StartPosition = start;
            EndPosition = end;
            if (curMoveStep >= maxMoveStep || (path != null && path.Count == 0))
            {
                curMoveStep = 0;
            }

            bool foundPath = false;
            foundPath = aStar.FindPath(StartPosition.ZToZero(), EndPosition.ZToZero());
            path = aStar.Path;

            pathRenderer.LineRendererGenPath(
                foundPath: foundPath,
                worldPath: MapIns.CellToWorld(path),
                t: GetPerHasGone(), // (count - (maxMoveStep - CurrentMoveStep)) * 1.0f / count
                position: transform.position,
                target: MapIns.CellToWorld(EndPosition)
                );
            return foundPath;
        }

        private void MoveFinish()
        {
            Vector3Int currentCell = MapIns.WorldToCell(transform.position).ZToZero();
            if (!Binding())
            {
                if (breathFirstSearch.GetNearestCell(currentCell, out Vector3Int result))
                {
                    StartMove(currentCell, result);
                    curMoveStep = 0;
                }
            }
            else
            {
                Stop();
            }
        }

        public float GetPerHasGone()
        {
            int count = path.Count;
            if (count == 0) return 1;
            return (count - (maxMoveStep - curMoveStep)) * 1.0f / count;
        }

        private void Stop()
        {
            Animator.Stop(state: AnimState.Walking);
            IsMoving = false;
            path?.Clear();
            pathRenderer.Clear();
            Rotator.IsBlock = true;
        }

        private void StartMove(bool foundPath)
        {
            if (foundPath)
            {
                if (IsMoving == false)
                {
                    IsMoving = true;
                    Rotator.IsBlock = false;

                    Unbinding();
                    Animator.Play(AnimState.Walking);
                }
                CalculateCurrentTarget();
            }
            else // path not found
            {
                if (IsMoving == true)
                {
                    MoveFinish();
                }
            }
        }

        public List<float> GetTimes()
        {
            // return Offset.GetTime(GetMovePath(), transform.position);
            float distance = 0.0f;
            List<Vector3Int> currentPath = GetMovePath();
            List<float> separateTime = new List<float>();

            Vector3 currentPosition = transform.position;

            for (int i = currentPath.Count; i > 0; i--)
            {
                Vector3 nextPosition = MapIns.CellToWorld(currentPath[i - 1]);
                distance = Vector3.Distance(currentPosition, nextPosition);
                separateTime.Add(distance / Offset.MaxSpeed); // time
                currentPosition = nextPosition;
            }
            return separateTime;
        }

        public List<Vector3Int> GetMovePath()
        {
            return aStar.Truncate(path, maxMoveStep);
        }

        private void CalculateCurrentTarget()
        {
            int count = path.Count;
            if (count > 0)
            {
                target = MapIns.CellToWorld(path[count - 1]);
                Rotator.Target = target;
            }
        }

        private void OnDestroy()
        {
            if (!IsMoving)
            {
                Unbinding();
            }
        }


#if UNITY_EDITOR
        public void ActiveNav()
        {
            Remote.ActiveNav();
        }
#endif
    }
}