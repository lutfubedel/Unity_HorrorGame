using UnityEngine;

public class SanityController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float maxSanity = 100f;
    [Tooltip("Þu anki akýl saðlýðý")]
    public float currentSanity;

    [Tooltip("Saniyede azalacak akýl saðlýðý miktarý")]
    public float decayRate = 1f;

    // Bu deðer 0 ile 1 arasýndadýr. 0: Saðlýklý, 1: Tamamen Deli.
    // Diðer scriptler bu deðeri okuyacak.
    public float InsanityFactor { get; private set; }

    void Start()
    {
        currentSanity = maxSanity;
    }

    void Update()
    {
        // Akýl saðlýðýný zamanla düþür
        if (currentSanity > 0)
        {
            currentSanity -= decayRate * Time.deltaTime;
        }

        // Deðeri 0 ile Max arasýnda tut
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);

        // Delilik oranýný hesapla (Tersi alýnýr)
        InsanityFactor = 1f - (currentSanity / maxSanity);
    }

    // Dýþarýdan hasar vermek istersen (örn: canavar görünce) bu fonksiyonu çaðýr
    public void TakeSanityDamage(float amount)
    {
        currentSanity -= amount;
    }

    // Ýlaç alýnca vs. bu fonksiyonu çaðýr
    public void RestoreSanity(float amount)
    {
        currentSanity += amount;
    }
}