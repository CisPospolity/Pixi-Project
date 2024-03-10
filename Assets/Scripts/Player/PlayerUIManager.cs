using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerScript))]
public class PlayerUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject heartPrefab;
    [SerializeField]
    private Transform healthUIContainer;

    [Header("Sprites")]
    [SerializeField]
    private Sprite healthEmpty;
    [SerializeField]
    private Sprite healthFull;
    [SerializeField]
    private Sprite shieldHealth;
    private List<GameObject> heartIcons = new List<GameObject>();

    private PlayerScript playerScript;

    void Awake()
    {
        playerScript = GetComponent<PlayerScript>();
        playerScript.OnHealthChange += UpdateHealthUI;
    }

    public void UpdateHealthUI(int maxHealth, int currentHealth, int shield)
    {
        // Adjust the list size to match maxHealth
        while (heartIcons.Count < maxHealth)
        {
            GameObject newHeart = Instantiate(heartPrefab, healthUIContainer);
            heartIcons.Add(newHeart);
        }

        while (heartIcons.Count > maxHealth)
        {
            GameObject toRemove = heartIcons[heartIcons.Count - 1];
            heartIcons.Remove(toRemove);
            Destroy(toRemove);
        }

        for (int i = 0; i < heartIcons.Count; i++)
        {
            Image heartImage = heartIcons[i].GetComponent<Image>();

            if (i < currentHealth)
            {
                heartImage.sprite = healthFull; // Assuming you have a reference to this sprite
            }
            else
            {
                heartImage.sprite = healthEmpty; // Assuming you have a reference to this sprite
            }

            // Optional: Handle shield overlay here if needed
            if (i < shield)
            {
                // Add shield effect
                heartImage.overrideSprite = shieldHealth;
            } else
            {
                heartImage.overrideSprite = null;
            }
        }
    }
}

