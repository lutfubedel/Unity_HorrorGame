using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class NoteType
{
    public int id;
    public string title;
    public string message;
}


public class Note : MonoBehaviour
{

    public List<NoteType> notes = new List<NoteType>()
    {
        new NoteType {
            id = 1,
            title = "Sesi Kesilmiyor",
            message = "Bu odanýn duvarlarýnýn arkasýnda biri sürekli fýsýldýyor. Geceleri adýmý tekrar ediyor. 'Sen konuþmadýkça ben konuþacaðým' dedi. Ne olur beni yalnýz býrakmayýn."
        },

        new NoteType {
            id = 2,
            title = "Ýlaçlar Ýþe Yaramýyor",
            message = "Bugün dozumu artýrdýlar ama sesler hâlâ burada. 'Sýradaki onlar' diyorlar. Doktorlar duymuyor. Sesler artýk yüzüme dokunuyor."
        },

        new NoteType {
            id = 3,
            title = "Tedavi Odasýnda Olanlar",
            message = "Odayý temizlemeye geldiklerinde beni baðladýlar ama ben hareket etmedim. Kollarýmý kývýran onlar deðildi. Duvarýn arkasýndaki o þeydi."
        },

        new NoteType {
            id = 4,
            title = "Son Mesaj",
            message = "Yüzü olmayan adam sürekli kapýnýn arkasýnda duruyor. Artýk dayanamadým. Eðer bunu bulduysan kaç. Belki de artýk sana bulaþtý."
        },

        new NoteType {
            id = 5,
            title = "O Gözünü Açtý",
            message = "Sayfanýn üstünde titrek bir yazý: 'O GÖZÜNÜ AÇTI'. Altýnda koridorun sonuna çizilmiþ siyah bir figür. 'Uyuduðunu sanýyordum.'"
        },

        new NoteType {
            id= 6,
            title = "Ölemeyenler",
            message = "Burada kimse ölmez. Ýlaçlar, iðneler, baðlamalar... hiçbiri iþe yaramýyor. Ben üç kere öldüm ama sabah kendimi yataðýmda buldum. Neden hâlâ buradayým?"
        },

        new NoteType {
            id = 7,
            title = "Doktorun Notu",
            message = "Hasta 23, gece 03.15'te 'Onu içeri almayýn' diye baðýrarak uyandý. Odaya girdiðimizde kapý kapalýydý. O kapýyý hiç açmamýþtýk."
        },

        new NoteType {
            id = 8,
            title = "Kapý 7",
            message = "KAPI 7. KAPI 7. KAPI 7. Açmayýn. O kapý açýlýrsa ben de açýlýrým. Ben açýlýrsam o çýkar."
        },

        new NoteType {
            id = 9,
            title = "MR Görüntüsü",
            message = "MR sýrasýnda görüntüde bir yüz belirdi. Doktora gösterdim, 'bozulma' dedi. Ama o yüz fýsýldadý. Adýmý söyledi. Yarýn yeniden MR’a alacaklar. Gitmek istemiyorum."
        },

        new NoteType {
            id = 10,
            title = "Aralýklardayým",
            message = "Eðer bunu bulduysan bilin ki artýk buradayým ama görünmezim. Kapýlarýn ve duvarlarýn arasýndaki boþluklarda sýkýþtým. Yalnýz deðilim."
        }
    };



    public void CloseTheNote()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
