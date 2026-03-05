using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Editor menu for easy player health bar UI setup
/// </summary>
public class PlayerHealthBarSetupMenu
{
    [MenuItem("GameObject/UI/Player Health Bar", false, 11)]
    public static void CreatePlayerHealthBar()
    {
        // Check if a Canvas exists, if not create one
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            
            Debug.Log("✓ Created Canvas for UI");
        }

        // Check if player health bar already exists
        PlayerHealthBarUI existingHealthBar = GameObject.FindObjectOfType<PlayerHealthBarUI>();
        if (existingHealthBar != null)
        {
            Debug.LogWarning("Player Health Bar UI already exists in the scene!");
            Selection.activeGameObject = existingHealthBar.gameObject;
            return;
        }

        // Create player health bar container
        GameObject healthBarContainer = new GameObject("PlayerHealthBar");
        healthBarContainer.transform.SetParent(canvas.transform, false);
        
        RectTransform containerRect = healthBarContainer.AddComponent<RectTransform>();
        
        // Position at top-left corner with some padding
        containerRect.anchorMin = new Vector2(0f, 1f); // Top-left
        containerRect.anchorMax = new Vector2(0f, 1f);
        containerRect.pivot = new Vector2(0f, 1f);
        containerRect.anchoredPosition = new Vector2(20f, -20f); // 20px padding from edges
        containerRect.sizeDelta = new Vector2(200f, 30f); // Default size

        // Create background image
        GameObject background = new GameObject("Background");
        background.transform.SetParent(healthBarContainer.transform, false);
        RectTransform bgRect = background.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        bgRect.anchoredPosition = Vector2.zero;
        
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f); // Dark background
        bgImage.sprite = CreateWhiteSprite();

        // Create fill image
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(healthBarContainer.transform, false);
        RectTransform fillRect = fill.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.sizeDelta = new Vector2(-4f, -4f); // 2px inset on all sides
        fillRect.anchoredPosition = Vector2.zero;
        
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = Color.white; // White to show sprite's native color
        fillImage.sprite = CreateWhiteSprite();
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        fillImage.fillAmount = 1f;

        // Create border/box image
        GameObject box = new GameObject("Box");
        box.transform.SetParent(healthBarContainer.transform, false);
        RectTransform boxRect = box.AddComponent<RectTransform>();
        boxRect.anchorMin = Vector2.zero;
        boxRect.anchorMax = Vector2.one;
        boxRect.sizeDelta = Vector2.zero;
        boxRect.anchoredPosition = Vector2.zero;
        
        Image boxImage = box.AddComponent<Image>();
        boxImage.color = Color.white; // White to show sprite's native color
        boxImage.sprite = CreateWhiteSprite();
        boxImage.type = Image.Type.Sliced;
        
        // Add the PlayerHealthBarUI component
        PlayerHealthBarUI healthBarUI = healthBarContainer.AddComponent<PlayerHealthBarUI>();
        healthBarUI.healthBarBackground = bgImage;
        healthBarUI.healthBarFill = fillImage;
        healthBarUI.healthBarBox = boxImage;
        healthBarUI.useNativeSpriteColors = true; // Enable by default to prevent black bar issue

        // Select the created object
        Selection.activeGameObject = healthBarContainer;

        Debug.Log("════════════════════════════════════════════════");
        Debug.Log("✓ Player Health Bar UI created successfully!");
        Debug.Log("✓ Position: Top-left corner of screen");
        Debug.Log("✓ 'Use Native Sprite Colors' is enabled by default");
        Debug.Log("✓ Configure appearance in Inspector:");
        Debug.Log("  - Assign your custom sprites to the Background, Fill, and Box Image components");
        Debug.Log("  - Each child object has an Image component - set the 'Source Image' field");
        Debug.Log("  - Adjust colors, animation speed, and health thresholds as needed");
        Debug.Log("  - Resize the health bar by adjusting the RectTransform");
        Debug.Log("════════════════════════════════════════════════");
    }

    [MenuItem("Tools/Player Health Bar/Setup Player Health Bar UI")]
    public static void SetupFromMenu()
    {
        CreatePlayerHealthBar();
    }

    [MenuItem("Tools/Player Health Bar/Select Player Health Bar")]
    public static void SelectPlayerHealthBar()
    {
        PlayerHealthBarUI healthBar = GameObject.FindObjectOfType<PlayerHealthBarUI>();
        if (healthBar != null)
        {
            Selection.activeGameObject = healthBar.gameObject;
            Debug.Log("Selected Player Health Bar UI");
        }
        else
        {
            Debug.LogWarning("No Player Health Bar UI found in scene. Create one first!");
        }
    }

    [MenuItem("Tools/Player Health Bar/Select Player Health Bar", true)]
    public static bool SelectPlayerHealthBarValidation()
    {
        return GameObject.FindObjectOfType<PlayerHealthBarUI>() != null;
    }

    /// <summary>
    /// Create a simple white sprite for UI elements
    /// </summary>
    private static Sprite CreateWhiteSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100f);
        return sprite;
    }
}
