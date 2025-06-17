using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    // Reference to the Slider UI
    public Slider loadingSlider;

    // Index of the scene to load
    public int sceneIndex;

    private void Start()
    {
        // Start the fake loading process
        StartCoroutine(gameLoading());
    }

    // Coroutine for fake loading
    IEnumerator gameLoading()
    {
        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false; // Prevent the scene from activating immediately

        float progress = 0f;

        // Simulate loading until we reach 0.97
        while (progress < 0.97f)
        {
            // Increment progress (adjust the speed here)
            progress += Time.deltaTime * 0.5f; // Increased increment speed
            loadingSlider.value = progress;

            // Optionally, you can output progress as a percentage
            //Debug.Log("Loading progress: " + (progress * 100) + "%");

            // Wait a short time to simulate loading time
            yield return null; // Wait for the next frame
        }

        // Ensure the slider reaches 0.97
        loadingSlider.value = 0.97f;

        // Simulate a small delay before activating the scene
        yield return new WaitForSeconds(0.3f); // Reduced wait time before activating the scene

        // Allow the scene to activate
        operation.allowSceneActivation = true;
    }
}
