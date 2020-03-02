using UnityEngine;

[ExecuteInEditMode]
public class OptimizeMesh : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] float _quality = 0.5f;
    MeshFilter _renderer;
    Mesh _mesh;
    void Start()
    {
        _renderer = GetComponent<MeshFilter>();
        _mesh = _renderer.sharedMesh;
    }

    public void DecimateMesh()
    {
        var meshSimplifier = new UnityMeshSimplifier.MeshSimplifier();
        meshSimplifier.Initialize(_mesh);
        meshSimplifier.SimplifyMesh(_quality);
        var destMesh = meshSimplifier.ToMesh();
        _renderer.sharedMesh = destMesh;
    }

    public void SaveMesh()
    {
        MeshSaverEditor.SaveMesh(_renderer.sharedMesh, "Optimized__" + gameObject.name, false, true);
    }
}
