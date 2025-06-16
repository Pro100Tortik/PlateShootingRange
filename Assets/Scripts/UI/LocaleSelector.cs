using UnityEngine.Localization.Settings;
using System.Collections;
using UnityEngine;
using TMPro;

public class LocaleSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    private bool _changing = false;

    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        var currentLocale = LocalizationSettings.SelectedLocale;
        var locales = LocalizationSettings.AvailableLocales.Locales;
        dropdown.SetValueWithoutNotify(locales.IndexOf(currentLocale));
    }

    public void ChangeLocale(int index)
    {
        if (_changing == true)
            return;

        StartCoroutine(SetLocale(index));
    }

    private IEnumerator SetLocale(int index)
    {
        _changing = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        _changing = false;
    }
}
