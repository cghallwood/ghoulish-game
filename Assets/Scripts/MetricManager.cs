using UnityEngine;
using System.Collections;
using System.IO;

/* This is a central class will track all of the metrics you want 
 * in your game. That might be things like number of deaths, number 
 * of times the player uses a particular mechanic, or the total 
 * time spent in a level. These are unique to your game and you'll
 * need to decide specifically what data you would like to collect.
 */

public class MetricManager : MonoBehaviour
{
    private int sessionNumber = 0;
    private int avgScore = 0;
    private int playerDeathsInThisSession = 0;
    private int playerEscapesInThisSession = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void TrackAverageScore(int score)
    {
        sessionNumber++;
        avgScore = (avgScore + score) / sessionNumber;
    }

    public void TrackPlayerEscapes()
    {
        playerEscapesInThisSession++;
    }

    // Public method that you'll call to add to playerDeathsInThisSession.
    public void TrackPlayerDeath()
    {
        playerDeathsInThisSession++;
    }

    /* Unity calls OnApplicationQuit right before your application 
     * actually exits.
     *      
     * You can use this to save information for the next time the 
     * game starts, or in our case write the metrics out to a file.
     */
    private void OnApplicationQuit()
    {
        WriteMetricsToFile();
    }

    // Generate the report that will be saved out to a file.
    private void WriteMetricsToFile()
    {
        string totalReport = "Report generated on " + System.DateTime.Now + "\n\n";
        totalReport += "Total Report:\n";
        totalReport += ConvertMetricsToStringRepresentation();

        //this makes the file legible on whatever system the game is running on
        totalReport = totalReport.Replace("\n", System.Environment.NewLine);

        string reportFile = CreateUniqueFileName();
        File.WriteAllText(reportFile, totalReport);
    }

    /* Utility method that converts all metrics tracked in this script 
     * to their string representation so they look correct when printing 
     * to a file.
     */
    private string ConvertMetricsToStringRepresentation()
    {
        string metrics = "Player metrics:\n";
        metrics += "Average Score: " + avgScore.ToString() + "\n";
        metrics += "Player Escapes: " + playerEscapesInThisSession.ToString() + "\n";
        metrics += "Player Deaths: " + playerDeathsInThisSession.ToString() + "\n";

        return metrics;
    }

    /* Uses the current date/time on this computer to create a uniquely 
     * named file, so you don't end up overwriting the same file over 
     * and over.
     */
    private string CreateUniqueFileName()
    {
        string dateTime = System.DateTime.Now.ToString();
        dateTime = dateTime.Replace("/", "_");
        dateTime = dateTime.Replace(":", "_");
        dateTime = dateTime.Replace(" ", "___");
        return "Ghoulish_metrics_" + dateTime + ".txt";
    }
}
