using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Image fadeUI;
    [SerializeField]
    private string levelName;
    [SerializeField]
    private float fadeSpeed = 0.8f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure your player GameObject has the "Player" tag.
        {
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(fadeUI.gameObject);
            fadeUI.gameObject.SetActive(true);
            StartCoroutine(FadeAndLoadLevel(levelName));
        }
    }

    IEnumerator FadeAndLoadLevel(string levelName)
    {
        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene(levelName);
        yield return StartCoroutine(FadeFromBlack());
    }

    IEnumerator FadeToBlack()
    {
        float alpha = fadeUI.color.a;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeSpeed;
            fadeUI.color = new Color(fadeUI.color.r, fadeUI.color.g, fadeUI.color.b, alpha);
            yield return null;
        }
    }

    IEnumerator FadeFromBlack()
    {
        // This ensures we're starting from a fully black screen.
        fadeUI.color = new Color(fadeUI.color.r, fadeUI.color.g, fadeUI.color.b, 1);

        float alpha = fadeUI.color.a;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeSpeed;
            fadeUI.color = new Color(fadeUI.color.r, fadeUI.color.g, fadeUI.color.b, alpha);
            yield return null;
        }

        Destroy(this.gameObject);
        Destroy(fadeUI.gameObject);
    }

    // Optionally, to ensure a smooth transition, you might want to use SceneManager.sceneLoaded to trigger the FadeFromBlack.
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeFromBlack()); // Ensure fade in happens after the scene is loaded.
    }
}
