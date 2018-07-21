using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript: MonoBehaviour
{
    public A thisA;
}

[SerializeField]
public class A {
    public int mong;
    public Dictionary<string, B> listB;
}
[SerializeField]
public class B 
{
    public int vi;
}
