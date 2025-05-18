using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Type the EXACT name of the scene you want to load")]
    public string targetSceneName = "Level1"; // Default value, editable in Inspector

    [Header("Button Settings")]
    [Tooltip("Drag your UI Button here")]
    public Button restartButton;

    void Start()
    {
        // Auto-get the Button component if not assigned
        if (restartButton == null)
        {
            restartButton = GetComponent<Button>();
        }

        // Setup button click action
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(LoadTargetScene);
        }
        else
        {
            Debug.LogWarning("RestartButton: No button component found!");
        }
    }

    public void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("RestartButton: No target scene name specified!");
        }
    }
}