using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Custom inspector for PlayerHealthBarUI to provide better user experience
/// Shows warnings when sprites aren't assigned and provides helper buttons
/// </summary>
[CustomEditor(typeof(PlayerHealthBarUI))]
public class PlayerHealthBarUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerHealthBarUI healthBar = (PlayerHealthBarUI)target;

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Player Health Bar UI - Top Left Corner\n\nDrag your custom sprites below. Make sure your sprites have 'Texture Type' set to 'Sprite (2D and UI)' in import settings.", MessageType.Info);
        EditorGUILayout.Space();

        // Important color mode warning
        if (!healthBar.useNativeSpriteColors)
        {
            EditorGUILayout.HelpBox("⚠️ Color Tinting is enabled!\n\nIf your health bar appears BLACK or DARK:\n• Enable 'Use Native Sprite Colors' to show your sprite's original colors\n• OR use white/transparent sprites and let the color tinting handle the coloring", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.HelpBox("✓ Using native sprite colors - your sprites will show their original colors!", MessageType.Info);
        }
        EditorGUILayout.Space();

        // Draw default inspector
        DrawDefaultInspector();

        EditorGUILayout.Space();

        // Validation section
        bool hasIssues = false;
        
        if (healthBar.healthBarBackground == null)
        {
            EditorGUILayout.HelpBox("⚠️ Health Bar Background is not assigned!", MessageType.Warning);
            hasIssues = true;
        }
        
        if (healthBar.healthBarFill == null)
        {
            EditorGUILayout.HelpBox("⚠️ Health Bar Fill is not assigned!", MessageType.Warning);
            hasIssues = true;
        }
        
        if (healthBar.healthBarBox == null)
        {
            EditorGUILayout.HelpBox("⚠️ Health Bar Box is not assigned!", MessageType.Warning);
            hasIssues = true;
        }

        if (!hasIssues)
        {
            EditorGUILayout.HelpBox("✓ All UI references are assigned correctly!", MessageType.Info);
        }

        EditorGUILayout.Space();

        // Helper buttons
        EditorGUILayout.LabelField("Quick Actions", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Enable Native Sprite Colors (Fix Black Bar)"))
        {
            SerializedProperty nativeColorsProp = serializedObject.FindProperty("useNativeSpriteColors");
            nativeColorsProp.boolValue = true;
            serializedObject.ApplyModifiedProperties();
            
            // Also set all Image colors to white
            if (healthBar.healthBarBackground != null)
                healthBar.healthBarBackground.color = Color.white;
            if (healthBar.healthBarFill != null)
                healthBar.healthBarFill.color = Color.white;
            if (healthBar.healthBarBox != null)
                healthBar.healthBarBox.color = Color.white;
            
            Debug.Log("✓ Enabled native sprite colors - your sprites will now show their original colors!");
        }
        
        if (GUILayout.Button("Auto-Find UI Components"))
        {
            AutoFindComponents(healthBar);
        }

        if (GUILayout.Button("How to Fix Sprite Import Settings"))
        {
            ShowSpriteImportHelp();
        }

        if (GUILayout.Button("Test Health Bar (75% Health)"))
        {
            if (Application.isPlaying)
            {
                healthBar.UpdateHealth(75f, 100f);
                Debug.Log("Set health to 75%");
            }
            else
            {
                Debug.LogWarning("Enter Play Mode to test health bar!");
            }
        }

        if (GUILayout.Button("Test Health Bar (25% Health)"))
        {
            if (Application.isPlaying)
            {
                healthBar.UpdateHealth(25f, 100f);
                Debug.Log("Set health to 25%");
            }
            else
            {
                Debug.LogWarning("Enter Play Mode to test health bar!");
            }
        }
    }

    private void AutoFindComponents(PlayerHealthBarUI healthBar)
    {
        // Try to auto-find the Image components in children
        Transform transform = healthBar.transform;
        
        Transform bgTransform = transform.Find("Background");
        if (bgTransform != null && healthBar.healthBarBackground == null)
        {
            Image bgImage = bgTransform.GetComponent<Image>();
            if (bgImage != null)
            {
                SerializedProperty bgProp = serializedObject.FindProperty("healthBarBackground");
                bgProp.objectReferenceValue = bgImage;
                Debug.Log("✓ Found Background image");
            }
        }

        Transform fillTransform = transform.Find("Fill");
        if (fillTransform != null && healthBar.healthBarFill == null)
        {
            Image fillImage = fillTransform.GetComponent<Image>();
            if (fillImage != null)
            {
                SerializedProperty fillProp = serializedObject.FindProperty("healthBarFill");
                fillProp.objectReferenceValue = fillImage;
                Debug.Log("✓ Found Fill image");
            }
        }

        Transform boxTransform = transform.Find("Box");
        if (boxTransform != null && healthBar.healthBarBox == null)
        {
            Image boxImage = boxTransform.GetComponent<Image>();
            if (boxImage != null)
            {
                SerializedProperty boxProp = serializedObject.FindProperty("healthBarBox");
                boxProp.objectReferenceValue = boxImage;
                Debug.Log("✓ Found Box image");
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowSpriteImportHelp()
    {
        EditorUtility.DisplayDialog(
            "How to Fix Sprite Import Settings",
            "If you cannot drag sprites into the Image fields:\n\n" +
            "1. Select your sprite in the Project window\n" +
            "2. In the Inspector, find 'Texture Type'\n" +
            "3. Change it to 'Sprite (2D and UI)'\n" +
            "4. Click 'Apply'\n\n" +
            "Now you can drag the sprite into the Image component!\n\n" +
            "Alternative: You can also select the Image component's sprite field, " +
            "then click the circle icon to pick from a list of available sprites.",
            "Got it!"
        );
    }
}
