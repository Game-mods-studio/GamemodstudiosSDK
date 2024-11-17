using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    public bool showDebugMenu = false;
    private float deltaTime = 0.0f;

    // FPS and Memory Usage tracking
    private const int graphResolution = 100;
    private float[] fpsHistory = new float[graphResolution];
    private float[] memoryHistory = new float[graphResolution];

    // Graph settings
    private GameObject fpsGraphObject;
    private GameObject memoryGraphObject;
    private LineRenderer fpsLineRenderer;
    private LineRenderer memoryLineRenderer;

    void Start()
    {
        // Create LineRenderers for FPS and Memory graphs
        fpsGraphObject = new GameObject("FPS Graph");
        memoryGraphObject = new GameObject("Memory Graph");

        fpsLineRenderer = fpsGraphObject.AddComponent<LineRenderer>();
        memoryLineRenderer = memoryGraphObject.AddComponent<LineRenderer>();

        InitializeLineRenderer(fpsLineRenderer, Color.green, new Vector3(-5, 3, 0));
        InitializeLineRenderer(memoryLineRenderer, Color.blue, new Vector3(-5, 1, 0));

        // Disable graphs by default
        fpsGraphObject.SetActive(false);
        memoryGraphObject.SetActive(false);
    }

    void Update()
    {
        // Toggle debug menu visibility
        if (Input.GetKeyDown(KeyCode.F3))
        {
            showDebugMenu = !showDebugMenu;

            // Toggle graphs visibility
            fpsGraphObject.SetActive(showDebugMenu);
            memoryGraphObject.SetActive(showDebugMenu);
        }

        // Calculate FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Record FPS and Memory Usage
        AddToHistory(fpsHistory, fps);
        AddToHistory(memoryHistory, System.GC.GetTotalMemory(false) / (1024.0f * 1024.0f)); // Memory in MB

        // Update graphs if they are active
        if (showDebugMenu)
        {
            UpdateGraph(fpsLineRenderer, fpsHistory, 60); // Assume FPS max range is 60
            UpdateGraph(memoryLineRenderer, memoryHistory, 500); // Assume max memory range is 500 MB
        }
    }

    private void InitializeLineRenderer(LineRenderer lineRenderer, Color color, Vector3 position)
    {
        lineRenderer.positionCount = graphResolution;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.transform.position = position;
    }

    private void AddToHistory(float[] history, float value)
    {
        for (int i = 1; i < history.Length; i++)
        {
            history[i - 1] = history[i];
        }
        history[history.Length - 1] = value;
    }

    private void UpdateGraph(LineRenderer lineRenderer, float[] history, float maxRange)
    {
        for (int i = 0; i < history.Length; i++)
        {
            float x = (float)i / (history.Length - 1) * 10.0f; // Spread over a 10-unit width
            float y = Mathf.Clamp(history[i] / maxRange, 0, 1) * 2.0f; // Scale to a 2-unit height
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    void OnGUI()
    {
        if (!showDebugMenu) return;

        int width = Screen.width, height = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(10, 10, width, height * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 2 / 100;
        style.normal.textColor = Color.white;

        // FPS and Frame Time
        float fps = 1.0f / deltaTime;
        float ms = deltaTime * 1000.0f;
        string fpsText = string.Format("FPS: {0:0.} ({1:0.0} ms/frame)", fps, ms);

        // System Info
        string systemInfo = $"OS: {SystemInfo.operatingSystem}, Unity: {Application.unityVersion}";
        string ramInfo = $"RAM: {SystemInfo.systemMemorySize} MB";
        string gpuInfo = $"GPU: {SystemInfo.graphicsDeviceName} (Shader Model {SystemInfo.graphicsShaderLevel}, API: {SystemInfo.graphicsDeviceType})";

        // Screen Info
        string screenInfo = $"Resolution: {Screen.width}x{Screen.height} @ {Screen.currentResolution.refreshRate} Hz, VSync: {(QualitySettings.vSyncCount > 0 ? "On" : "Off")}";

        string frameRate = $"Target Frame Rate: {Application.targetFrameRate}";

        // Player Info
        string playerInfo = "Player not found";
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            playerInfo = $"Player Position: X={pos.x:F2}, Y={pos.y:F2}, Z={pos.z:F2}\nPlayer Rotation: {player.transform.rotation.eulerAngles}";
        }

        // Scene Info
        string sceneInfo = $"Scene: {SceneManager.GetActiveScene().name}";
        string objectCount = $"GameObjects: {FindObjectsOfType<GameObject>().Length}";

        // Input Info
        string mousePos = $"Mouse Position: {Input.mousePosition}";
        string keysPressed = "Keys Pressed: ";
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(key))
                keysPressed += $"{key}, ";
        }
        keysPressed = keysPressed.TrimEnd(',', ' ');

        // Combine all debug info
        string debugText = $"{fpsText}\n{systemInfo}\n{ramInfo}\n{gpuInfo}\n{screenInfo}\n{frameRate}\n\n{playerInfo}\n{sceneInfo}\n{objectCount}\n\n{mousePos}\n{keysPressed}";

        // Display the debug information
        GUI.Label(rect, debugText, style);
    }
}
