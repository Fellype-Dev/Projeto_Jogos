using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCameraSetup : MonoBehaviour
{
    private void Awake()
    {
        Camera playerCamera = GetComponent<Camera>();
        
        // Remove a layer LOCAL_PLAYER_HIDDEN do culling mask
        playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(TagsLayersManager.LOCAL_PLAYER_HIDDEN_LAYER));
        
        // Configurações recomendadas
        playerCamera.depth = 1;
        playerCamera.fieldOfView = 80f;
    }
}