using UnityEngine;

public static class TagsLayersManager
{
    // Nomes das layers
    public const string LOCAL_PLAYER_HIDDEN_LAYER = "PlayerLocalHidden";
    public const string DEFAULT_LAYER = "Default";

    [RuntimeInitializeOnLoadMethod]
    private static void InitializeLayers()
    {
        // Verifica se a layer existe
        if (LayerMask.NameToLayer(LOCAL_PLAYER_HIDDEN_LAYER) == -1)
        {
            Debug.LogError($"Layer '{LOCAL_PLAYER_HIDDEN_LAYER}' não encontrada. Crie-a nas Project Settings > Tags and Layers");
        }
    }
}