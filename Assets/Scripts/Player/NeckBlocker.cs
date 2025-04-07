using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class NeckBlocker : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private Vector3 blockPosition = new Vector3(0, 0.15f, 0.1f);
    [SerializeField] private Vector3 blockScale = new Vector3(0.4f, 0.25f, 0.15f);
    [SerializeField] private Material blockerMaterial;

    void Start()
    {
        ConfigureBlocker();
    }

    private void ConfigureBlocker()
    {
        // Configura transformação
        transform.localPosition = blockPosition;
        transform.localScale = blockScale;
        transform.localRotation = Quaternion.identity;

        // Configura renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = blockerMaterial;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        // Remove componentes desnecessários
        Collider collider = GetComponent<Collider>();
        if (collider != null) Destroy(collider);
    }
}