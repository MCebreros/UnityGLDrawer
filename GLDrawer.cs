using UnityEngine;

public static class GLDrawer
{
	private static int CircleDivisions = 30;

	private static readonly Vector3[] wireSphereVertexes;
	private static readonly Vector3[] cubeVertexes;
	private static readonly int c1;
	private static readonly int c2;
	
	private static Material lineMaterial;
	private static void CreateLineMaterial()
	{
		if (!lineMaterial)
		{
			// Unity has a built-in shader that is useful for drawing
			// simple colored things.
			Shader shader = Shader.Find("Hidden/Internal-Colored");
			lineMaterial = new Material(shader);
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			// Turn on alpha blending
			lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			// Turn backface culling off
			lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			// Turn off depth writes
			lineMaterial.SetInt("_ZWrite", 1);
		}
	}

	static GLDrawer()
	{
		//Calculating Wire Sphere Vertexes
		wireSphereVertexes = new Vector3[(CircleDivisions + 1) * 3];
		int deg = 360 / CircleDivisions;
		for (int i = 0; i < 360; i += 2 * deg)
		{
			float a1 = Mathf.Cos(Mathf.Deg2Rad * i);
			float o1 = Mathf.Sin(Mathf.Deg2Rad * i);

			float a2 = Mathf.Cos(Mathf.Deg2Rad * (i + deg));
			float o2 = Mathf.Sin(Mathf.Deg2Rad * (i + deg));

			//XZ
			wireSphereVertexes[i / deg] = new Vector3(a1, 0, o1);
			wireSphereVertexes[i / deg + 1] = new Vector3(a2, 0, o2);

			//XY
			wireSphereVertexes[(i / deg) + (CircleDivisions + 1)] = new Vector3(a1, o1, 0);
			wireSphereVertexes[(i / deg + 1) + (CircleDivisions + 1)] = new Vector3(a2, o2, 0);

			//ZY
			wireSphereVertexes[(i / deg) + (CircleDivisions + 1) * 2] = new Vector3(0, o1, a1);
			wireSphereVertexes[(i / deg + 1) + (CircleDivisions + 1) * 2] = new Vector3(0, o2, a2);
		}

		wireSphereVertexes[wireSphereVertexes.Length / 3 - 1] = wireSphereVertexes[0];
		wireSphereVertexes[wireSphereVertexes.Length * 2 / 3 - 1] = wireSphereVertexes[CircleDivisions + 1];
		wireSphereVertexes[wireSphereVertexes.Length - 1] = wireSphereVertexes[(CircleDivisions + 1) * 2];
		
		c1 = wireSphereVertexes.Length / 3;
		c2 = wireSphereVertexes.Length * 2 / 3;
		
		//Calculate Cube Vertexes
		cubeVertexes = new Vector3[24]; //4 vertexes per face
		
		//X aligned face
		cubeVertexes[0] = new Vector3(-0.5f,-0.5f,-0.5f);
		cubeVertexes[1] = new Vector3(-0.5f,-0.5f,0.5f);
		cubeVertexes[2] = new Vector3(-0.5f,0.5f,0.5f);
		cubeVertexes[3] = new Vector3(-0.5f,0.5f,-0.5f);
		
		//Y aligned face
		cubeVertexes[4] = new Vector3(-0.5f,-0.5f,-0.5f);
		cubeVertexes[5] = new Vector3(-0.5f,-0.5f,0.5f);
		cubeVertexes[6] = new Vector3(0.5f,-0.5f,0.5f);
		cubeVertexes[7] = new Vector3(0.5f,-0.5f,-0.5f);
		
		//Z aligned face
		cubeVertexes[8] = new Vector3(-0.5f,-0.5f,-0.5f);
		cubeVertexes[9] = new Vector3(-0.5f,0.5f,-0.5f);
		cubeVertexes[10] = new Vector3(0.5f,0.5f,-0.5f);
		cubeVertexes[11] = new Vector3(0.5f,-0.5f,-0.5f);
		
		//Duplicate faces
		Vector3[] transformationVectors = new[] {Vector3.right, Vector3.up, Vector3.forward};
		for (int i = 12; i < 24; i++)
		{
			int mirrorIndex = i - 12;
			cubeVertexes[i] = cubeVertexes[mirrorIndex] + transformationVectors[mirrorIndex/4];
		}
	}

