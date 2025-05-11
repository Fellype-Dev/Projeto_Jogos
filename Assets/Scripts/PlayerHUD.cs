using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image vidaBarra;
    public Image estaminaBarra;
    public Image slot1;
    public Image slot2;

    public void AtualizarVida(float valor)
    {
        vidaBarra.fillAmount = Mathf.Clamp01(valor);
    }

    public void AtualizarEstamina(float valor)
    {
        estaminaBarra.fillAmount = Mathf.Clamp01(valor);
    }

    public void AtualizarInventario(Sprite item1, Sprite item2)
    {
        slot1.sprite = item1;
        slot1.enabled = item1 != null;

        slot2.sprite = item2;
        slot2.enabled = item2 != null;
    }
}
