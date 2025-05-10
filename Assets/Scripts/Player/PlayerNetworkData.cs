using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkData : NetworkBehaviour
{
    public string PlayerId { get; private set; }

    [SerializeField] private Camera playerCamera;

    public void Initialize(string playerId)
    {
        PlayerId = playerId;
        if (IsServer)
        {
            // Sincronize dados iniciais aqui se precisar
            Debug.Log("[PlayerNetworkData] Inicializando dados do jogador no servidor");
        }
    }

   public override void OnNetworkSpawn()
   {
        base.OnNetworkSpawn();

        Debug.Log($"[PlayerNetworkData] OnNetworkSpawn chamado no client: {NetworkManager.Singleton.LocalClientId}, dono: {OwnerClientId}, IsOwner: {IsOwner}");

        // Verifica se é o dono da instância (jogador local)
        if (!IsOwner)
        {
            Debug.Log("[PlayerNetworkData] Não é o dono, desativando câmera e AudioListener para este jogador.");
            var camera = GetComponentInChildren<Camera>();
            if (camera != null)
            {
                camera.enabled = false;
                Debug.Log("[PlayerNetworkData] Câmera desativada para jogador não-dono.");
            }

            var audioListener = GetComponentInChildren<AudioListener>();
            if (audioListener != null)
            {
                audioListener.enabled = false;
                Debug.Log("[PlayerNetworkData] AudioListener desativado para jogador não-dono.");
            }
        }
    
        // Verificando se a câmera está configurada corretamente
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
            Debug.Log(playerCamera != null
                ? "[PlayerNetworkData] Câmera localizada com sucesso via GetComponentInChildren"
                : "[PlayerNetworkData] Câmera NÃO encontrada!");
        }

        // Se for o jogador local (dono da instância), ativa a câmera
        if (IsOwner && playerCamera != null)
        {
            Debug.Log("[PlayerNetworkData] Ativando câmera local.");
            playerCamera.enabled = true;

            var listener = playerCamera.GetComponent<AudioListener>();
            if (listener != null)
            {
                listener.enabled = true;
                Debug.Log("[PlayerNetworkData] AudioListener ativado.");
            }
            else
            {
                Debug.Log("[PlayerNetworkData] AudioListener não encontrado no playerCamera.");
            }
        }
        else if (playerCamera != null)
        {
            Debug.Log("[PlayerNetworkData] Desativando câmera (não é o dono).");
            playerCamera.enabled = false;

            var listener = playerCamera.GetComponent<AudioListener>();
            if (listener != null)
            {
                listener.enabled = false;
                Debug.Log("[PlayerNetworkData] AudioListener desativado.");
            }
            else
            {
                Debug.Log("[PlayerNetworkData] AudioListener não encontrado no playerCamera.");
            }
        }
    }
}
