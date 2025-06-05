using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    [System.Serializable]
    public class ChamberState
    {
        public GameObject chamber;
        public bool analysisDone = false;
        public bool isAnalyzing = false;
        public float analysisTimer = 0f;
    }

    public float analysisDuration = 3f;
    public List<ChamberState> analiseStates = new List<ChamberState>();
    private bool isNearChamber = false;

    public bool hasBox = false;
    public bool hasDepositado = false;
    public GameObject currentBox;
    private GameObject carregadaBox;
    public List<GameObject> boxObjects;

    public bool hasHD = false;
    public bool hdInserted = false;
    public bool transferComplete = false;
    public bool hdComDados = false;
    public GameObject currentHD;
    private GameObject carregadoHD;
    public List<GameObject> hdObjects;

    public Transform inventorySlot;
    public float interactionDistance = 5f;
    public List<GameObject> dropZones;
    public List<GameObject> ListaComputadores;
    public List<GameObject> ListaServidores;
    public PlayerInteractionUI interactionUI;
    public TransferManager transferManager;
    public TaskHUD taskHUD;
    public GameObject droppedHDPrefab;

    private void Start()
    {
        boxObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Box"));
        hdObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("HD"));
        ListaComputadores = new List<GameObject>(GameObject.FindGameObjectsWithTag("Computador"));
        ListaServidores = new List<GameObject>(GameObject.FindGameObjectsWithTag("Servidor"));

        foreach (GameObject chamber in GameObject.FindGameObjectsWithTag("Camara"))
        {
            analiseStates.Add(new ChamberState { chamber = chamber });
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && hasHD && carregadoHD != null)
        {
            hasHD = false;
            Vector3 dropPosition = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
            GameObject droppedHD = Instantiate(droppedHDPrefab, dropPosition, Quaternion.identity);
            droppedHD.tag = "HD";
            hdObjects.Add(droppedHD);

            Destroy(carregadoHD);
            carregadoHD = null;
            Debug.Log("HD dropado.");
        }

        if (Input.GetKeyDown(KeyCode.Q) && hdInserted && !transferComplete)
        {
            Debug.Log("Iniciando transferência de dados...");
            transferManager.StartTransfer();
        }

        currentBox = GetNearestBox();
        currentHD = GetNearestHD();
        ChamberState currentChamber = GetNearestChamberState();

        if (currentChamber != null)
        {
            float distance = Vector3.Distance(transform.position, currentChamber.chamber.transform.position);
            isNearChamber = distance <= interactionDistance;

            if (isNearChamber && Input.GetKeyDown(KeyCode.E))
            {
                if (!currentChamber.isAnalyzing && !currentChamber.analysisDone)
                {
                    currentChamber.isAnalyzing = true;
                    currentChamber.analysisTimer = 0f;
                    Debug.Log("Iniciando análise.");
                }
                else if (currentChamber.isAnalyzing && currentChamber.analysisTimer >= analysisDuration)
                {
                    currentChamber.isAnalyzing = false;
                    currentChamber.analysisDone = true;
                    Debug.Log("Análise concluída para esta câmara.");
                    taskHUD.CompleteTask("ANÁLISE DE OPERÁRIOS");
                }
            }

            if (currentChamber.isAnalyzing)
            {
                currentChamber.analysisTimer += Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!hasBox && currentBox != null && IsNear(currentBox))
            {
                hasBox = true;
                hasDepositado = false;
                carregadaBox = currentBox;
                carregadaBox.SetActive(false);
                carregadaBox.transform.SetParent(inventorySlot);
                carregadaBox.transform.localPosition = Vector3.zero;
                carregadaBox.transform.localRotation = Quaternion.identity;
                boxObjects.Remove(carregadaBox);
                currentBox = null;
                Debug.Log("Caixa coletada.");
            }
            else if (hasBox && IsNear(GetNearestDropZone()))
            {
                hasBox = false;
                hasDepositado = true;
                Debug.Log("Caixa depositada.");
                taskHUD.CompleteTask("TRANSPORTE DE CAIXAS");
                Destroy(carregadaBox);
                carregadaBox = null;
            }
            else if (!hasHD && currentHD != null && IsNear(currentHD))
            {
                Debug.Log("Tentando coletar HD...");

                hasHD = true;
                carregadoHD = currentHD;

                // Verificação do componente HDData
                HDData hdData = carregadoHD.GetComponent<HDData>();
                if (hdData != null)
                {
                    Debug.Log("Componente HDData encontrado. hasData = " + hdData.hasData);
                }
                else
                {
                    Debug.LogWarning("HD coletado não possui componente HDData.");
                }

                carregadoHD.SetActive(false);
                carregadoHD.transform.SetParent(inventorySlot);
                carregadoHD.transform.localPosition = Vector3.zero;
                carregadoHD.transform.localRotation = Quaternion.identity;
                hdObjects.Remove(carregadoHD);
                currentHD = null;

                Debug.Log("HD coletado com sucesso.");
            }
            else if (hasHD && IsNearComputador() && !hdInserted && !hdComDados)
            {
                hdInserted = true;
                hasHD = false;

                if (carregadoHD != null)
                {
                    carregadoHD.SetActive(false);
                    Debug.Log("HD inserido no computador.");
                }
                else
                {
                    Debug.LogWarning("carregadoHD é nulo ao tentar inserir no computador.");
                }
            }
            else if (hdInserted && transferComplete && IsNearComputador())
            {
                hdInserted = false;
                hasHD = true;
                transferComplete = false;
                hdComDados = true;

                if (carregadoHD != null)
                {
                    carregadoHD.SetActive(true);
                    carregadoHD.transform.SetParent(inventorySlot);
                    carregadoHD.transform.localPosition = Vector3.zero;
                    carregadoHD.transform.localRotation = Quaternion.identity;
                    Debug.Log("HD retirado com dados.");
                }
                else
                {
                    Debug.LogError("carregadoHD está nulo ao tentar retirar HD do computador.");
                }
            }
            else if (hasHD && IsNearServidor() && hdComDados)
            {
                hasHD = false;
                hdComDados = false;

                if (taskHUD != null)
                    taskHUD.CompleteTask("TRANSFERÊNCIA DE DADOS");
                else
                    Debug.LogError("taskHUD não foi configurado!");

                Debug.Log("HD inserido no servidor. Tarefa concluída.");

                if (carregadoHD != null)
                {
                    Destroy(carregadoHD);
                    carregadoHD = null;
                }
                else
                {
                    Debug.LogWarning("carregadoHD já está nulo ao tentar destruir.");
                }
            }
        }
    }

    private void LateUpdate()
    {
        ChamberState chamber = GetNearestChamberState();
        if (chamber != null)
        {
            float dist = Vector3.Distance(transform.position, chamber.chamber.transform.position);
            if (dist <= interactionDistance)
            {
                if (!chamber.isAnalyzing && !chamber.analysisDone)
                {
                    interactionUI.MostrarTexto("[E] Iniciar análise");
                }
                else if (chamber.isAnalyzing && chamber.analysisTimer < analysisDuration)
                {
                    interactionUI.MostrarTexto("Analisando... (" + (analysisDuration - chamber.analysisTimer).ToString("F1") + "s)");
                }
                else if (chamber.isAnalyzing && chamber.analysisTimer >= analysisDuration)
                {
                    interactionUI.MostrarTexto("[E] Finalizar análise");
                }
                else if (chamber.analysisDone)
                {
                    interactionUI.MostrarTexto("Análise feita");
                }
                return;
            }
        }

        // Outros textos de interação
        if (currentBox != null && !hasBox && IsNear(currentBox))
        {
            interactionUI.MostrarTexto("[E] Pegar Caixa");
        }
        else if (hasBox && !hasDepositado && GetNearestDropZone() != null)
        {
            interactionUI.MostrarTexto("[E] Depositar Caixa");
        }
        else if (hasDepositado && IsNear(GetNearestDropZone()))
        {
            interactionUI.MostrarTexto("Caixa depositada");
        }
        else if (currentHD != null && hasHD && !hdInserted)
        {
            interactionUI.MostrarTexto("[E] Retirar HD");
        }
        else if (currentHD != null && !hasHD && IsNear(currentHD))
        {
            interactionUI.MostrarTexto("[E] Pegar HD");
        }
        else if (IsNearComputador() && hasHD && !hdInserted && !hdComDados)
        {
            interactionUI.MostrarTexto("[E] Inserir HD no Computador");
        }
        else if (hdInserted && !transferComplete && IsNearComputador())
        {
            interactionUI.MostrarTexto("[Q] Iniciar Transferência");
        }
        else if (hdInserted && transferComplete && IsNearComputador())
        {
            interactionUI.MostrarTexto("[E] Retirar HD com Dados");
        }
        else if (IsNearServidor() && hasHD && hdComDados)
        {
            interactionUI.MostrarTexto("[E] Inserir HD no Servidor");
        }
        else
        {
            interactionUI.EsconderTexto();
        }
    }

    // Métodos auxiliares
    private GameObject GetNearestBox() => GetNearestObject(boxObjects);
    private GameObject GetNearestHD() => GetNearestObject(hdObjects);
    private GameObject GetNearestDropZone() => GetNearestObject(dropZones);

    private GameObject GetNearestObject(List<GameObject> list)
    {
        GameObject nearest = null;
        float dist = Mathf.Infinity;
        foreach (GameObject obj in list)
        {
            if (obj != null)
            {
                float d = Vector3.Distance(transform.position, obj.transform.position);
                if (d < dist && d <= interactionDistance)
                {
                    nearest = obj;
                    dist = d;
                }
            }
        }
        return nearest;
    }

    private ChamberState GetNearestChamberState()
    {
        ChamberState nearest = null;
        float dist = Mathf.Infinity;
        foreach (ChamberState state in analiseStates)
        {
            if (state.chamber != null)
            {
                float d = Vector3.Distance(transform.position, state.chamber.transform.position);
                if (d < dist && d <= interactionDistance)
                {
                    nearest = state;
                    dist = d;
                }
            }
        }
        return nearest;
    }

    private bool IsNear(GameObject obj)
    {
        return obj != null && Vector3.Distance(transform.position, obj.transform.position) <= interactionDistance;
    }

    private bool IsNearComputador()
    {
        foreach (GameObject comp in ListaComputadores)
        {
            if (comp != null && Vector3.Distance(transform.position, comp.transform.position) <= interactionDistance)
                return true;
        }
        return false;
    }

    private bool IsNearServidor()
    {
        foreach (GameObject servidor in ListaServidores)
        {
            if (servidor != null && Vector3.Distance(transform.position, servidor.transform.position) <= interactionDistance)
                return true;
        }
        return false;
    }
}
