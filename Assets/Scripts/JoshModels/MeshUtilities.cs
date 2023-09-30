using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUtilities
{
	public static Mesh Cube(float size)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4 * 6]
        {
            //front
		    new Vector3(-size,-size,-size),
            new Vector3(size, -size, -size),
            new Vector3(size, size, -size),
            new Vector3(-size, size, -size),
            
            // back
            new Vector3(-size, -size, size),
            new Vector3(size, -size, size),
            new Vector3(size, size, size),
            new Vector3(-size, size, size),
            
            // left
            new Vector3(-size, -size, -size),
            new Vector3(-size, size, -size),
            new Vector3(-size, size, size),
            new Vector3(-size, -size, size),
            
            // right
            new Vector3(size, -size, -size),
            new Vector3(size, size, -size),
            new Vector3(size, size, size),
            new Vector3(size, -size, size),
            
            // bottom
            new Vector3(-size, -size, -size),
            new Vector3(-size, -size, size),
            new Vector3(size, -size, size),
            new Vector3(size, -size, -size),
            
            // top
            new Vector3(-size, size, -size),
            new Vector3(-size, size, size),
            new Vector3(size, size, size),
            new Vector3(size, size, -size)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6 * 2 * 3]
        {
            //front
            3, 2, 1,
            3, 1, 0,

            // back
            4,5,6,
            4,6,7,

            // left
            11,10,9,
            11,9,8,

            // right
            12,13,14,
            12,14,15,

            // bottom
            19,18,17,
            19,17,16,

            // top
            20,21,22,
            20,22,23
        };
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        return mesh;
    }

    public static Mesh Pyramid(float size)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[16]
        {
        // Side 1 (base vertices and a point vertex)
        new Vector3(-size, -size, -size), // 0
        new Vector3(size, -size, -size),  // 1
        new Vector3(0, size, 0),          // 2

        // Side 2
        new Vector3(size, -size, -size),  // 3
        new Vector3(size, -size, size),   // 4
        new Vector3(0, size, 0),          // 5

        // Side 3
        new Vector3(size, -size, size),   // 6
        new Vector3(-size, -size, size),  // 7
        new Vector3(0, size, 0),          // 8

        // Side 4
        new Vector3(-size, -size, size),  // 9
        new Vector3(-size, -size, -size), // 10
        new Vector3(0, size, 0),          // 11

        // Base
        new Vector3(-size, -size, -size), // 12
        new Vector3(size, -size, -size),  // 13
        new Vector3(size, -size, size),   // 14
        new Vector3(-size, -size, size)   // 15
        };
        mesh.vertices = vertices;

        int[] triangles = new int[18]
        {
        // Sides (triangles)
        0, 2, 1,
        3, 5, 4,
        6, 8, 7,
        9, 11, 10,

        // Base (square)
        12, 13, 14,
        12, 14, 15
        };
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }


    public static Mesh Cylinder(int d, float r, float h)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4 * d + 2]; // added extra
        float dTheta = Mathf.PI * 2.0f / d;
        for (int i = 0; i < d; i++)
        {
            float theta = i * dTheta;
            float x = r * Mathf.Cos(theta);
            float z = r * Mathf.Sin(theta);
            // top vertex
            vertices[i] = new Vector4(x, h, z);
            // bottom vertex
            vertices[i + d] = new Vector4(x, -h, z);

            // vertices for caps
            vertices[i + d * 2] = new Vector4(x, h, z); // top cap
            vertices[i + d * 3] = new Vector4(x, -h, z); // bottom cap

        }

        // center vertices for caps
        vertices[d * 4] = new Vector3(0, h, 0); // top center
        vertices[d * 4 + 1] = new Vector3(0, -h, 0); // bottom center

        mesh.vertices = vertices;

        int[] tris = new int[d * 12]; // two tris for each side
        for (int i = 0; i < d; i++)
        {
            tris[i * 6] = i;                        // current top vertex
            tris[i * 6 + 1] = (i + 1) % d;          // next top vertex (wrapping)
            tris[i * 6 + 2] = d + (i + 1) % d;      // next bottom vertex (wrapping)

            tris[i * 6 + 3] = i;                    // current top vertex
            tris[i * 6 + 4] = d + (i + 1) % d;      // next bottom vertex (wrapping)
            tris[i * 6 + 5] = d + i;                // current bottom vertex
        }

        // add triangles for caps
        for (int i = 0; i < d; i++)
        {
            // top cap
            tris[d * 6 + i * 3] = d * 2 + i;
            tris[d * 6 + i * 3 + 1] = d * 4; // top center
            tris[d * 6 + i * 3 + 2] = d * 2 + (i + 1) % d;

            // bottom cap
            tris[d * 9 + i * 3] = d * 3 + i;
            tris[d * 9 + i * 3 + 1] = d * 3 + (i + 1) % d;
            tris[d * 9 + i * 3 + 2] = d * 4 + 1; // bottom center
        }

        mesh.triangles = tris;

        mesh.RecalculateNormals();

        return mesh;
    }


    public static Mesh Sweep(Vector3[] profile, Matrix4x4[] path, bool closed)
    {
		Mesh mesh = new Mesh();

		int numVerts = path.Length * profile.Length;
		int numTris;

		if (closed)
			numTris = 2 * path.Length * profile.Length;
		else
			numTris = 2 * (path.Length-1) * profile.Length;


		Vector3[] vertices = new Vector3[numVerts];
		int[]tris = new int[numTris * 3];

		for (int i = 0; i < path.Length; i++)
		{
			for (int j = 0; j < profile.Length; j++)
			{
				Vector3 v = path[i].MultiplyPoint(profile[j]);
				vertices[i*profile.Length+j] = v;

				if (closed || i < path.Length - 1)
				{

					tris[6 * (i * profile.Length + j)] = (j + i * profile.Length);
					tris[6 * (i * profile.Length + j) + 1] = ((j + 1) % profile.Length + i * profile.Length);
					tris[6 * (i * profile.Length + j) + 2] = ((j + 1) % profile.Length + ((i + 1) % path.Length) * profile.Length);
					tris[6 * (i * profile.Length + j) + 3] = (j + i * profile.Length);
					tris[6 * (i * profile.Length + j) + 4] = ((j + 1) % profile.Length + ((i + 1) % path.Length) * profile.Length);
					tris[6 * (i * profile.Length + j) + 5] = (j + ((i + 1) % path.Length) * profile.Length);
				}
			}
		}

		mesh.vertices = vertices;

		mesh.triangles = tris;

		mesh.RecalculateNormals();

		return mesh;
	}

	public static Matrix4x4[] MakeCirclePath(float radius, int density)
	{
		Matrix4x4[] path = new Matrix4x4[density];
		for (int i = 0; i < density; i++)
		{
			float angle = (360.0f * i) / density;
			path[i] = Matrix4x4.Rotate(Quaternion.Euler(0, -angle, 0))* Matrix4x4.Translate(new Vector3(radius,0,0));
		}
		return path;
	}

	public static Vector3[] MakeCircleProfile(float radius, int density)
	{
		Vector3[] profile = new Vector3[density];
		for (int i = 0; i < density; i++)
		{
			float angle = (2.0f * Mathf.PI * i) / density;
			profile[i] = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle),0);
		}
		return profile;
	}

    public static Mesh Pencil(float length, float radius, float taper, int sides, int tipDensity)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // Add center vertex for the base
        vertices.Add(new Vector3(0, 0, 0));

        // Add base vertices for flat shading
        for (int i = 0; i < sides; i++)
        {
            float angle = i * 2 * Mathf.PI / sides;
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            vertices.Add(new Vector3(x, y, 0));
        }

        // Generate base triangles
        for (int i = 1; i <= sides; i++)
        {
            int nextIdx = (i % sides) + 1;
            triangles.Add(0);
            triangles.Add(nextIdx);
            triangles.Add(i);
        }

        // Generate body vertices
        for (int i = 0; i < sides; i++)
        {
            float angle = i * 2 * Mathf.PI / sides;
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            // Triple duplicate vertices for flat shading
            vertices.Add(new Vector3(x, y, 0));
            vertices.Add(new Vector3(x, y, 0));
            vertices.Add(new Vector3(x, y, 0));
            vertices.Add(new Vector3(x, y, length - taper));
            vertices.Add(new Vector3(x, y, length - taper));
            vertices.Add(new Vector3(x, y, length - taper));
        }

        // Generate body triangles
        int bodyStart = sides + 1; // Account for base vertices and the central vertex
        for (int i = 0; i < sides; i++)
        {
            int startIdx = bodyStart + i * 6; // 6 vertices for each side
            int nextIdx = bodyStart + ((i + 1) % sides) * 6;

            // First triangle
            triangles.Add(startIdx + 2);
            triangles.Add(nextIdx);
            triangles.Add(startIdx + 4);

            // Second triangle
            triangles.Add(startIdx + 5);
            triangles.Add(nextIdx + 1);
            triangles.Add(nextIdx + 3);
        }

        // Add tip vertex
        vertices.Add(new Vector3(0, 0, length));

        // Generate tip vertices
        for (int i = 0; i < tipDensity; i++)
        {
            float angle = i * 2 * Mathf.PI / tipDensity;
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            vertices.Add(new Vector3(x, y, length - taper));
        }

        // Generate tip triangles
        int tipStart = bodyStart + sides * 6;  // Base vertices, central vertex, and body vertices
        for (int i = 1; i <= tipDensity; i++)
        {
            int nextIdx = (i % tipDensity) + 1;
            triangles.Add(tipStart);
            triangles.Add(tipStart + i);
            triangles.Add(tipStart + nextIdx);
        }

        //// Bridging vertices
        //for (int i = 0; i < sides; i++)
        //{
        //    float angle = i * 2 * Mathf.PI / sides;
        //    float x = radius * Mathf.Cos(angle);
        //    float y = radius * Mathf.Sin(angle);

        //    float theta = 2 * Mathf.PI / sides; // The angle between vertices
        //    float phi = theta / 2; // The angle from the center to the midpoint between vertices
        //    float d = radius * Mathf.Cos(phi) / Mathf.Cos(theta / 2); // distance to move along z-axis

        //    // Calculate new x, y using reduced distance 'd'
        //    float newX = d * Mathf.Cos(angle);
        //    float newY = d * Mathf.Sin(angle);

        //    vertices.Add(new Vector3(newX, newY, length - taper - d));
        //}



        //// Bridging Triangles
        //int bridgeStart = tipStart + tipDensity; // New vertices have been added to the end
        //int tipEndBodyStart = bodyStart;

        //for (int i = 0; i < sides; i++)
        //{
        //    int bridgeIdx = bridgeStart + i;
        //    int bodyIdx = tipEndBodyStart + i * 6 + 5; // Top vertices of the body
        //    int nextBridgeIdx = bridgeStart + (i + 1) % sides;
        //    int nextBodyIdx = tipEndBodyStart + ((i + 1) % sides) * 6 + 5;

        //    // Connect bridging vertex to body
        //    triangles.Add(bridgeIdx);
        //    triangles.Add(bodyIdx);
        //    triangles.Add(nextBodyIdx);
        //}

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }


}