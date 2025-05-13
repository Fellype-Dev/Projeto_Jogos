using UnityEngine;

public class Revolver : MonoBehaviour
{
    [Header("Configuração do Tiro")]
    public float fireCooldown = 1.0f; // Tempo entre tiros
    public int maxAmmo = 6; // Balas por recarga
    public float reloadTime = 2f;

    [Header("Referências")]
    public Transform firePoint; // Posição da saída do tiro
    public ParticleSystem muzzleFlash; // Efeito de disparo (opcional)
    public GameObject hitEffectPrefab; // Efeito ao atingir algo (opcional)
    public AudioSource shotSound; // Som de tiro (opcional)

    private int currentAmmo;
    private float lastFireTime;
    private bool isReloading;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetButtonDown("Fire1") && Time.time - lastFireTime >= fireCooldown)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Reload());
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
{
    lastFireTime = Time.time;
    currentAmmo--;

    // Toca o som se estiver atribuído
    if (shotSound != null)
        shotSound.Play();

    // Toca o efeito visual se estiver atribuído
    if (muzzleFlash != null)
        muzzleFlash.Play();

    RaycastHit hit;
    if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 100f))
    {
        Debug.Log("Acertou: " + hit.collider.name);

        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Recarregando...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
