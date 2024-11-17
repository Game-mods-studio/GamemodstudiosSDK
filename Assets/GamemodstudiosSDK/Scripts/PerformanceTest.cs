using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class PerformanceTest : MonoBehaviour
{
    public Text resultsText; // Assign a UI Text element in the inspector to display results.
    private string results = "";
    private string filePath;

    private void Start()
    {
        // Define the file path
        filePath = Path.Combine(Application.persistentDataPath, "PerformanceTestResults.txt");
        StartCoroutine(RunTests());
    }

    private IEnumerator RunTests()
    {
        results = "Performance Test Results:\n";
        
        // Test 1: Rendering many objects
        yield return TestRenderingLoad();

        // Test 2: Physics simulation
        yield return TestPhysicsLoad();

        // Test 3: Particle system
        yield return TestParticleLoad();

        // Save results to file
        SaveResultsToFile();

        // Display results in the UI
        resultsText.text = results;
    }

    private IEnumerator TestRenderingLoad()
    {
        results += "Rendering Test: ";
        int objectCount = 1000;
        GameObject[] testObjects = new GameObject[objectCount];
        GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);

        for (int i = 0; i < objectCount; i++)
        {
            testObjects[i] = Instantiate(prefab, Random.insideUnitSphere * 10, Quaternion.identity);
        }

        yield return new WaitForSeconds(5); // Run the test for 5 seconds
        float fps = 1.0f / Time.deltaTime;

        results += $"FPS: {fps:F2}\n";

        foreach (GameObject obj in testObjects)
        {
            Destroy(obj);
        }
        Destroy(prefab);
    }

    private IEnumerator TestPhysicsLoad()
    {
        results += "Physics Test: ";
        int objectCount = 500;
        GameObject[] testObjects = new GameObject[objectCount];
        GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Rigidbody rb = prefab.AddComponent<Rigidbody>();

        for (int i = 0; i < objectCount; i++)
        {
            testObjects[i] = Instantiate(prefab, new Vector3(i % 10, i / 10, 0), Quaternion.identity);
        }

        yield return new WaitForSeconds(5); // Run the test for 5 seconds
        float fps = 1.0f / Time.deltaTime;

        results += $"FPS: {fps:F2}\n";

        foreach (GameObject obj in testObjects)
        {
            Destroy(obj);
        }
        Destroy(prefab);
    }

    private IEnumerator TestParticleLoad()
    {
        results += "Particle System Test: ";
        ParticleSystem ps = new GameObject("ParticleSystem").AddComponent<ParticleSystem>();
        var main = ps.main;
        main.maxParticles = 10000;

        ps.Play();
        yield return new WaitForSeconds(5); // Run the test for 5 seconds
        float fps = 1.0f / Time.deltaTime;

        results += $"FPS: {fps:F2}\n";

        Destroy(ps.gameObject);
    }

    private void SaveResultsToFile()
    {
        try
        {
            File.WriteAllText(filePath, results);
            Debug.Log($"Results saved to {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save results: {e.Message}");
        }
    }
}
