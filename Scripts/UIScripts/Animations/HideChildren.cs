using System.Collections.Generic;
using UnityEngine;

namespace UI.Animation
{
    public class HideChildren : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> children;
        [SerializeField]
        private GameObject target;

        [ContextMenu("Get Children")]
        public void GetChildren()
        {
            if (children == null) children = new List<GameObject>();
            else children.Clear();

            int childCount = target.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                children.Add(target.transform.GetChild(i).gameObject);
            }
        }

        [ContextMenu("Hidden")]
        public void Hidden()
        {
            int count = (int)children?.Count;
            for (int i = 0; i < count; i++)
            {
                children[i].SetActive(false);
            }
        }

        [ContextMenu("Show")]
        public void Shown()
        {
            int count = (int)children?.Count;
            for (int i = 0; i < count; i++)
            {
                children[i].SetActive(true);
            }
        }
    }
}