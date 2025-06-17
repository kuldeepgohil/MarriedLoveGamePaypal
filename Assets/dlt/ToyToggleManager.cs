using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToyToggleManager : MonoBehaviour
{
    public Toggle togglePrefab;
    public Transform toggleParent;
    private Dictionary<string, Toggle> toggles = new Dictionary<string, Toggle>(); // Use string keys for UUIDs
    public List<string> selectedToyIDs = new List<string>(); // List to store selected toy IDs

    void Start()
    {
        List<Toy> toys = FetchToysFromAPI();
        CreateToggles(toys);
    }

    private List<Toy> FetchToysFromAPI()
    {
        return new List<Toy> {
            new Toy { id = "6712476fa4e8d5f01026fd44", name = "Toy1" },
            new Toy { id = "671248e3a4e8d5f01026fd73", name = "Toy2" },
            new Toy { id = "671248e3a4e8d5dddf01026fd73", name = "Toy3" },
            new Toy { id = "671248e3a4e8d5f01fff026fd73", name = "Toy4" },
            new Toy { id = "671248e3a4e8d5f010ffs26fd73", name = "Toy5" },
            // Add more toys
        };
    }

    private void CreateToggles(List<Toy> toys)
    {
        foreach (var toy in toys)
        {
            Toggle toggleInstance = Instantiate(togglePrefab, toggleParent);
            toggleInstance.GetComponentInChildren<Text>().text = toy.name;

            string toyID = toy.id;
            toggleInstance.onValueChanged.AddListener((isSelected) =>
            {
                if (isSelected)
                    selectedToyIDs.Add(toyID); // Add ID when selected
                else
                    selectedToyIDs.Remove(toyID); // Remove ID when deselected

                SendSelectedIDsToAPI();
            });

            // Use string ID directly as key in the dictionary
            toggles.Add(toyID, toggleInstance);
        }
    }

    private void SendSelectedIDsToAPI()
    {
        // Build the formatted string manually
        string formattedIDs = "[" + string.Join(",", selectedToyIDs.ConvertAll(id => $"'{id}'")) + "]";

        Debug.Log("Selected Toy IDs sent to API: " + formattedIDs);

        // Replace with actual API call
        // Example: APIClient.Post("your_endpoint_url", formattedIDs);
    }
}

[System.Serializable]
public class Toy
{
    public string id;
    public string name;
}
