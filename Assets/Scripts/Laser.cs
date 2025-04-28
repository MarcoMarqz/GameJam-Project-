using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Header("Laser Visual Settings")]
    public float maxDistance = 100f;
    public float laserWidth = 0.1f;
    public Color laserColor = Color.red;
    public Material laserMaterial;
    public GameObject hitEffectPrefab;
    public Transform firePoint;
    public LayerMask hitLayers;

    [Header("Invisible Bullet Settings")]
    public GameObject laserBulletPrefab; // Prefab for invisible damage bullets
    public float fireRate = 0.1f; // Time between each invisible bullet spawn
    public float bulletSpeed = 50f; // How fast the invisible bullet moves

    private LineRenderer laserLine;
    private GameObject currentHitEffect;
    private bool isFiring = false;
    private float fireTimer = 0f;

    void Start()
    {
        if (firePoint == null)
            firePoint = transform;

        SetupLineRenderer();
    }

    void SetupLineRenderer()
    {
        laserLine = GetComponent<LineRenderer>();
        if (laserLine == null)
            laserLine = gameObject.AddComponent<LineRenderer>();

        laserLine.startWidth = laserWidth;
        laserLine.endWidth = laserWidth;
        laserLine.positionCount = 2;

        if (laserMaterial != null)
        {
            laserLine.material = laserMaterial;
        }
        else
        {
            Material defaultMaterial = new Material(Shader.Find("Particles/Standard Unlit"));
            defaultMaterial.SetColor("_Color", laserColor);
            defaultMaterial.SetColor("_EmissionColor", laserColor * 2f);
            defaultMaterial.EnableKeyword("_EMISSION");
            laserLine.material = defaultMaterial;
        }

        laserLine.enabled = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            if (!isFiring)
                StartFiring();

            UpdateLaser();
            FireInvisibleBullets();
        }
        else if (isFiring)
        {
            StopFiring();
        }
    }

    void StartFiring()
    {
        isFiring = true;
        laserLine.enabled = true;
        fireTimer = 0f; // Reset timer immediately
    }

    void StopFiring()
    {
        isFiring = false;
        laserLine.enabled = false;

        if (currentHitEffect != null)
        {
            Destroy(currentHitEffect);
            currentHitEffect = null;
        }
    }

    void UpdateLaser()
    {
        laserLine.SetPosition(0, firePoint.position);

        RaycastHit hit;
        Vector3 endPosition;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, maxDistance, hitLayers))
        {
            endPosition = hit.point;

            if (hitEffectPrefab != null)
            {
                UpdateHitEffect(hit.point, hit.normal);
            }
        }
        else
        {
            endPosition = firePoint.position + (firePoint.forward * maxDistance);

            if (currentHitEffect != null)
            {
                Destroy(currentHitEffect);
                currentHitEffect = null;
            }
        }

        laserLine.SetPosition(1, endPosition);
    }

    void UpdateHitEffect(Vector3 position, Vector3 normal)
    {
        if (currentHitEffect == null)
        {
            currentHitEffect = Instantiate(hitEffectPrefab, position, Quaternion.LookRotation(normal));
        }
        else
        {
            currentHitEffect.transform.position = position;
            currentHitEffect.transform.rotation = Quaternion.LookRotation(normal);
        }
    }

    void FireInvisibleBullets()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            GameObject bullet = Instantiate(laserBulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * bulletSpeed;
            }

            fireTimer = 0f; // Reset the fire timer
        }
    }
}
