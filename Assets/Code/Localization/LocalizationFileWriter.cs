using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using CustomUtilities;

public class LocalizationFileWriter : MonoBehaviour
{
    [SerializeField] private string fileName = "English.csv";
    [SerializeField] private List<Chapter> chapters;

    private StringBuilder _csvFile;

    [ContextMenu("WriteEventsToCSV")]
    public void WriteEventsToCSV()
    {
        _csvFile = new StringBuilder();

        foreach (Chapter chapter in chapters)
        {
            foreach (GameEvent gameEvent in chapter.events)
            {
                WriteEvent(chapter, gameEvent);
                _csvFile.AppendLine(); // Add a space to visually separate events
            }
        }

        string path = Path.Combine(Application.dataPath, fileName);
        File.WriteAllText(path, _csvFile.ToString());

        Debug.Log("File created successfully in " + path);
    }

    private void WriteEvent(Chapter chapter, GameEvent gameEvent)
    {
        // Write event title
        WriteEventTitle(chapter, gameEvent);

        // Write decisions for the event
        WriteEventDecisions(chapter, gameEvent);
    }

    private void WriteEventTitle(Chapter chapter, GameEvent gameEvent)
    {
        string chapterTitle = StringUtility.ReplaceSpaces(chapter.name);
        int eventID = gameEvent.GetEventID();
        string key = $"chapter_{chapterTitle}_event_{eventID}_title";
        string translation = gameEvent.Title;

        WriteToFile(key, translation);
    }

    private void WriteEventDecisions(Chapter chapter, GameEvent gameEvent)
    {
        string chapterTitle = StringUtility.ReplaceSpaces(chapter.name);
        int eventID = gameEvent.GetEventID();

        for (int i = 0; i < gameEvent.Decisions.Length; i++)
        {
            Decision decision = gameEvent.Decisions[i];

            string titleKey = $"chapter_{chapterTitle}_event_{eventID}_decision_{i}_title";
            string descriptionKey = $"chapter_{chapterTitle}_event_{eventID}_decision_{i}_desc";

            string titleTranslation = decision.Title;
            string descriptionTranslation = decision.Description;

            WriteToFile(titleKey, titleTranslation);
            WriteToFile(descriptionKey, descriptionTranslation);
        }
    }

    private void WriteToFile(string key, string translation)
    {
        if (_csvFile == null)
        {
            Debug.LogError("CSV file variable is null!");
            return;
        }

        _csvFile.AppendLine("\"" + key + "\",\"" + translation + "\"");
    }
}