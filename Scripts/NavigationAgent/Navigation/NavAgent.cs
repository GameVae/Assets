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
    [RequireComponent(typeof(LineRenderer), typeof(AgentWayPoint))]
    public class NavAgent : MonoBehaviour
    {
        private NavRemote remote;

        #region Singleton
        private HexMap mapIns;
        private BreathFirstSearch breathFirstSearch;
        #endregion

        #region A* Pathfinding
        private int maxSearchLevel;
        private Vector3 currentTarget;
        private AStartAlgorithm aStar;
        private List<Vector3Int> path;
        #endregion

        private NavPathRenderer pathRenderer;

        public AnimatorController Anim;
        public NavOffset Offset;

        private float speed;
        private float maxAngular;
        private int curMoveStep;
        private int maxMoveStep;

        private FixedMovement fixedMove;

        public bool IsMoving { get; private set; }
        public WayPoint WayPoint { get; private set; }

        public Vector3Int EndPosition { get; private set; }
        public Vector3Int StartPosition { get; private set; }
        public Vector3Int CurrentPosition
        {
            get
            {
                if (mapIns != null)
                    return mapIns.WorldToCell(transform.position);
                return Vector3Int.zero;
            }
        }

        public ListUpgrade AgentType
        {
            get { return remote.Type; }
        }

        public int ID { get { return remote.AgentID; } }

        #region Initalize
        private void Awake()
        {
            pathRenderer = new NavPathRenderer(GetComponent<LineRenderer>());
            WayPoint = GetComponent<AgentWayPoint>();
            remote = GetComponent<NavRemote>();

            OffsetInit();

        }

        private void OffsetInit()
        {
            speed = Offset.MaxSpeed;
            maxAngular = Offset.MaxAngular;
            maxMoveStep = Offset.MaxMoveStep;
            maxSearchLevel = Offset.MaxSearchLevel;
        }

        private void Start()
        {
            mapIns = Singleton.Instance<HexMap>();
            breathFirstSearch = Singleton.Instance<BreathFirstSearch>();

            aStar = new AStartAlgorithm(mapIns, maxSearchLevel);
            fixedMove = new FixedMovement(this, Anim);

            MoveFinish();
        }
        #endregion

        private void Update()
        {
            //print("delta time: " + Time.deltaTime);
            MoveToTarget();

            fixedMove.Update();

            pathRenderer.Update();
        }

        private void FixedUpdate()
        {
            if (IsMoving)
                RotateToTarget(currentTarget);
        }

        private void AStarMoveToTarget()
        {
            if (IsMoving)
            {
                if (path.Count > 0)
                {
                    //Vector3 currentTarget = mapIns.CellToWorld(path[path.Count - 1]);
                    transform.position = Vector3.MoveTowards(
                        current: transform.position,
                        target: currentTarget,
                        maxDistanceDelta: Time.deltaTime * speed);

                    if ((transform.position - currentTarget).magnitude <= Constants.TINY_VALUE)
                    {
                        path.RemoveAt(path.Count - 1);
                        CalculateCurrentTarget();
                        curMoveStep++;

                        pathRenderer.RemoveForwardPoint(path.Count, GetPerHasGone());
                        if (maxMoveStep == curMoveStep) pathRenderer.Clear();
                    }
                }
                else
                {
                    MoveFinish();
                    IsMoving = false;
                    Anim.Stop(AnimState.Walking);
                }
            }
        }

        private void MoveToTarget()
        {
            if (maxMoveStep - 1 < curMoveStep/* || (path != null && path.Count == 0)*/)
            {
                path?.Clear();
            }
            AStarMoveToTarget();
        }

        private bool FindPath(Vector3Int start, Vector3Int end)
        {
            StartPosition = start;
            EndPosition = end;
            if (curMoveStep >= maxMoveStep || (path != null && path.Count == 0))
            {
                curMoveStep = 0;
            }

            IsMoving = aStar.FindPath(StartPosition.ZToZero(), EndPosition.ZToZero());
            path = aStar.Path;

            pathRenderer.LineRendererGenPath(
                foundPath: IsMoving,
                worldPath: mapIns.CellToWorld(path),
                t: GetPerHasGone(), // (count - (maxMoveStep - CurrentMoveStep)) * 1.0f / count
                position: transform.position,
                target: mapIns.CellToWorld(EndPosition)
                );
            return IsMoving;
        }

        private void MoveFinish()
        {
            Vector3Int currentCell = mapIns.WorldToCell(transform.position).ZToZero();
            if (!WayPoint.Binding())
            {
                if (breathFirstSearch.GetNearestCell(currentCell, out Vector3Int result))
                {
                    FindPath(currentCell, result);
                    curMoveStep = 0;
                }
            }
        }

        public float GetPerHasGone()
        {
            int count = path.Count;
            if (count == 0) return 1;
            return (count - (maxMoveStep - curMoveStep)) * 1.0f / count;
        }

        public bool StartMove(Vector3Int start, Vector3Int end)
        {
            if (IsMoving == false)
            {
                WayPoint.Unbinding();
            }

            bool foundPath = FindPath(start, end);
            if (foundPath)
            {
                Anim.Play(AnimState.Walking);
                CalculateCurrentTarget();
            }
            else if (IsMoving == true)
            {
                WayPoint.Binding();
                Anim.Stop(AnimState.Walking);

            }
            return foundPath;
        }

        public void StartMove(JSONObject r_move)
        {
            fixedMove.StartMove(r_move);
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
                Vector3 nextPosition = mapIns.CellToWorld(currentPath[i - 1]);
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
                currentTarget = mapIns.CellToWorld(path[count - 1]);
            }
        }

        private void OnDestroy()
        {
            if (!IsMoving)
            {
                WayPoint.Unbinding();
            }
        }

        #region Animation
        public void RotateToTarget(Vector3 target)
        {
            Vector3 direction = (target - transform.position) * maxAngular;
            transform.forward += direction * Time.deltaTime;
        }
        #endregion


#if UNITY_EDITOR
        public void ActiveNav()
        {
            remote.ActiveNav();
        }
#endif
    }
}