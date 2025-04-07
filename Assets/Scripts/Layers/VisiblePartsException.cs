using UnityEngine;

public class VisiblePartsException : MonoBehaviour
{
    [SerializeField] private Transform[] alwaysVisibleParts;
    
    private void Start()
    {
        foreach (Transform part in alwaysVisibleParts)
        {
            if (part != null)
            {
                part.gameObject.layer = LayerMask.NameToLayer(TagsLayersManager.DEFAULT_LAYER);
                
                Renderer r = part.GetComponent<Renderer>();
                if (r != null)
                {
                    r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }
        }
    }
}