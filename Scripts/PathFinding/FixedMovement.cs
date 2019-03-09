using Generic.Contants;
using Generic.Singleton;
using ManualTable.Row;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMovement
{
    private bool isMoving;
    private Vector3 target;
    private HexMap mapIns;
    private float speed;
    private List<MoveStep> moveSteps;
    private List<Vector3Int> path;
    private NavAgent targetAgent;
    private MovementSerMessageHandler moveHandler;
    private MoveStep lastStep;

    public List<Vector3Int> Path
    { get { return path; } }

    public Vector3 Target
    { get { return target; } }

    public FixedMovement(NavAgent agent)
    {
        isMoving = false;

        path = new List<Vector3Int>();
        mapIns = Singleton.Instance<HexMap>();
        moveHandler = new MovementSerMessageHandler();
        targetAgent = agent;
    }

    public void Update()
    {
        if (isMoving)
        {
            targetAgent.transform.position = Vector3.MoveTowards(
                    current: targetAgent.transform.position,
                    target: target,
                    maxDistanceDelta: Time.deltaTime * speed);

            if ((targetAgent.transform.position - target).magnitude <= Constants.TINY_VALUE)
            {
                NextStep();
            }
        }
    }

    public void StartMove(JSONObject data)
    {
        moveSteps = moveHandler.HandlerEvent(data);
        path = moveHandler.GetPath(moveSteps);

        isMoving = true;
        NextStep();
    }

    private void NextStep()
    {
        if (moveSteps.Count > 0)
        {
            MoveStep step = moveSteps[0];
            target = mapIns.CellToWorld(step.NextPosition);
            float lastTime = lastStep == null ? 0 : lastStep.TimeSecond;
            speed = Vector3.Distance(target, targetAgent.transform.position) / (step.TimeSecond - lastTime);

            lastStep = step;
            moveSteps.RemoveAt(0);
            Path.RemoveAt(0);
        }
        else
            isMoving = false;

    }


}
