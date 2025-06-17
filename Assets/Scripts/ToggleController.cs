using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public Toggle toggle1; // Assign your first toggle in the Inspector
    public Toggle toggle2; // Assign your second toggle in the Inspector

    private void Start()
    {
        // Add listener to toggle1
        toggle1.onValueChanged.AddListener(delegate { OnToggleChanged(toggle1); });
        // Add listener to toggle2
        toggle2.onValueChanged.AddListener(delegate { OnToggleChanged(toggle2); });
        toggle1.isOn = true;
        toggle2.isOn = false;
    }

    private void OnToggleChanged(Toggle changedToggle)
    {

       

        // Perform the action only if the toggle is turned ON
        if (changedToggle.isOn)
        {
            // If toggle1 is turned on, turn off toggle2
            if (changedToggle == toggle1)
            {
                toggle2.isOn = false;
            }
            // If toggle2 is turned on, turn off toggle1
            else if (changedToggle == toggle2)
            {
                toggle1.isOn = false;
            }
        }
    }

    private void OnDestroy()
    {
        // Clean up listeners when the object is destroyed
        toggle1.onValueChanged.RemoveAllListeners();
        toggle2.onValueChanged.RemoveAllListeners();
    }
}
