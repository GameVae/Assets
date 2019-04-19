
namespace Map
{
    public abstract class SingleWayPoint : WayPoint
    {
        public override bool Binding()
        {
            return Manager.Add(this);
        }

        public override bool Unbinding()
        {
            //if(agentNodeManager.GetInfo(Position, out NodeInfo info))
            //{
            //    if(info == NodeInfo) // agent only remove its info otherwise return false (ignore)
            //    {
            //        return agentNodeManager.Remove(Position);

            //    }
            //}
            //return false;
            return Manager.Remove(this);
        }
    }
}
