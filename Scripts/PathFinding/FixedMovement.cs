using Animation;
using EnumCollect;
using Generic.Contants;
using Generic.Singleton;
using ManualTable.Row;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FixedMovement
{
    private bool isMoving;
    private HexMap mapIns;
    private float speed;
    private List<MoveStep> moveSteps;
    private List<Vector3Int> path;
    private NavAgent targetAgent;
    private MovementSerMessageHandler moveHandler;
    private AnimatorController anim;

    public List<Vector3Int> Path
    { get { return path; } }

    public FixedMovement(NavAgent agent, AnimatorController animatorController)
    {
        isMoving = false;

        path = new List<Vector3Int>();
        mapIns = Singleton.Instance<HexMap>();
        moveHandler = new MovementSerMessageHandler();
        targetAgent = agent;
        anim = animatorController;
    }

    public void Update()
    {
        if (isMoving)
        {
            if (path.Count > 0)
            {
                Vector3 currentTarget = mapIns.CellToWorld(path[path.Count - 1]);

                targetAgent.transform.position = Vector3.MoveTowards(
                    current: targetAgent.transform.position,
                    target: currentTarget,
                    maxDistanceDelta: Time.deltaTime * speed);

                targetAgent.RotateToTarget(currentTarget);

                if ((targetAgent.transform.position - currentTarget).magnitude <= Constants.TINY_VALUE)
                {
                    NextStep();
                }
            }
            else
            {
                Stop();
            }
        }
    }

    public void StartMove(JSONObject r_move)
    {
        moveSteps = moveHandler.HandlerEvent(r_move);
        path = moveHandler.GetPath(moveSteps);

        isMoving = true;
        Vector3 target = mapIns.CellToWorld(moveSteps[0].NextPosition);
        speed = CalculateSpeed(targetAgent.transform.position, target, 0, moveSteps[0].TimeSecond);
        targetAgent.WayPoint.Unbinding();
        anim.Play(AnimState.Walking);
    }

    private void NextStep()
    {
        float lastTime = moveSteps[0].TimeSecond;

        Path.RemoveAt(Path.Count - 1);
        moveSteps.RemoveAt(0);

        Vector3 target = mapIns.CellToWorld(moveSteps[0].NextPosition);
        speed = CalculateSpeed(targetAgent.transform.position, target, lastTime, moveSteps[0].TimeSecond);

    }

    private float CalculateSpeed(Vector3 pos, Vector3 tar, float lastTime, float targetTime)
    {
        return Vector3.Distance(pos, tar) / (targetTime - lastTime);
    }

    private void Stop()
    {
        anim.Stop(AnimState.Walking);
        targetAgent.WayPoint.Binding();
    }

}
