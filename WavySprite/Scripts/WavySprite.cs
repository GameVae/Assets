using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)),RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class WavySprite:MonoBehaviour{

	[HideInInspector]
	public  MeshRenderer mr;
	private Material mat;
	private MeshFilter mf;
	private Mesh mesh;
	private List<Vector3> vertices=new List<Vector3>(200);
	private List<Vector3> uvs=new List<Vector3>(200);
	private List<Color> colors=new List<Color>(200);
	private int[] triangles;

	public Texture2D texture;
	private Texture2D _texture;

	public Color tint=Color.white;
	private Color _tint;

	[Range(0,30)]
	public int divisionsX=10;
	private int _divisionsX;

	[Range(0,30)]
	public int divisionsY=10;
	private int _divisionsY;

	public enum waveDirections{Vertical,Horizontal};
	public waveDirections waveDirection=waveDirections.Vertical;
	private waveDirections _waveDirection;

	public enum objSides{Top,Right,Bottom,Left,None}
	public objSides staticSide=objSides.Bottom;
	private objSides _staticSide;

	[Range(-10,10)]
	public float waveFrequency=10f;
	private float _waveFrequency;

	[Range(0f,1f)]
	public float waveForce=0.03f;
	private float _waveForce;

	[Range(0f,10f)]
	public float waveSpeed=1f;
	private float _waveSpeed;

	float meshWidth=1;
	float meshHeight=1;

	[HideInInspector]
	public int sortingLayer=0;
	private int _sortingLayer;

	[HideInInspector]
	public int orderInLayer=0;
	private int _orderInLayer=0;

	void OnEnable(){
		mr=GetComponent<MeshRenderer>();
		mf=GetComponent<MeshFilter>();
		SetMeshAndMaterial();
		GenerateMesh();
		sortingLayer=mr.sortingLayerID;
		orderInLayer=mr.sortingOrder;
        Update();
    }

	void Update(){
		if(
			_texture!=texture || 
			_tint!=tint ||
			_divisionsX!=divisionsX || 
			_divisionsY!=divisionsY || 
			_waveDirection!=waveDirection || 
			_staticSide!=staticSide || 
			_waveFrequency!=waveFrequency || 
			_waveForce!=waveForce ||
			_waveSpeed!=waveSpeed ||
			_sortingLayer!=sortingLayer ||
			_orderInLayer!=orderInLayer
		){
			SetMeshAndMaterial();
			_divisionsX=divisionsX;
			_divisionsY=divisionsY;
			if(_waveDirection!=waveDirection){
				mat.SetFloat("_WaveDirection",waveDirection==waveDirections.Vertical?0:1);
				_waveDirection=waveDirection;
			}
			if(_staticSide!=staticSide){
				if(staticSide==objSides.Top) mat.SetFloat("_StaticSide",1);
				if(staticSide==objSides.Right) mat.SetFloat("_StaticSide",2);
				if(staticSide==objSides.Bottom) mat.SetFloat("_StaticSide",3);
				if(staticSide==objSides.Left) mat.SetFloat("_StaticSide",4);
				if(staticSide==objSides.None) mat.SetFloat("_StaticSide",0);
				_staticSide=staticSide;
			}
			_waveFrequency=waveFrequency;
			_waveForce=waveForce;
			_waveSpeed=waveSpeed;
			mat.SetFloat("_WaveFrequency",waveFrequency);
			mat.SetFloat("_WaveForce",waveForce);
			mat.SetFloat("_WaveSpeed",waveSpeed);
			if(_texture!=texture){
				_texture=texture;
				mat.SetTexture("_MainTex",texture);
				if(texture!=null){
					if(texture.width>texture.height){
						meshWidth=1f;
						meshHeight=(float)texture.height/(float)texture.width;
					}else{
						meshWidth=(float)texture.width/(float)texture.height;
						meshHeight=1f;
					}
				}else{
					meshWidth=1f;
					meshHeight=1f;
				}
			}

			_tint=tint;
			mat.SetColor("_Color",tint);

			if(_sortingLayer!=sortingLayer || _orderInLayer!=orderInLayer){
				mr.sortingLayerID=sortingLayer;
				mr.sortingOrder=orderInLayer;
				_sortingLayer=sortingLayer;
				_orderInLayer=orderInLayer;
			}
			GenerateMesh();
		}
	}

	void SetMeshAndMaterial(){
		if(mesh==null){
			mesh=new Mesh();
			mesh.name="WavySpriteMesh";
			if(mf.sharedMesh!=null) DestroyImmediate(mf.sharedMesh);
		}
		if(mf.sharedMesh==null){
			mf.sharedMesh=mesh;
		}
		if(mat==null){
			mat=new Material(Shader.Find("Custom/WavySprite"));
			mat.name="WavySpriteMaterial";
			if(mr.sharedMaterial!=null) DestroyImmediate(mr.sharedMaterial);
		}
		if(mr.sharedMaterial==null){
			mr.sharedMaterial=mat;
		}
	}
	
	void GenerateMesh(){
		int pointsX=divisionsX+2;
		int pointsY=divisionsY+2;
		int verticeNum=0;
		int squareNum=-1;
		vertices.Clear();
		uvs.Clear();
		colors.Clear();
		triangles=new int[((pointsX*pointsY)*2)*3];
		for(int y=0;y<pointsY;y++){
			for(int x=0;x<pointsX;x++){
				vertices.Add(new Vector3(
					(((float)x/(float)(pointsX-1))-0.5f)*meshWidth,
					((float)y/(float)(pointsY-1))*meshHeight,
					0f
				));
				uvs.Add(new Vector3(
					((float)x/(float)(pointsX-1)),
					((float)y/(float)(pointsY-1)),
					0f
				));
				//Add triangles
				if(x>0 && y>0){
					verticeNum=x+(y*pointsX);
					squareNum++;
					triangles[squareNum*6]=verticeNum-pointsX-1;
					triangles[squareNum*6+1]=verticeNum-1;
					triangles[squareNum*6+2]=verticeNum;
					triangles[squareNum*6+3]=verticeNum;
					triangles[squareNum*6+4]=verticeNum-pointsX;
					triangles[squareNum*6+5]=verticeNum-pointsX-1;
				}
			}
		}
		mesh.Clear();
		mesh.SetVertices(vertices);
		mesh.SetUVs(0,uvs);
		mesh.SetColors(colors);
		mesh.SetTriangles(triangles,0);
	}
}
