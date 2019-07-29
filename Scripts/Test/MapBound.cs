using Generic.Pooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBound : MonoBehaviour
{
    public class QuadInfo
    {
        public const int Left = 0x0001;
        public const int Top =  0x0010;

        public readonly int Side;
        public GameObject Quad;
        public MeshFilter QuadMesh;

        public QuadInfo(GameObject quad,int side)
        {
            Side = side;
            Quad = quad;
            QuadMesh = quad.GetComponent<MeshFilter>();
            Debugger.Log(Size());
        }

        public bool IsOutside(Bird bird)
        {
            Vector3 position = bird.transform.position;

            return false;
        }

        public Vector2 Size()
        {
            return QuadMesh.mesh.bounds.size;
        }
    }

    private QuadInfo leftInfo;

    private Pooling<Bird> birdPool;

    private Pooling<Bird> BirdPool
    {
        get
        {
            return birdPool ?? (birdPool = new Pooling<Bird>(CreateBird));
        }
    }

    public Bird Prefab;

    public GameObject QuadLeft;

    public CameraController CameraController;
    
    public CameraBlindInsideMap CameraBlind
    {
        get
        {
            return CameraController.CameraBinding;
        }
    }

    public void Start()
    {
        CameraController.CameraChanged += CameraChanged;

        leftInfo = new QuadInfo(QuadLeft,QuadInfo.Left);
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            BirdPool.GetItem().LetStart(leftInfo);
        }
    }

    private void CameraChanged()
    {
        //Vector3[] conners = CameraBlind.Conners;
        Vector3 left = QuadLeft.transform.position;
        left.z = CameraBlind.Center.z;

        QuadLeft.transform.position = left;

    }

    private Bird CreateBird(int insId)
    {
        Bird bird = Instantiate(Prefab);

        bird.FirstSetup(insId);
        bird.SetPool(BirdPool);

        return bird;
    }
}
