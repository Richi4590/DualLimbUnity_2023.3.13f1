using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UpdateCollisionMesh : MonoBehaviour
{
    [SerializeField] private GameObject objectWithMesh;
    SkinnedMeshRenderer meshRenderer;
    MeshCollider meshCollider;

    private void Start()
    {
        meshRenderer = objectWithMesh.GetComponent<SkinnedMeshRenderer>();
        meshCollider = objectWithMesh.GetComponent<MeshCollider>();
    }

    private void FixedUpdate()
    {
        UpdateCollider();
    }

    public void UpdateCollider()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);

        // Get the scale of the parent object
        Vector3 parentScale = transform.lossyScale;

        int decimalPlaces = 4;
        parentScale = new Vector3((float)System.Math.Round(1 / parentScale.x, decimalPlaces), (float)System.Math.Round(1 / parentScale.y, decimalPlaces), (float)System.Math.Round(1 / parentScale.z, decimalPlaces));

        // Apply the parent's scale to the collider mesh
        Vector3[] vertices = colliderMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Vector3.Scale(vertices[i], parentScale);
        }
        colliderMesh.vertices = vertices;

        // Set the scaled mesh to the collider
        meshCollider.sharedMesh = colliderMesh;
    }
}