	public static void WireSphere(Vector3 center, float radius, Color color)
	{
		CreateLineMaterial();
		lineMaterial.SetPass(0);
		
		//Drawing
		GL.PushMatrix();
		Matrix4x4 m = Matrix4x4.identity;
		m.SetTRS(center,Quaternion.identity,Vector3.one*radius);

		GL.MultMatrix(m);
		GL.Begin(GL.LINE_STRIP);
		GL.Color(color);
		for (int i = 0; i < c1; i++)
			GL.Vertex(wireSphereVertexes[i]);
		for (int i = c1; i < c2; i++)
			GL.Vertex(wireSphereVertexes[i]);
		GL.End();
		GL.Begin(GL.LINE_STRIP);
		GL.Color(color);
		for (int i = c2; i < wireSphereVertexes.Length; i++)
			GL.Vertex(wireSphereVertexes[i]);
		GL.End();
		GL.PopMatrix();
	}
	
	public static void WireSphere(Vector3 center, float radius, Color color1, Color color2, Color color3)
	{
		CreateLineMaterial();
		lineMaterial.SetPass(0);
		
		//Drawing
		GL.PushMatrix();
		Matrix4x4 m = Matrix4x4.identity;
		m.SetTRS(center,Quaternion.identity,Vector3.one*radius);

		GL.MultMatrix(m);
		GL.Begin(GL.LINE_STRIP);
		GL.Color(color1);
		for (int i = 0; i < c1; i++)
			GL.Vertex(wireSphereVertexes[i]);
		GL.Color(color2);
		for (int i = c1; i < c2; i++)
			GL.Vertex(wireSphereVertexes[i]);
		GL.End();
		GL.Begin(GL.LINE_STRIP);
		GL.Color(color3);
		for (int i = c2; i < wireSphereVertexes.Length; i++)
			GL.Vertex(wireSphereVertexes[i]);
		GL.End();
		GL.PopMatrix();
	}

	public static void Cube(Vector3 center, Vector3 size, Color color)
	{
		CreateLineMaterial();
		lineMaterial.SetPass(0);
		
		GL.PushMatrix();
		Matrix4x4 m =Matrix4x4.identity;
		m.SetTRS(center,Quaternion.identity,size);
		
		GL.MultMatrix(m);
		GL.Begin(GL.QUADS);
		GL.Color(color);
		for(int i = 0; i<cubeVertexes.Length; i++)
			GL.Vertex(cubeVertexes[i]);
		GL.End();
		GL.PopMatrix();
	}
	
	public static void Line(Vector3 start, Vector3 end, Color color)
	{
		CreateLineMaterial();
		lineMaterial.SetPass(0);
		
		GL.PushMatrix();
		GL.Begin(GL.LINE_STRIP);
		GL.Color(color);
		GL.Vertex(start);
		GL.Vertex(end);
		GL.End();
		GL.PopMatrix();
	}
	
	public static void AA3DGrid(Vector3 pivot, Vector3Int dimensions, Vector3 cellExtents, Color color)
	{
		
		int xDim = dimensions.x + 1;
		int yDim = dimensions.y + 1;
		int zDim = dimensions.z + 1;
		
		//Horizontals
		float hLen = dimensions.x * cellExtents.x;
		for (int y = 0; y < yDim; y++)
		{
			for (int z = 0; z < zDim; z++)
			{
				Vector3 start = pivot + new Vector3(0, y * cellExtents.y, z * cellExtents.z);
				Vector3 end = start + new Vector3(hLen, 0, 0);
				Line(start,end,color);
			}
		}
		
		//Verticals
		float vLen = dimensions.y * cellExtents.y;
		for (int x = 0; x < xDim; x++)
		{
			for (int z = 0; z < zDim; z++)
			{
				Vector3 start = pivot + new Vector3(x * cellExtents.x, 0, z * cellExtents.z);
				Vector3 end = start + new Vector3(0, vLen, 0);
				Line(start,end,color);
			}
		}
		
		//Transversals
		float tLen = dimensions.z * cellExtents.z;
		for (int x = 0; x < xDim; x++)
		{
			for (int y = 0; y < yDim; y++)
			{
				Vector3 start = pivot + new Vector3(x * cellExtents.x, y * cellExtents.y, 0);
				Vector3 end = start + new Vector3(0, 0, tLen);
				Line(start,end,color);
			}
		}
	}
}