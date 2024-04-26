using UnityEngine;
using TMPro;
using System;
using System.Globalization;

public class DatePicker : MonoBehaviour
{
    public TMP_Dropdown dayDropdown;
    public TMP_Dropdown monthDropdown;
    public TMP_Dropdown yearDropdown;

    private void Start()
    {
        for (int year = DateTime.Now.Year; year >= 1970; year--)
        {
            yearDropdown.options.Add(new TMP_Dropdown.OptionData(year.ToString()));
        }
        yearDropdown.value = 0;
        yearDropdown.RefreshShownValue();
    
        string[] monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames;
        for (int i = 0; i < monthNames.Length - 1; i++) // -1 to exclude empty string at the end
        {
            monthDropdown.options.Add(new TMP_Dropdown.OptionData(monthNames[i]));
        }
        monthDropdown.value = DateTime.Now.Month - 1;
        monthDropdown.RefreshShownValue();
        
        UpdateDaysDropdown(DateTime.Now.Month, DateTime.Now.Year);
    }

    public void OnMonthOrYearValueChanged()
    {
        int selectedMonthIndex = monthDropdown.value + 1; // Month index starts from 1
        int selectedYear = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int selectedDay = dayDropdown.value + 1; // Day index starts from 1
        int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonthIndex);
    
        // Reset day dropdown if the selected day doesn't exist in the new month
        if (selectedDay > daysInMonth)
        {
            selectedDay = daysInMonth;
        }

        UpdateDaysDropdown(selectedMonthIndex, selectedYear);
        dayDropdown.value = selectedDay - 1;
        dayDropdown.RefreshShownValue();
    }

    private void UpdateDaysDropdown(int month, int year)
    {
        int daysInMonth = DateTime.DaysInMonth(year, month);
        dayDropdown.ClearOptions();
        for (int day = 1; day <= daysInMonth; day++)
        {
            dayDropdown.options.Add(new TMP_Dropdown.OptionData(day.ToString()));
        }
        dayDropdown.RefreshShownValue();
    }
    
    public string GetDate()
    {
        int selectedMonthIndex = monthDropdown.value + 1; // Month index starts from 1
        int selectedYear = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int selectedDay = dayDropdown.value + 1; // Day index starts from 1

        return string.Format("{0:D2}/{1:D2}/{2}", selectedMonthIndex, selectedDay, selectedYear);
    }

}