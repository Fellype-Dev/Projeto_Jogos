using UnityEngine;

public class PlayerInteractionInventory : MonoBehaviour
{
    public Transform holdPosition;
    public float interactionRange = 3f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.G;

    private GameObject[] inventory = new GameObject[2];

    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
            TryPickupItem();

        if (Input.GetKeyDown(dropKey))
            DropItem();
    }

    void TryPickupItem()
    {
        if (inventory[0] != null && inventory[1] != null)
        {
            Debug.Log("Inventário cheio!");
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange);
        foreach (Collider col in colliders)
        {
            GameObject item = col.gameObject;

            if (item.CompareTag("Item") && !IsInInventory(item))
            {
                AddToInventory(item);
                return;
            }
        }

        Debug.Log("Nenhum item próximo.");
    }

    void AddToInventory(GameObject item)
    {
        int slot = inventory[0] == null ? 0 : 1;

        inventory[slot] = item;
        item.transform.SetParent(holdPosition);
        item.transform.localPosition = new Vector3(0.3f * slot, 0, 0);
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Debug.Log("Item adicionado: " + item.name);
    }

    void DropItem()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                GameObject item = inventory[i];
                inventory[i] = null;

                item.transform.SetParent(null);
                item.transform.position = holdPosition.position;

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.velocity = Vector3.zero;
                    rb.AddForce(transform.forward * 1.5f + Vector3.up * 2f, ForceMode.Impulse);
                }

                Debug.Log("Item dropado: " + item.name);
                RearrangeItems();
                return;
            }
        }

        Debug.Log("Nenhum item para dropar.");
    }

    void RearrangeItems()
    {
        int index = 0;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                inventory[i].transform.localPosition = new Vector3(0.3f * index, 0, 0);
                index++;
            }
        }
    }

    bool IsInInventory(GameObject item)
    {
        return inventory[0] == item || inventory[1] == item;
    }
}
