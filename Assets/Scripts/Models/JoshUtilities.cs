using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoshUtilities
{
    public static Vector3[] MakeWavyCircleProfile(float baseRadius, int density, float waveAmplitude, float waveLength)
    {
        Vector3[] profile = new Vector3[density];
        for (int i = 0; i < density; i++)
        {
            float angle = (2.0f * Mathf.PI * i) / density;
            float adjustedRadius = baseRadius + waveAmplitude * Mathf.Sin(waveLength * angle); // Adding a sine wave to the radius to make it "wavy"

            profile[i] = new Vector3(adjustedRadius * Mathf.Cos(angle), adjustedRadius * Mathf.Sin(angle), 0);
        }
        return profile;
    }

    public static Vector3[] MakeOvalProfile(float radiusX, float radiusY, int density)
    {
        Vector3[] profile = new Vector3[density];

        for (int i = 0; i < density; i++)
        {
            float angle = (2.0f * Mathf.PI * i) / density;
            float x = radiusX * Mathf.Cos(angle);
            float y = radiusY * Mathf.Sin(angle);
            profile[i] = new Vector3(x, y, 0);
        }

        return profile;
    }


    public static Matrix4x4[] DoublePathDensity(Matrix4x4[] originalPath)
    {
        // Assuming originalPath is filled with x elements
        Matrix4x4[] newPath = new Matrix4x4[originalPath.Length * 2 - 1];  // to hold double elements

        // Copy existing elements into new array
        for (int i = 0; i < originalPath.Length; i++)
        {
            newPath[2 * i] = originalPath[i];
        }

        // Calculate intermediate elements
        for (int i = 0; i < originalPath.Length - 1; i++)
        {
            // For scale
            Vector3 startScale = originalPath[i].lossyScale;
            Vector3 endScale = originalPath[i + 1].lossyScale;
            Vector3 middleScale = Vector3.Lerp(startScale, endScale, 0.5f);

            // For translation
            Vector3 startTrans = new Vector3(originalPath[i].m03, originalPath[i].m13, originalPath[i].m23);
            Vector3 endTrans = new Vector3(originalPath[i + 1].m03, originalPath[i + 1].m13, originalPath[i + 1].m23);
            Vector3 middleTrans = Vector3.Lerp(startTrans, endTrans, 0.5f);

            // Calculate the average rotation (slerping between two quaternions)
            Quaternion rotation1 = originalPath[i].rotation;
            Quaternion rotation2 = originalPath[i + 1].rotation;
            Quaternion avgRotation = Quaternion.Slerp(rotation1, rotation2, 0.5f);

            // Combine scale, translation, and rotation
            Matrix4x4 avgMatrix = Matrix4x4.Translate(middleTrans) * Matrix4x4.Rotate(avgRotation) * Matrix4x4.Scale(middleScale);

            // Place this new element into the array
            newPath[2 * i + 1] = avgMatrix;
        }
        return newPath;
    }

    public static Matrix4x4[] MakeSpherePath(int segments, float height, float startAt)
    {
        // Height is twice the radius of the sphere.
        float radius = height / 2.0f;

        // Validate startAt
        if (startAt < 0.0f || startAt >= 1.0f)
        {
            throw new ArgumentException("startAt must be between 0 (inclusive) and 1 (exclusive)");
        }

        // Calculate the effective number of segments based on the startAt parameter.
        int effectiveSegments = Mathf.CeilToInt((1 - startAt) * (segments + 1));

        // Allocate space for the transformation matrices.
        Matrix4x4[] spherePath = new Matrix4x4[effectiveSegments + 1];

        // Generate the transformation matrices for the sphere segments.
        for (int i = 0; i < effectiveSegments; i++)
        {
            // Calculate the angle from the vertical axis, considering the startAt parameter.
            float angle = Mathf.PI * (i + startAt * (segments + 1)) / (segments + 1);

            // Calculate the Z-coordinate along the sphere.
            float z = -radius * Mathf.Cos(angle);

            // Calculate the scaling factor for this segment's circle.
            float scale = radius * Mathf.Sin(angle);

            // Create and store the transformation matrix for this segment.
            spherePath[i] = Matrix4x4.Scale(new Vector3(scale, scale, 1)) * Matrix4x4.Translate(new Vector3(0, 0, z));
        }

        // Initialize top point (scale is zero so it's just a point).
        spherePath[effectiveSegments] = Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, 0, radius));

        return spherePath;
    }

    public static Matrix4x4[] MakeSpherePathClosed(int segments, float height, float startAt)
    {
        float radius = height / 2.0f;

        // Validate startAt
        if (startAt < 0.0f || startAt >= 1.0f)
        {
            throw new ArgumentException("startAt must be between 0 (inclusive) and 1 (exclusive)");
        }

        int effectiveSegments = Mathf.CeilToInt((1 - startAt) * (segments + 1));

        // Allocate one more space to add an extra face at the bottom.
        Matrix4x4[] spherePath = new Matrix4x4[effectiveSegments + 2];

        for (int i = 0; i < effectiveSegments; i++)
        {
            float angle = Mathf.PI * (i + startAt * (segments + 1)) / (segments + 1);
            float z = -radius * Mathf.Cos(angle);
            float scale = radius * Mathf.Sin(angle);
            spherePath[i + 1] = Matrix4x4.Scale(new Vector3(scale, scale, 1)) * Matrix4x4.Translate(new Vector3(0, 0, z));
        }

        // Duplicate the first "real" segment to fill the gap at the bottom.
        spherePath[0] = spherePath[1];
        spherePath[0] *= Matrix4x4.Scale(new Vector3(0, 0, 1));

        // Initialize top point (scale is zero so it's just a point).
        spherePath[effectiveSegments + 1] = Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, 0, radius));

        return spherePath;
    }

    public static Matrix4x4[] MakeStraightPath(float height)
    {
        Matrix4x4[] straightPath = new Matrix4x4[6];


        straightPath[0] = Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0));
        straightPath[1] = Matrix4x4.Scale(new Vector3(1, 1, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0));
        straightPath[2] = Matrix4x4.Scale(new Vector3(1, 1, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0));

        straightPath[3] = Matrix4x4.Scale(new Vector3(1, 1, 1)) * Matrix4x4.Translate(new Vector3(0, 0, height));
        straightPath[4] = Matrix4x4.Scale(new Vector3(1, 1, 1)) * Matrix4x4.Translate(new Vector3(0, 0, height));
        straightPath[5] = Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, 0, height));

        return straightPath;
    }


}
