using UnityEngine;
using UnityEngine.UI;

public class DatepickerAnni : MonoBehaviour
{
    public Dropdown dayDropdown;
    public Dropdown monthDropdown;
    public Dropdown yearDropdown;
    public Text selectedDateText;

    public GameObject calenderPop;

    public createProfile createprofile;
    void Start()
    {
        PopulateDropdowns();
        SetDefaultToToday();  // Set the default values to today's date
        UpdateDateText();
    }

    public void clickCalender()
    {
        calenderPop.SetActive(true);
    }
    public void closeCalender()
    {
        calenderPop.SetActive(false);
    }

    void PopulateDropdowns()
    {
        // Populate day dropdown (1 to 31)
        dayDropdown.options.Clear();
        for (int i = 1; i <= 31; i++)
        {
            dayDropdown.options.Add(new Dropdown.OptionData(i.ToString()));
        }

        // Populate month dropdown (1 to 12)
        monthDropdown.options.Clear();
        for (int i = 1; i <= 12; i++)
        {
            //monthDropdown.options.Add(new Dropdown.OptionData(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i)));
            monthDropdown.options.Add(new Dropdown.OptionData(i.ToString()));
        }

        // Populate year dropdown (e.g., 1900 to current year)
        yearDropdown.options.Clear();
        for (int i = 1940; i <= System.DateTime.Now.Year; i++)
        {
            yearDropdown.options.Add(new Dropdown.OptionData(i.ToString()));
        }

        // Set listeners for when the user selects a new value
        dayDropdown.onValueChanged.AddListener(delegate { UpdateDateText(); });
        monthDropdown.onValueChanged.AddListener(delegate { UpdateDateText(); });
        yearDropdown.onValueChanged.AddListener(delegate { UpdateDateText(); });
    }

    void SetDefaultToToday()
    {
        // Get today's date
        System.DateTime today = System.DateTime.Now;

        // Set the dropdowns to today's day, month, and year
        dayDropdown.value = today.Day - 1;  // Dropdown values start from 0
        monthDropdown.value = today.Month - 1;  // Dropdown values start from 0
        yearDropdown.value = yearDropdown.options.FindIndex(option => option.text == today.Year.ToString());
    }

    void UpdateDateText()
    {
        // Construct the date from the dropdowns
        int day = dayDropdown.value + 1;
        int month = monthDropdown.value + 1;
        int year = int.Parse(yearDropdown.options[yearDropdown.value].text);

        // Display selected date
        selectedDateText.text = new System.DateTime(year, month, day).ToString("yyyy-MM-dd");
       
        createprofile.anni_Text = selectedDateText.text;
        createprofile.aniDate = new System.DateTime(year, month, day);
    }
}

