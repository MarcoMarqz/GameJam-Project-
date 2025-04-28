using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Header("Laser Settings")]
    public float maxDistance = 100f;        // Maximum distance the laser can travel
    public float laserWidth = 0.1f;         // Width of the laser beam
    public Color laserColor = Color.red;    // Color of the laser beam
    public bool useHitEffects = true;       // Whether to show effects when laser hits something
    public float damageAmount = 10f;        // Damage per second if you want the laser to cause damage
    public LayerMask hitLayers;             // Layers the laser can hit

    [Header("References")]
    public Transform firePoint;             // Where the laser originates from
    public Material laserMaterial;          // Material for the laser beam
    public GameObject hitEffectPrefab;      // Optional effect shown where laser hits

    // Private variables
    private LineRenderer laserLine;
    private GameObject currentHitEffect;
    private bool isFiring = false;

    void Start()
    {
        // If no fire point is specified, use this object's position
        if (firePoint == null)
            firePoint = transform;

        // Set up the line renderer for the laser
        SetupLineRenderer();
    }

    void SetupLineRenderer()
    {
        // Get or add a LineRenderer component
        laserLine = GetComponent<LineRenderer>();
        if (laserLine == null)
            laserLine = gameObject.AddComponent<LineRenderer>();

        // Configure the line renderer for a laser appearance
        laserLine.startWidth = laserWidth;
        laserLine.endWidth = laserWidth;
        laserLine.positionCount = 2; // Start and end points only

        // Apply material if provided, otherwise create a default one
        if (laserMaterial != null)
        {
            laserLine.material = laserMaterial;
        }
        else
        {
            // Create a default material with emission
            Material defaultMaterial = new Material(Shader.Find("Particles/Standard Unlit"));
            defaultMaterial.SetColor("_Color", laserColor);
            defaultMaterial.SetColor("_EmissionColor", laserColor * 2f);
            defaultMaterial.EnableKeyword("_EMISSION");
            laserLine.material = defaultMaterial;
        }

        // Start with the laser turned off
        laserLine.enabled = false;
    }

    void Update()
    {
        // Check for the C key being pressed or held
        if (Input.GetKey(KeyCode.C))
        {
            // Start firing if not already
            if (!isFiring)
            {
                StartFiring();
            }

            // Update the laser while firing
            UpdateLaser();
        }
        else if (isFiring)
        {
            // Stop firing when key is released
            StopFiring();
        }
    }

    void StartFiring()
    {
        isFiring = true;
        laserLine.enabled = true;
    }

    void StopFiring()
    {
        isFiring = false;
        laserLine.enabled = false;

        // Clean up hit effect if it exists
        if (currentHitEffect != null)
        {
            Destroy(currentHitEffect);
            currentHitEffect = null;
        }
    }

    void UpdateLaser()
    {
        // Set start position at the fire point
        laserLine.SetPosition(0, firePoint.position);

        RaycastHit hit;
        Vector3 endPosition;

        // Cast a ray to see if the laser hits anything
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, maxDistance, hitLayers))
        {
            endPosition = hit.point;

            // Apply damage to the hit object if it has health
            ApplyDamage(hit.collider.gameObject);

            // Show hit effect if enabled
            if (useHitEffects && hitEffectPrefab != null)
            {
                UpdateHitEffect(hit.point, hit.normal);
            }
        }
        else
        {
            // If nothing is hit, the laser extends to its maximum distance
            endPosition = firePoint.position + (firePoint.forward * maxDistance);

            // Remove hit effect since nothing was hit
            if (currentHitEffect != null)
            {
                Destroy(currentHitEffect);
                currentHitEffect = null;
            }
        }

        // Set the end position of the laser
        laserLine.SetPosition(1, endPosition);
    }

    void UpdateHitEffect(Vector3 position, Vector3 normal)
    {
        // Create hit effect if it doesn't exist yet
        if (currentHitEffect == null)
        {
            currentHitEffect = Instantiate(hitEffectPrefab, position, Quaternion.LookRotation(normal));
        }
        else
        {
            // Update the position and rotation of the existing hit effect
            currentHitEffect.transform.position = position;
            currentHitEffect.transform.rotation = Quaternion.LookRotation(normal);
        }
    }

    void ApplyDamage(GameObject hitObject)
    {
        // Check for Enemy script on the hit object
        Enemy enemy = hitObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Apply damage over time (scaled by Time.deltaTime for frame rate independence)
            enemy.TakeDamage((int)(damageAmount * Time.deltaTime));
        }

        // You can add other damageable object types here
    }
}