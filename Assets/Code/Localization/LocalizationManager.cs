using System.Collections.Generic;
using CustomUtilities;
using UnityEngine;

public class LocalizationManager : MonoBehaviour, IDataPersistence
{
    public static LocalizationManager Instance;

    [SerializeField] private TextAsset localizationFile;

    private Dictionary<string, string> translations = new Dictionary<string, string>();

    private void Awake()
    {
        Instance = this;
    }

    public void LoadData(GameData data)
    {
        if (data.Language == null)
        {
            Debug.LogError("Localization file is null or empty!");
            localizationFile = DataPersistenceManager.Instance.LocalizationFile;
            LoadLanguage(localizationFile);
            return;
        }

        localizationFile = data.Language;
        LoadLanguage(localizationFile);
    }

    public void SaveData(ref GameData data)
    {
        data.Language = localizationFile; 
    }
    
    public void LoadLanguage(TextAsset languageFile)
    {
        string data = languageFile.text;
        string[] lines = data.Split(new char[] { '\n' });
        translations.Clear();
        foreach (string line in lines)
        {
            int firstCommaIndex = line.IndexOf(',');
            if (firstCommaIndex >= 0 && firstCommaIndex < line.Length - 1)
            {
                string key = line.Substring(0, firstCommaIndex).Trim().Trim('"'); // Remove spaces and quotes.
                string translation = line.Substring(firstCommaIndex + 1).Trim().Trim('"'); // Remove spaces and quotes.
                translations[key] = translation;
            }
        }
    }

    public string GetTranslation(string key)
    {
        if (translations.TryGetValue(key, out string translation))
        {
            return translation;
        }
        else
        {
            if (localizationFile != null)
            {
                LoadLanguage(localizationFile);

                if (translations.TryGetValue(key, out translation))
                {
                    return translation;
                }
            }

            Debug.LogError("No translation found for key: " + key);
            return key;
        }
    }
}

public static class LocalizationKeysHolder
{
    public static string GetEventTitleKey(Chapter chapter, int currentEventID)
    {
        string chapterTitle = StringUtility.ReplaceSpaces(chapter.name);
        string key = $"chapter_{chapterTitle}_event_{currentEventID}_title";
        return key;
    }

    public static string GetDecisionTitleKey(Chapter chapter, int eventID, int decisionIndex)
    {
        string chapterTitle = StringUtility.ReplaceSpaces(chapter.name);
        string key = $"chapter_{chapterTitle}_event_{eventID}_decision_{decisionIndex}_title";
        return key;
    }
    
    public static string GetDecisionDescriptionKey(Chapter chapter, int eventID, int decisionIndex)
    {
        string chapterTitle = StringUtility.ReplaceSpaces(chapter.name);
        string key = $"chapter_{chapterTitle}_event_{eventID}_decision_{decisionIndex}_desc";
        return key;
    }
}