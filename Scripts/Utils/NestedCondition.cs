using System.Collections.Generic;

public sealed class NestedCondition
{
    private List<System.Func<bool>> conditions;

    public NestedCondition()
    {
        conditions = new List<System.Func<bool>>();
    }

    public event System.Func<bool> Conditions
    {
        add
        {
            conditions.Add(value);
        }
        remove
        {
            int index = conditions.IndexOf(value);
            if (index >= 0) conditions.RemoveAt(index);
        }
    }

    public bool Evaluate()
    {
        if (conditions != null)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions[i].Invoke() == false) return false;
            }
        }
        return true;
    }
}
