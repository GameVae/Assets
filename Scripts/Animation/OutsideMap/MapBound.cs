using Generic.Pooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBound : MonoBehaviour
{
    [Serializable]
    public class QuadInfo
    {
        public const int Left = 0x0001;
        public const int Top =  0x0010;

        public int Side;
        public GameObject Quad;
        public MeshFilter QuadMesh;
        public Transform EndOfLeft;
        public Transform EndOfRight;

        public void Init(int side)
        {
            Side = side;
            QuadMesh = Quad.GetComponent<MeshFilter>();
        }

        public bool IsOutside(Bird bird)
        {
            Vector3 birdPos = bird.transform.position;

            return birdPos.z < EndOfLeft.position.z || birdPos.z > EndOfRight.position.z;
        }

        public Vector2 Size()
        {
            return QuadMesh.mesh.bounds.size;
        }
    }

    [SerializeField] private QuadInfo leftInfo;

    private Pooling<Bird> birdPool;

    private Pooling<Bird> BirdPool
    {
        get
        {
            return birdPool ?? (birdPool = new Pooling<Bird>(CreateBird));
        }
    }

    public Bird Prefab;

    public CameraController CameraController;
    
    public CameraBlindInsideMap CameraBlind
    {
        get
        {
            return CameraController.CameraBinding;
        }
    }

    private void Start()
    {
        CameraController.CameraChanged += CameraChanged;

        leftInfo.Init(QuadInfo.Left);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            BirdPool.GetItem().LetStart(leftInfo);
        }
    }

    private void CameraChanged()
    {
        //Vector3[] conners = CameraBlind.Conners;
        Vector3 left = leftInfo.Quad.transform.position;
        left.z = CameraBlind.Center.z;

        leftInfo.Quad.transform.position = left;

    }

    private Bird CreateBird(int insId)
    {
        Bird bird = Instantiate(Prefab);

        bird.FirstSetup(insId);
        bird.SetPool(BirdPool);

        return bird;
    }
}
