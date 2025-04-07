using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject handModel;       // Objeto 3D da mão
    [SerializeField] private Animator handAnimator;      // Componente Animator
    [SerializeField] private string animationTrigger = "Wave"; // Nome do trigger

    [Header("Configurações")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Y;
    [SerializeField] private float animationCooldown = 2f;

    private bool isHandVisible = false;
    private float lastAnimationTime;

    void Start()
    {

        Debug.Log("HandController iniciado"); // Confirma que o script está ativo

        // Garante que a mão comece invisível
        SetHandVisibility(false);
        
        // Verifica componentes essenciais
        if (handAnimator == null && handModel != null)
            handAnimator = handModel.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleHand();
        }
    }

    public void ToggleHand()
    {
        isHandVisible = !isHandVisible;
        SetHandVisibility(isHandVisible);

        if (isHandVisible)
        {
            PlayHandAnimation();
        }
    }

    private void SetHandVisibility(bool visible)
    {
        if (handModel != null)
        {
            // Ativa/desativa o objeto completamente
            handModel.SetActive(visible);
            
            // Alternativa: controle por layer (se preferir)
            // handModel.layer = visible ? 
            //     LayerMask.NameToLayer("Default") : 
            //     LayerMask.NameToLayer("PlayerLocalHidden");
        }
    }

    private void PlayHandAnimation()
    {
        if (handAnimator != null && Time.time > lastAnimationTime + animationCooldown)
        {
            handAnimator.SetTrigger(animationTrigger);
            lastAnimationTime = Time.time;
        }
    }
}