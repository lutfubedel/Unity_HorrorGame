using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SanityVisuals : MonoBehaviour
{
    [Header("Referanslar")]
    public SanityController sanityController;
    public PostProcessVolume processVolume; // Sahnedeki Global Volume'ü buraya sürükle

    // Efekt deðiþkenleri
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private Grain grain;
    private LensDistortion lensDistortion;

    void Start()
    {
        // Profildeki ayarlarý çekiyoruz. Hata almamak için bu efektlerin profilde ekli olduðundan emin ol.
        processVolume.profile.TryGetSettings(out vignette);
        processVolume.profile.TryGetSettings(out chromaticAberration);
        processVolume.profile.TryGetSettings(out grain);
        processVolume.profile.TryGetSettings(out lensDistortion);
    }

    void Update()
    {
        if (sanityController == null) return;

        float factor = sanityController.InsanityFactor; // 0 (Ýyi) - 1 (Kötü)

        // 1. VIGNETTE (Kenar Kararmasý)
        if (vignette != null)
        {
            vignette.enabled.value = true;
            vignette.intensity.value = Mathf.Lerp(0.25f, 0.50f, factor); // Kararma miktarý
            vignette.smoothness.value = Mathf.Lerp(1f, 0.2f, factor); // Kenar sertliði
        }

        // 2. CHROMATIC ABERRATION (Renk Kaymasý)
        if (chromaticAberration != null)
        {
            chromaticAberration.enabled.value = true;
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 1f, factor); // Renkler ayrýlsýn
        }

        // 3. GRAIN (Kumlanma)
        if (grain != null)
        {
            grain.enabled.value = true;
            grain.intensity.value = Mathf.Lerp(0f, 1f, factor);
            grain.size.value = Mathf.Lerp(0.3f, 1.8f, factor); // Tanecik boyutu büyüsün
        }

        // 4. LENS DISTORTION (Ekran Bükülmesi)
        if (lensDistortion != null)
        {
            lensDistortion.enabled.value = true;
            // Hafif içe bükülme (-30)
            float targetDistortion = Mathf.Lerp(0f, -30f, factor);

            // Eðer çok delirdiyse (%80 üstü) ekran nefes alýp verir gibi bükülsün
            if (factor > 0.8f)
            {
                float pulse = Mathf.Sin(Time.time * 2f) * 10f; // Nabýz etkisi
                lensDistortion.intensity.value = targetDistortion + pulse;
            }
            else
            {
                lensDistortion.intensity.value = targetDistortion;
            }
        }
    }
}