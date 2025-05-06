using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    
[Header("Map-Container")]
public GameObject outsideMap;
public GameObject insideMap;

[Header("Optionale Fade-Dauer")]
public float fadeTime = 0.5f;

private bool switched = false;

void Start()
{
    // Starte mit außen aktiv, innen deaktiviert
    outsideMap.SetActive(true);
    insideMap.SetActive(false);
}

void OnTriggerEnter(Collider other)
{
    if (switched) return;            // nur einmal schalten
    if (other.CompareTag("Player"))  // oder dein eigener Spieler-Tag
    {
        StartCoroutine(SwitchMaps());
        switched = true;
    }
}

private IEnumerator SwitchMaps()
{
    // --- Optional: Bildschirm ausblenden (z. B. CanvasGroup) ---
     yield return StartCoroutine(FadeOut());

    // Maps umschalten
    outsideMap.SetActive(false);
    insideMap.SetActive(true);

    // --- Optional: Bildschirm wieder einblenden ---
     yield return StartCoroutine(FadeIn());
        
    yield break;
}

// Beispiel-Fade mit CanvasGroup (falls du einen Fullscreen-UI-Block nutzt)

public CanvasGroup fadeCanvas;
private IEnumerator FadeOut()
{
    float t = 0;
    while (t < fadeTime)
    {
        fadeCanvas.alpha = Mathf.Lerp(0, 1, t / fadeTime);
        t += Time.deltaTime;
        yield return null;
    }
    fadeCanvas.alpha = 1;
}
private IEnumerator FadeIn()
{
    float t = 0;
    while (t < fadeTime)
    {
        fadeCanvas.alpha = Mathf.Lerp(1, 0, t / fadeTime);
        t += Time.deltaTime;
        yield return null;
    }
    fadeCanvas.alpha = 0;
}


}
