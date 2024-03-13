using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerScript))]
[RequireComponent(typeof(PlayerInputSystem))]
public class PlayerUIManager : MonoBehaviour
{
    public enum UIScreenType
    {
        None,
        PauseMenu,
        AbilityScreen,
        DeathScreen
        // Add new screen types here as your game expands
    }

    [Header("Health")]
    [SerializeField]
    private GameObject heartPrefab;
    [SerializeField]
    private Transform healthUIContainer;

    [SerializeField]
    private Sprite healthEmpty;
    [SerializeField]
    private Sprite healthFull;
    [SerializeField]
    private Sprite shieldHealth;
    private List<GameObject> heartIcons = new List<GameObject>();

    [Header("Ability Select")]
    [SerializeField]
    private Transform abilitySelectTransform;
    [SerializeField]
    private Image dashIcon;
    [SerializeField]
    private Image quickSkillIcon;
    [SerializeField]
    private Image strongSkillIcon;

    public Image dashIconCooldown;
    public Image quickSkillIconCooldown;
    public Image strongSkillIconCooldown;

    [SerializeField]
    private Transform pauseMenuTransform;
    [SerializeField]
    private Transform deathScreenTransform;
    private PlayerScript playerScript;
    private PlayerInputSystem inputSystem;

    public static bool isGamePaused = false;

    private Dictionary<UIScreenType, Transform> screenMap;
    void Awake()
    {
        screenMap = new Dictionary<UIScreenType, Transform>() {
            {UIScreenType.AbilityScreen, abilitySelectTransform },
            {UIScreenType.PauseMenu, pauseMenuTransform },
            {UIScreenType.DeathScreen, deathScreenTransform }
        };

        playerScript = GetComponent<PlayerScript>();
        inputSystem = GetComponent<PlayerInputSystem>();
        playerScript.onHealthChange += UpdateHealthUI;
        inputSystem.onAbilitySelectMenu += ToggleAbilityScreen;
        inputSystem.onPauseMenu += TogglePauseMenu;

    }

    private void Start()
    {
        CloseCurrentScreen();
        Pause(false);
    }

    private UIScreenType currentScreen = UIScreenType.None;

    public void ToggleAbilityScreen()
    {
        if (currentScreen != UIScreenType.AbilityScreen)
        {
            OpenScreen(UIScreenType.AbilityScreen);
        } else
        {
            CloseCurrentScreen();
        }
    }

    public void OpenScreen(UIScreenType screenType)
    {
        if (CanOpenScreen(screenType))
        {
            CloseCurrentScreen();
            // Logic to open the requested screen
            // For example, set currentScreen to the new screen
            currentScreen = screenType;

            if (screenMap.ContainsKey(screenType))
            {
                Transform screenToActivate = screenMap[screenType];
                if (screenToActivate != null) screenToActivate.gameObject.SetActive(true);

                // Pause game if necessary
                if (screenType == UIScreenType.PauseMenu || screenType == UIScreenType.DeathScreen || screenType == UIScreenType.AbilityScreen)
                {
                    Pause(true);
                }
            }
        }
    }

    private void CloseCurrentScreen()
    {
        if (currentScreen != UIScreenType.None && screenMap.ContainsKey(currentScreen))
        {
            Transform screenToDeactivate = screenMap[currentScreen];
            if (screenToDeactivate != null) screenToDeactivate.gameObject.SetActive(false);
            currentScreen = UIScreenType.None;

            // Resume game logic if necessary
            Pause(false);
        }
    }

    private bool CanOpenScreen(UIScreenType screenType)
    {
        // Logic to determine if a screen can be opened
        // For example, do not open any screen if the currentScreen is PauseMenu
        // and do not open PauseMenu if currentScreen is AbilityScreen
        if (currentScreen == UIScreenType.DeathScreen)
        {
            return false; // Don't allow switching screens when the Death Screen is displayed
        }

        if((currentScreen == UIScreenType.PauseMenu || currentScreen == UIScreenType.AbilityScreen) && screenType == UIScreenType.DeathScreen) {
            return true; // Allow opening Death Screen over other screens
        }

        // Existing conditions for PauseMenu and AbilityScreen
        if (currentScreen == UIScreenType.PauseMenu && screenType == UIScreenType.AbilityScreen)
        {
            return false;
        }
        if (currentScreen == UIScreenType.AbilityScreen && screenType == UIScreenType.PauseMenu)
        {
            return false;
        }

        return true; // Modify conditions as necessary
    }

    public void TogglePauseMenu()
    {
        // Ensure the Pause Menu cannot be toggled when the Death Screen is active
        if (currentScreen != UIScreenType.DeathScreen)
        {
            if (currentScreen == UIScreenType.None || currentScreen == UIScreenType.PauseMenu)
            {
                if (currentScreen == UIScreenType.PauseMenu)
                {
                    CloseCurrentScreen();
                }
                else
                {
                    OpenScreen(UIScreenType.PauseMenu);
                }
            }
        }
    }

    public void ToggleDeathScreen()
    {
        if (currentScreen == UIScreenType.None || currentScreen == UIScreenType.DeathScreen)
        {
            if (currentScreen == UIScreenType.DeathScreen)
            {
                CloseCurrentScreen();
            }
            else
            {
                OpenScreen(UIScreenType.DeathScreen);
            }
        }
    }

    public void ShowDeathScreen()
    {
        OpenScreen(UIScreenType.DeathScreen);
        Pause(true);
        // Additional logic to handle the game state on death, such as stopping gameplay, can be added here
    }
    public void Pause(bool pause)
    {
        isGamePaused = pause;
        int scale = pause ? 0 : 1;
        Time.timeScale = 1 * scale;
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

    public void UpdateDashIcon(Sprite icon)
    {
        dashIcon.sprite = icon;
    }

    public void UpdateQuickSkillIcon(Sprite icon)
    {
        quickSkillIcon.sprite = icon;
    }

    public void UpdateStrongSkillIcon(Sprite icon)
    {
        strongSkillIcon.sprite = icon;
    }
}

