using TMPro;
using UnityEngine;

public class PlayerInteractionUI : MonoBehaviour
{
    public TextMeshProUGUI interactionText;

    public void MostrarTexto(string texto)
    {
        if (interactionText != null)
        {
            interactionText.text = texto;
            interactionText.enabled = true;
        }
    }

    public void EsconderTexto()
    {
        if (interactionText != null)
        {
            interactionText.text = "";
            interactionText.enabled = false;
        }
    }
}