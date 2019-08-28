using System.Collections.Generic;
using Animation;
using EnumCollect;
using Generic.Contants;
using Generic.Singleton;
using UnityEngine;


namespace Entities.Navigation
{
    [RequireComponent(typeof(LineRenderer))]
    public class NavAgent : AgentMoveability
    {
        private AnimatorController animator;
        private NavPathRenderer pathRenderer;
        private NavAgentController agentController;
        private AgentRemote currentEnemy;

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
        public NavAgentController AgentController
        {
            get
            {
                return agentController ?? (agentController = Singleton.Instance<NavAgentController>());
            }
        }

        private float maxSpeed;
        private int curMoveStep;
        private int maxMoveStep;

        public Vector3Int EndPosition { get; private set; }
        public Vector3Int StartPosition { get; private set; }
        public AgentRemote TargetEnemy
        {
            get { return currentEnemy; }
        }

        private AStarAlgorithm AStar
        {
            get
            {
                return aStar ?? (aStar = new AStarAlgorithm(MapIns, maxSearchLevel));
            }
        }

        #region Initalize
        private void Awake()
        {
            pathRenderer = new NavPathRenderer(GetComponent<LineRenderer>());
            Initalize();
        }

        private void Initalize()
        {
            NavOffset offset = Remote.Offset;

            maxSpeed = offset.MaxSpeed;
            maxMoveStep = offset.MaxMoveStep;
            maxSearchLevel = offset.MaxSearchLevel;
        }
        #endregion

        protected override void Update()
        {
            base.Update();

            pathRenderer.Update();
        }

        #region CALCULATE POSITION
        private void AStarMoveToTarget()
        {
            if (IsMoving)
            {
                if (path.Count > 0)
                {
                    Vector3 position = Vector3.MoveTowards(
                        current: transform.position,
                        target: target,
                        maxDistanceDelta: Time.deltaTime * maxSpeed);
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

        private void CalculateCurrentTarget()
        {
            int count = path.Count;
            if (count > 0)
            {
                target = MapIns.CellToWorld(path[count - 1]);
                Rotator.Target = target;
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
        #endregion

        #region FIND PATH BY MAIN THREAD

        public bool StartMove(Vector3Int start, Vector3Int end)
        {
            if (Remote.FixedMove.IsMoving)
                Remote.FixedMove.Stop();

            bool foundPath = FindPath(start, end);
            InitForMove(foundPath);
            //if (foundPath)
            //{
            //moveEvent.Move(GetMovePath(), GetTimes(), CurrentPosition, Remote);
            //}
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
            foundPath = AStar.FindPath(StartPosition.ZToZero(), EndPosition.ZToZero());
            path = AStar.Path;

            pathRenderer.LineRendererGenPath(
                foundPath: foundPath,
                worldPath: MapIns.CellToWorld(path),
                t: GetPerHasGone(), // (count - (maxMoveStep - CurrentMoveStep)) * 1.0f / count
                position: transform.position,
                target: MapIns.CellToWorld(EndPosition)
                );
            return foundPath;
        }

        #endregion

        #region FIND PATH BY ANOTHER THREAD
        private void FindPathDone_Callback(AStarAlgorithm aStar, bool found)
        {
            path = aStar.Path;

            AgentController.ThreadHelper.MainThreadInvoke(() => InitalizeMove(found));
            AgentController.FindPathDone_OnlyMainThread(this, found);
        }

        private void InitalizeMove(bool foundPath)
        {
            if (Remote.FixedMove.IsMoving)
                Remote.FixedMove.Stop();

            InitForMove(foundPath);

            pathRenderer.LineRendererGenPath(
               foundPath: foundPath,
               worldPath: MapIns.CellToWorld(path),
               t: GetPerHasGone(), // (count - (maxMoveStep - CurrentMoveStep)) * 1.0f / count
               position: transform.position,
               target: MapIns.CellToWorld(EndPosition)
               );
        }

        public void AsyncStartMove(Vector3Int start, Vector3Int end, AgentRemote targetEnemy)
        {
            StartPosition = start;
            EndPosition = end;
            currentEnemy = targetEnemy;

            if (curMoveStep >= maxMoveStep || (path != null && path.Count == 0))
            {
                curMoveStep = 0;
            }

            AStarAlgorithm.FindInfo info = new AStarAlgorithm.FindInfo()
            {
                StartPosition = start,
                EndPosition = end,
                DoneCallback = FindPathDone_Callback
            };
            AStar.FindPath(info);
        }

        #endregion

        public void Stop()
        {
            Animator.Stop(state: AnimState.Walking);
            IsMoving = false;
            path?.Clear();
            pathRenderer.Clear();
            Rotator.IsBlock = true;
        }

        public void MoveFinish()
        {
            Vector3Int currentCell = MapIns.WorldToCell(transform.position).ZToZero();
            if (!Remote.Binding())
            {
                if (MapIns.GetNearestPosition(currentCell, out Vector3Int result))
                {
                    // StartMove(currentCell, result); // main thread
                    // AsyncStartMove(currentCell, result); // another thread
                    AgentController.MoveAgent(this, currentCell, result, currentEnemy);
                    curMoveStep = 0;
                }
            }
            else
            {
                Stop();
            }
        }

        private void InitForMove(bool foundPath)
        {
            if (foundPath)
            {
                Remote.StopAttackEffect();
                if (IsMoving == false)
                {
                    IsMoving = true;
                    Rotator.IsBlock = false;

                    Remote.Unbinding();
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

        public float GetPerHasGone()
        {
            int count = path.Count;
            if (count == 0) return 1;
            return (count - (maxMoveStep - curMoveStep)) * 1.0f / count;
        }

        public List<float> GetTimes()
        {
            float distance = 0.0f;
            List<Vector3Int> currentPath = GetMovePath();
            List<float> separateTime = new List<float>();

            Vector3 currentPosition = transform.position;

            for (int i = currentPath.Count; i > 0; i--)
            {
                Vector3 nextPosition = MapIns.CellToWorld(currentPath[i - 1]);
                distance = Vector3.Distance(currentPosition, nextPosition);
                separateTime.Add(distance / maxSpeed); // time
                currentPosition = nextPosition;
            }
            return separateTime;
        }

        public List<Vector3Int> GetMovePath()
        {
            return AStar.Truncate(path, maxMoveStep);
        }

        private void OnDestroy()
        {
            if (!IsMoving)
            {
                Remote.Unbinding();
            }
        }
    }
}