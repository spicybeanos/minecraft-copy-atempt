using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{

    public Material MeshMaterial;
    public new MeshCollider collider;
    public void CombineMeshes()
    {
        MeshFilter[] meshFilter = GetComponentsInChildren<MeshFilter>();

        CombineInstance[] combine = new CombineInstance[meshFilter.Length];

        for(int i = 0; i < meshFilter.Length; i++)
        {
            combine[i].subMeshIndex = 0;
            combine[i].mesh = meshFilter[i].sharedMesh;
            combine[i].transform = meshFilter[i].transform.localToWorldMatrix;
        }

        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(combine);
        GetComponent<MeshFilter>().sharedMesh = finalMesh;
        GetComponent<Renderer>().material = MeshMaterial;
        collider.sharedMesh = finalMesh;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
