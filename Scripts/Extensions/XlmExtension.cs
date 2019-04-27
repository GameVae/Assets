using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Extensions.Xml
{
    public static class XlmExtension
    {
        public static XmlNode GetChildNode(this XmlNode node, string tag)
        {
            XmlNodeList children = node.ChildNodes;
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].LocalName.Equals(tag))
                    return children[i];
            }
            return null;
        }

        public static XmlNode[] GetChildrenNode(this XmlNode node, string tag)
        {
            XmlNodeList children = node.ChildNodes;
            List<XmlNode> nodes = new List<XmlNode>();

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].LocalName.Equals(tag))
                    nodes.Add(children[i]);
            }
            return nodes.ToArray();
        }

        public static XmlAttribute GetAttribute(this XmlNode node, string attr)
        {
            XmlAttributeCollection attrs = node.Attributes;
            if (attr != null)
            {
                for (int i = 0; i < attrs.Count; i++)
                {
                    if (attrs[i].Name.Equals(attr))
                    {
                        return attrs[i];
                    }
                }
            }
            return null;
        }

        public static XmlNode FirstChildByAttribute(this XmlNode node, string attrName, string attrValue)
        {
            XmlNodeList children = node.ChildNodes;
            for (int i = 0; i < children.Count; i++)
            {
                XmlAttribute attr = children[i].GetAttribute(attrName);
                if (attr != null)
                {
                    if(attr.Value.Equals(attrValue))
                    {
                        return children[i];
                    }
                }
            }
            return null;
        }
    }
}