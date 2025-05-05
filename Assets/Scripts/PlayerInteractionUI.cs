using UnityEngine;
using TMPro;

public class PlayerInteractionUI : MonoBehaviour
{
    public TextMeshProUGUI interactionText;

    public void MostrarTexto(string texto)
    {
        interactionText.text = texto;
        interactionText.enabled = true;
    }

    public void EsconderTexto()
    {
        interactionText.text = "";
        interactionText.enabled = false;
    }
}
