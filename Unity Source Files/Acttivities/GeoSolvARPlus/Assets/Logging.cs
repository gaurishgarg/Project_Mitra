
using UnityEngine;
using System;
using System.IO;

public class Logging : MonoBehaviour
{
    // Public variables controlled by other scripts
    public static string ScreenType = "Menu";
    public static string ActivitySet = "";
    public static Transform FocusTransform;
    public static bool ActivityCorrectEnd = false;
    public static string Status = "Not Connected";
    public static string AnswerMarked = "NA";

    [Header("Logging Settings")]
    [SerializeField] private float logInterval = 1f;

    private string logFilePath;
    private float nextLogTime;
    private Camera mainCamera;
    private bool sessionActive;

    private void Awake()
    {
        mainCamera = Camera.main;

        string logDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "SpatialReasoning",
            "Logs"
        );

        Directory.CreateDirectory(logDirectory);

        string fileName = "Session_" +
            DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

        logFilePath = Path.Combine(logDirectory, fileName);

        CreateCSV();

        sessionActive = true;
        nextLogTime = Time.unscaledTime;
    }

    private void CreateCSV()
    {
        string header =
            "Timestamp,ScreenType,ActivitySet,ObjectInFocus,Status," +
            "GlobalPosition,GlobalRotation,GlobalScale," +
            "CameraPosition,CameraRotation,ActivityCorrectEnd,AnswerMarked";

        File.WriteAllText(logFilePath, header + Environment.NewLine);
    }

    private void Update()
    {
        if (!sessionActive || Time.unscaledTime < nextLogTime)
            return;

        nextLogTime = Time.unscaledTime + logInterval;
        WriteLog();
    }

    private void WriteLog()
    {
        // Automatically retrieve focused object information
        string objectName = "";
        Vector3 position = Vector3.zero;
        Vector3 rotation = Vector3.zero;
        Vector3 scale = Vector3.zero;

        if (FocusTransform != null)
        {
            objectName = FocusTransform.name;
            position = FocusTransform.position;
            rotation = FocusTransform.eulerAngles;
            scale = FocusTransform.lossyScale;
        }

        // Automatically retrieve camera information
        Vector3 cameraPosition = Vector3.zero;
        Vector3 cameraRotation = Vector3.zero;

        if (mainCamera != null)
        {
            cameraPosition = mainCamera.transform.position;
            cameraRotation = mainCamera.transform.eulerAngles;
        }

        string timestamp =
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        string row = string.Join(",",
            EscapeCSV(timestamp),
            EscapeCSV(ScreenType),
            EscapeCSV(ActivitySet),
            EscapeCSV(objectName),
            EscapeCSV(Status),
            FormatVector(position),
            FormatVector(rotation),
            FormatVector(scale),
            FormatVector(cameraPosition),
            FormatVector(cameraRotation),
            ActivityCorrectEnd.ToString(),
            EscapeCSV(AnswerMarked)
        );

        File.AppendAllText(logFilePath, row + Environment.NewLine);
    }

    private string FormatVector(Vector3 v)
    {
        return $"\"{v.x:F2};{v.y:F2};{v.z:F2}\"";
    }

    private string EscapeCSV(string value)
    {
        return "\"" + (value ?? "").Replace("\"", "\"\"") + "\"";
    }

    public string GetLogPath()
    {
        return logFilePath;
    }

    private void OnApplicationQuit()
    {
        sessionActive = false;
    }
}
