using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance { get; private set; }

    public List<int> StaticObstacles { get; private set; }
    public List<Obstacle> DynamicObstacles { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance.gameObject);
        StaticObstacles = new List<int>();
        DynamicObstacles = new List<Obstacle>();
    }

    public void AddStaticObstacle(int index)
    {
        if (!StaticObstacles.Contains(index))
        {
            StaticObstacles.Add(index);
        }
    }
    public void RemoveStaticObstacle(int index)
    {
        int pos = StaticObstacles.IndexOf(index);
        if (pos >= 0) StaticObstacles.RemoveAt(pos);
    }

    public void AddDynamicObstacle(Obstacle obs)
    {
        if (!DynamicObstacles.Contains(obs)) DynamicObstacles.Add(obs);
    }
    public void RemoveDynamicObstacle(Obstacle obs)
    {
        int pos = DynamicObstacles.IndexOf(obs);
        if (pos >= 0) StaticObstacles.RemoveAt(pos);
    }

    public int[] GetDynamicObstacle(int yourIndex)
    {
        List<int> result = new List<int>();
        int temp = -1;
        for (int i = 0; i < DynamicObstacles.Count; i++)
        {
            temp = DynamicObstacles[i].GetIndex();
            if (temp != yourIndex)
            {
                result.Add(temp);
            }
        }
        return result.ToArray();
    }
    public int[] GetStaticObstacle()
    {
        return StaticObstacles.ToArray();
    }

    public bool Contain(int index)
    {
        for (int i = 0; i < StaticObstacles.Count; i++)
        {
            if (index == StaticObstacles[i]) return true;
        }

        int[] dynamicObs = GetDynamicObstacle(-1);
        for (int i = 0; i < dynamicObs.Length; i++)
        {
            if (dynamicObs[i] == index) return true;
        }
        return false;
    }
}
