using UnityEngine;
using System.Collections.Generic;

public class TerrianGenerationOld : MonoBehaviour {

	public static List<Vector2> Vectors;
	public Mesh terrian;
	public int tesselationLevel = 5;
	public float hillHight = 2f;


	public void Start()
	{
		List<Vector3> controllVectors = new List<Vector3>();
		float hightIncrement = hillHight / 9f;
		float currentPointY = 0;
		float currentXScale = transform.position.x - (transform.localScale.x / 2f);
		float scaleIncrement = transform.lossyScale.x / 9f;
		for (int i = 0; i < 9; i++)
		{
			float point1 = currentXScale;
			float point2 = currentPointY;
            float point3 = 0;
			controllVectors.Add(new Vector3(point1, point2, point3));

			if (i <= 5)
			{ 
				currentPointY += hightIncrement;
				currentXScale += scaleIncrement;
			}
			else
			{
				currentXScale -= scaleIncrement;
				currentPointY -= hightIncrement;
			}

		}

		hightIncrement = 9f / hillHight;
		//currentPointX = 0;
		currentPointY = 0;
		List<Vector2> controllUVs = new List<Vector2>();
		for (int i = 0; i < 9; i++)
		{
			float point1 = 0;
			float point2 = currentPointY;
			controllUVs.Add(new Vector2(point1, point2));

			//currentPointX++;
			if (i <= 4)
				currentPointY += hightIncrement;
			else
				currentPointY -= hightIncrement;
		}

		CreateBezierMesh(tesselationLevel, controllVectors, controllUVs);
		GetComponent<MeshFilter>().mesh = terrian;
		
	}

	Vector2 BezCurveUV(float tessellation, Vector2 point1, Vector2 point2, Vector2 point3)
	{
		Vector2 bezPoint = new Vector2();

		float a = 1f - tessellation;
		float tt = tessellation * tessellation;

		float[] tPoints = new float[2];
		for (int i = 0; i < 2; i++)
		{
			tPoints[i] = ((a * a) * point1[i]) + (2 * a) * (tessellation * point2[i]) + (tt * point3[i]);
		}
		bezPoint.Set(tPoints[0], tPoints[1]);

		return bezPoint;
    }

	Vector3 BezCurve(float tessellation, Vector3 point1, Vector3 point2, Vector3 point3)
	{
		Vector3 bezPoint = new Vector3();

		float a = 1f - tessellation;
		float tt = tessellation * tessellation;

		float[] tPoints = new float[3];
		for (int i = 0; i < 3; i++)
		{
			tPoints[i] = ((a * a) * point1[i]) + (2 * a) * (tessellation * point2[i]) + (tt * point3[i]);
		}
		bezPoint.Set(tPoints[0], tPoints[1], tPoints[2]);

		return bezPoint;
	}

	List<Vector3> Tessellate(int level, Vector3 point1, Vector3 point2, Vector3 point3)
	{
		List<Vector3> vectors = new List<Vector3>();

		float stepDelta = 1f/level;
		float step = stepDelta;

		vectors.Add(point1);
		for(int i = 0; i < (level - 1); i++)
		{
			vectors.Add(BezCurve(step, point1, point2, point3));
			step += stepDelta;
		}

		vectors.Add(point3);
		return vectors;
	}

	List<Vector2> TessellateUV(int level, Vector3 point1, Vector3 point2, Vector3 point3)
	{
		List<Vector2> vectors = new List<Vector2>();

		float stepDelta = 1f / level;
		float step = stepDelta;

		vectors.Add(point1);
		for (int i = 0; i < (level - 1); i++)
		{
			vectors.Add(BezCurveUV(step, point1, point2, point3));
			step += stepDelta;
		}

		vectors.Add(point3);
		return vectors;
	}

	public void CreateBezierMesh (int level, List<Vector3> controll, List<Vector2> controllUVs)
	{
		Mesh patchMesh = new Mesh();
		patchMesh.name = "BeziersMesh";

		List<Vector3> vertex = new List<Vector3>();
		List<int> index = new List<int>();
		List<Vector2> uvs = new List<Vector2>();


		List<Vector2> point1SUV;
		List<Vector3> point1s;
		point1s = Tessellate(level, controll[0], controll[3], controll[6]);
		point1SUV = TessellateUV(level, controllUVs[0], controllUVs[3], controllUVs[6]);

		List<Vector2> point2SUV;
		List<Vector3> point2s;
		point2s = Tessellate(level, controll[1], controll[4], controll[7]);
		point2SUV = TessellateUV(level, controllUVs[1], controllUVs[4], controllUVs[7]);

		List<Vector2> point3SUV;
		List<Vector3> point3s;
		point3s = Tessellate(level, controll[2], controll[5], controll[8]);
		point3SUV = TessellateUV(level, controllUVs[2], controllUVs[5], controllUVs[8]);

		for (int i = 0; i <= level; i++)
		{
			vertex.AddRange(Tessellate(level, point1s[i], point2s[i], point3s[i]));
			uvs.AddRange(TessellateUV(level, point1SUV[i], point2SUV[i], point3SUV[i]));
		}

		int numVerts = (level + 1) * (level + 1);

		int xStep = 1;
		int width = level + 1;
		for (int i = 0; i <numVerts- width; i++)
		{
			if (xStep == 1)
			{
				index.Add(i);
				index.Add(i + width);
				index.Add(i + 1);

				xStep++;
				continue;
			}
			else if (xStep == width)
			{
				index.Add(i);
				index.Add(i + (width - 1));
				index.Add(i + width);

				xStep = 1;
				continue;
			}
			else
			{
				index.Add(i);
				index.Add(i + (width - 1));
				index.Add(i + width);

				index.Add(i);
				index.Add(i + width);
				index.Add(i + 1);

				xStep++;
				continue;
			}
		}

		patchMesh.vertices = vertex.ToArray();
		patchMesh.triangles = index.ToArray();
		patchMesh.RecalculateBounds();
		patchMesh.RecalculateNormals();
		patchMesh.Optimize();

		terrian = patchMesh;
	}

}
