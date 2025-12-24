using UnityEngine;

public class SanityShake : MonoBehaviour
{
    public SanityController sanityController;

    [Header("Sallantý Ayarlarý")]
    public float shakeAmount = 0.5f; // Pozisyon sallantý gücü
    public float rollAmount = 2.0f;  // Z ekseni (kafa yatýrma) gücü
    public float shakeSpeed = 1.0f;  // Sallanma hýzý

    private Vector3 initialPosition;

    void Start()
    {
        sanityController = GameObject.FindWithTag("GlobalVolume").GetComponent<SanityController>();
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (sanityController == null) return;

        float factor = sanityController.InsanityFactor;

        // Eðer akýl saðlýðý iyiyse (%10 delilikten azsa) iþlem yapma
        if (factor < 0.1f)
        {
            // Yavaþça orijinal pozisyona ve düz açýya dön
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime);

            // FPS Mouse Look X ve Y'yi yönetir, biz sadece Z'yi (Roll) düzeltiyoruz
            Vector3 currentRot = transform.localEulerAngles;
            currentRot.z = Mathf.LerpAngle(currentRot.z, 0, Time.deltaTime);
            transform.localEulerAngles = currentRot;
            return;
        }

        // Perlin Noise ile rastgele ama akýcý deðerler üret
        float noiseX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0) - 0.5f) * shakeAmount * factor;
        float noiseY = (Mathf.PerlinNoise(0, Time.time * shakeSpeed) - 0.5f) * shakeAmount * factor;
        float noiseZ = (Mathf.PerlinNoise(Time.time * shakeSpeed, Time.time * shakeSpeed) - 0.5f) * rollAmount * factor * 5f; // Z ekseni daha belirgin olsun

        // 1. Pozisyonu Salla (Baþ dönmesi hissi)
        transform.localPosition = initialPosition + new Vector3(noiseX, noiseY, 0);

        // 2. Rotasyonu Salla (Sadece Z Ekseni - Roll)
        // Mevcut X ve Y açýsýný koru (Mouse Look bozulmasýn diye), sadece Z'yi deðiþtir.
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.z = noiseZ;
        transform.localEulerAngles = currentRotation;
    }
}