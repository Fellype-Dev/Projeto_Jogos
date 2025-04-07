using UnityEngine;

public class PlayerVisibilityController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform headgear;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform weaponParent;
    [SerializeField] private Transform additionalObject1;
    [SerializeField] private Transform additionalObject2;
    [SerializeField] private Transform playerHand; // Novo: Referência para a mão

    [Header("Configurações Gerais")]
    [SerializeField] private bool keepShadows = true;
    [SerializeField] private bool isLocalPlayer = true;

    [Header("Visibilidade por Objeto")]
    [SerializeField] private bool hideHeadgear = true;
    [SerializeField] private bool hideBody = false;
    [SerializeField] private bool hideWeaponParent = false;
    [SerializeField] private bool hideAdditional1 = false;
    [SerializeField] private bool hideAdditional2 = false;
    [SerializeField] private bool startWithHandHidden = true; // Novo: Controle inicial da mão

    [Header("Configurações da Mão")]
    [SerializeField] private KeyCode handToggleKey = KeyCode.Y;
    [SerializeField] private string handAnimationTrigger = "Wave";
    [SerializeField] private float handAnimationCooldown = 2f;

    // Variáveis de controle da mão
    private bool isHandVisible = false;
    private float lastHandAnimationTime;
    private Animator handAnimator;

    private void Start()
    {
        // Configura a mão
        if (playerHand != null)
        {
            handAnimator = playerHand.GetComponent<Animator>();
            SetHandVisibility(startWithHandHidden);
        }

        UpdateVisibility();
    }

    private void Update()
    {
        // Controle da mão apenas para jogador local
        if (isLocalPlayer && playerHand != null && Input.GetKeyDown(handToggleKey))
        {
            ToggleHand();
        }
    }

    public void SetAsLocalPlayer(bool isLocal)
    {
        isLocalPlayer = isLocal;
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (isLocalPlayer)
        {
            ConfigureLocalPlayer();
        }
        else
        {
            ConfigureOtherPlayer();
        }
    }

    #region Configurações de Visibilidade
    private void ConfigureLocalPlayer()
    {
        SetObjectVisibility(headgear, hideHeadgear);
        SetObjectVisibility(playerBody, hideBody);
        SetObjectVisibility(weaponParent, hideWeaponParent);
        SetObjectVisibility(additionalObject1, hideAdditional1);
        SetObjectVisibility(additionalObject2, hideAdditional2);

        if (keepShadows) ConfigureShadows();
    }

    private void ConfigureOtherPlayer()
    {
        SetObjectVisibility(headgear, false);
        SetObjectVisibility(playerBody, false);
        SetObjectVisibility(weaponParent, false);
        SetObjectVisibility(additionalObject1, false);
        SetObjectVisibility(additionalObject2, false);
    }
    #endregion

    #region Controle da Mão
    public void ToggleHand()
    {
        isHandVisible = !isHandVisible;
        SetHandVisibility(isHandVisible);

        if (isHandVisible && handAnimator != null && 
            Time.time > lastHandAnimationTime + handAnimationCooldown)
        {
            handAnimator.SetTrigger(handAnimationTrigger);
            lastHandAnimationTime = Time.time;
        }
    }

    private void SetHandVisibility(bool visible)
    {
        if (playerHand != null)
        {
            // Usa o mesmo sistema de layers para consistência
            string layer = visible ? TagsLayersManager.DEFAULT_LAYER : 
                                  TagsLayersManager.LOCAL_PLAYER_HIDDEN_LAYER;
            SetObjectLayer(playerHand, layer);
        }
    }
    #endregion

    #region Métodos Auxiliares
    private void SetObjectVisibility(Transform obj, bool shouldHide)
    {
        if (obj == null) return;
        
        string layerName = shouldHide ? TagsLayersManager.LOCAL_PLAYER_HIDDEN_LAYER : 
                                      TagsLayersManager.DEFAULT_LAYER;
        SetObjectLayer(obj, layerName);
    }

    private void SetObjectLayer(Transform obj, string layerName)
    {
        if (obj == null) return;
        
        obj.gameObject.layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in obj)
        {
            SetObjectLayer(child, layerName);
        }
    }

    private void ConfigureShadows()
    {
        SetShadowCasting(headgear, hideHeadgear);
        SetShadowCasting(playerBody, hideBody);
        SetShadowCasting(weaponParent, hideWeaponParent);
        SetShadowCasting(additionalObject1, hideAdditional1);
        SetShadowCasting(additionalObject2, hideAdditional2);
    }

    private void SetShadowCasting(Transform obj, bool shouldCastShadowsOnly)
    {
        if (obj == null) return;
        
        var mode = shouldCastShadowsOnly ? 
            UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly : 
            UnityEngine.Rendering.ShadowCastingMode.On;
            
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.shadowCastingMode = mode;
        }
    }
    #endregion
}