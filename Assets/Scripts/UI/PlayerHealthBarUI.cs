using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the player health bar UI - positioned at top-left of screen
/// Similar to enemy health manager but for screen-space UI
/// </summary>
public class PlayerHealthBarUI : MonoBehaviour
{
    public static PlayerHealthBarUI Instance { get; private set; }

    [Header("UI References")]
    [Tooltip("The Image component for the background. Assign your custom sprite to this Image's 'Source Image' field.")]
    public Image healthBarBackground;
    
    [Tooltip("The Image component for the fill (shows current health). Assign your custom sprite to this Image's 'Source Image' field.")]
    public Image healthBarFill;
    
    [Tooltip("The Image component for the border/box. Assign your custom sprite to this Image's 'Source Image' field.")]
    public Image healthBarBox;

    [Header("Health Bar Settings")]
    [Tooltip("Use sprite's original colors instead of color tinting. Enable this if you have custom colored sprites!")]
    public bool useNativeSpriteColors = false;
    
    [Tooltip("Color of the health fill when at full/high health (only used if 'Use Native Sprite Colors' is OFF)")]
    public Color healthyColor = new Color(0.2f, 1f, 0.4f); // Bright green
    
    [Tooltip("Color of the health fill when at medium health (only used if 'Use Native Sprite Colors' is OFF)")]
    public Color mediumHealthColor = new Color(1f, 0.8f, 0f); // Yellow/Orange
    
    [Tooltip("Color of the health fill when at low health (only used if 'Use Native Sprite Colors' is OFF)")]
    public Color lowHealthColor = new Color(1f, 0.3f, 0.2f); // Red
    
    [Tooltip("Health percentage threshold for medium color (0-1)")]
    [Range(0f, 1f)]
    public float mediumHealthThreshold = 0.5f;
    
    [Tooltip("Health percentage threshold for low color (0-1)")]
    [Range(0f, 1f)]
    public float lowHealthThreshold = 0.25f;

    [Header("Animation Settings")]
    [Tooltip("Speed of the health bar fill animation")]
    public float fillAnimationSpeed = 5f;
    
    [Tooltip("Enable smooth lerp animation when health changes")]
    public bool useSmoothAnimation = true;

    private float currentFillAmount = 1f;
    private float targetFillAmount = 1f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Validate UI components
        ValidateComponents();
        
        // Initialize health bar to full
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = 1f;
            
            // Ensure background and box use native sprite colors
            if (healthBarBackground != null)
                healthBarBackground.color = Color.white;
            if (healthBarBox != null)
                healthBarBox.color = Color.white;
            
            UpdateHealthColor(1f);
        }
        else
        {
            Debug.LogWarning("PlayerHealthBarUI: Health bar fill image is not assigned!");
        }
    }

    /// <summary>
    /// Validate that all required components are assigned
    /// </summary>
    private void ValidateComponents()
    {
        bool hasErrors = false;

        if (healthBarBackground == null)
        {
            Debug.LogWarning("PlayerHealthBarUI: Background Image is not assigned! Assign it in the Inspector.");
            hasErrors = true;
        }

        if (healthBarFill == null)
        {
            Debug.LogWarning("PlayerHealthBarUI: Fill Image is not assigned! Assign it in the Inspector.");
            hasErrors = true;
        }

        if (healthBarBox == null)
        {
            Debug.LogWarning("PlayerHealthBarUI: Box Image is not assigned! Assign it in the Inspector.");
            hasErrors = true;
        }

        if (!hasErrors)
        {
            Debug.Log("✓ Player Health Bar UI is properly configured!");
        }
    }

    private void Update()
    {
        // Smooth health bar animation
        if (useSmoothAnimation && healthBarFill != null)
        {
            if (Mathf.Abs(currentFillAmount - targetFillAmount) > 0.001f)
            {
                currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * fillAnimationSpeed);
                healthBarFill.fillAmount = currentFillAmount;
            }
        }
    }

    /// <summary>
    /// Update the health bar display
    /// </summary>
    /// <param name="currentHealth">Current health value</param>
    /// <param name="maxHealth">Maximum health value</param>
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthBarFill == null)
        {
            Debug.LogWarning("PlayerHealthBarUI: Health bar fill image is not assigned!");
            return;
        }

        float healthPercent = Mathf.Clamp01(currentHealth / maxHealth);
        targetFillAmount = healthPercent;

        if (!useSmoothAnimation)
        {
            currentFillAmount = targetFillAmount;
            healthBarFill.fillAmount = currentFillAmount;
        }

        UpdateHealthColor(healthPercent);
    }

    /// <summary>
    /// Update the color of the health bar based on health percentage
    /// </summary>
    private void UpdateHealthColor(float healthPercent)
    {
        if (healthBarFill == null) return;

        // If using native sprite colors, keep it white to show the sprite's original color
        if (useNativeSpriteColors)
        {
            healthBarFill.color = Color.white;
            return;
        }

        // Otherwise, apply color tinting based on health percentage
        Color newColor;
        
        if (healthPercent <= lowHealthThreshold)
        {
            // Low health - use low health color
            newColor = lowHealthColor;
        }
        else if (healthPercent <= mediumHealthThreshold)
        {
            // Medium health - lerp between low and medium color
            float t = (healthPercent - lowHealthThreshold) / (mediumHealthThreshold - lowHealthThreshold);
            newColor = Color.Lerp(lowHealthColor, mediumHealthColor, t);
        }
        else
        {
            // High health - lerp between medium and healthy color
            float t = (healthPercent - mediumHealthThreshold) / (1f - mediumHealthThreshold);
            newColor = Color.Lerp(mediumHealthColor, healthyColor, t);
        }

        healthBarFill.color = newColor;
    }

    /// <summary>
    /// Set health immediately without animation
    /// </summary>
    public void SetHealthImmediate(float currentHealth, float maxHealth)
    {
        if (healthBarFill == null) return;

        float healthPercent = Mathf.Clamp01(currentHealth / maxHealth);
        currentFillAmount = healthPercent;
        targetFillAmount = healthPercent;
        healthBarFill.fillAmount = healthPercent;
        UpdateHealthColor(healthPercent);
    }

    /// <summary>
    /// Show the health bar
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide the health bar
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
