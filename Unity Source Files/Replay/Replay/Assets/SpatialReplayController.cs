
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using TMPro;
using SFB;

public class SpatialReplayController : MonoBehaviour
{
    [Serializable]
    public class ReplayObject
    {
        public string csvObjectName;
        public Transform targetObject;

        [NonSerialized]
        public bool hasData;
    }

    [Header("UI")]
    [SerializeField] private TMP_Dropdown objectDropdown;

    [Header("Replay Objects")]
    [Tooltip("Order must match the manually populated TMP Dropdown.")]
    [SerializeField]
    private List<ReplayObject> replayObjects =
        new List<ReplayObject>();

    [Header("Replay Camera")]
    [Tooltip("Assign the actual camera Transform or recorded camera rig.")]
    [SerializeField] private Transform replayCamera;

    [Header("Playback")]
    [SerializeField] private float playbackSpeed = 1f;

    private class ReplayFrame
    {
        public double time;

        // Selected object transforms
        public Vector3 position;
        public Quaternion rotation;

        // Camera transforms
        public Vector3 cameraPosition;
        public Quaternion cameraRotation;
    }

    // Original data retained separately for every object.
    private readonly Dictionary<string, List<ReplayFrame>> replayData =
        new Dictionary<string, List<ReplayFrame>>();

    // Currently selected object's frames.
    private List<ReplayFrame> frames =
        new List<ReplayFrame>();

    private string loadedFilePath;

    private bool isPlaying;
    private bool hasLoadedData;

    private double playbackTime;
    private int currentFrameIndex;

    private ReplayObject selectedObject;

    // ==================================================
    // INITIALIZATION
    // ==================================================

    private void Start()
    {
        if (objectDropdown == null)
            Debug.LogError("TMP Dropdown is not assigned.");

        if (replayCamera == null)
            Debug.LogError("Replay Camera is not assigned.");

        if (objectDropdown != null)
        {
            objectDropdown.onValueChanged.AddListener(
                OnObjectDropdownChanged
            );
        }

        SetOnlySelectedObjectActive(null);

        Debug.Log("Browse and select a CSV session.");
    }


    public void QuitApp()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (objectDropdown != null)
        {
            objectDropdown.onValueChanged.RemoveListener(
                OnObjectDropdownChanged
            );
        }
    }

    // ==================================================
    // OBJECT VISIBILITY
    // ==================================================

    private void SetOnlySelectedObjectActive(ReplayObject selected)
    {
        foreach (ReplayObject obj in replayObjects)
        {
            if (obj == null || obj.targetObject == null)
                continue;

            obj.targetObject.gameObject.SetActive(obj == selected);
        }
    }

    // ==================================================
    // FILE BROWSER
    // ==================================================

    public void BrowseCSV()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel(
            "Select Replay CSV",
            "",
            "csv",
            false
        );

        if (paths == null ||
            paths.Length == 0 ||
            string.IsNullOrEmpty(paths[0]))
        {
            return;
        }

        LoadCSV(paths[0]);
    }

    // ==================================================
    // LOAD CSV
    // ==================================================

    private void LoadCSV(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("CSV file not found.");
            return;
        }

        StopReplay();

        hasLoadedData = false;
        loadedFilePath = null;

        // Create a new list rather than clearing a list
        // that may reference data inside replayData.
        frames = new List<ReplayFrame>();

        replayData.Clear();

        selectedObject = null;
        SetOnlySelectedObjectActive(null);

        foreach (ReplayObject obj in replayObjects)
        {
            if (obj != null)
                obj.hasData = false;
        }

        try
        {
            string[] lines = File.ReadAllLines(path);

            if (lines.Length < 2)
            {
                Debug.LogWarning("CSV contains no records.");
                return;
            }

            string[] headers = ParseCSVLine(lines[0]);

            Dictionary<string, int> columns =
                new Dictionary<string, int>(
                    StringComparer.OrdinalIgnoreCase
                );

            for (int i = 0; i < headers.Length; i++)
            {
                columns[headers[i].Trim()] = i;
            }

            // Required object and timestamp columns.
            string[] requiredColumns =
            {
                "Timestamp",
                "ObjectInFocus",
                "GlobalPosition",
                "GlobalRotation"
            };

            foreach (string column in requiredColumns)
            {
                if (!columns.ContainsKey(column))
                {
                    Debug.LogError(
                        "Missing CSV column: " + column
                    );
                    return;
                }
            }

            // Support both camera header naming conventions.
            string cameraPositionColumn =
                columns.ContainsKey("Camera_Position")
                ? "Camera_Position"
                : "CameraPosition";

            string cameraRotationColumn =
                columns.ContainsKey("Camera_Rotation")
                ? "Camera_Rotation"
                : "CameraRotation";

            if (!columns.ContainsKey(cameraPositionColumn))
            {
                Debug.LogError(
                    "Missing camera position column. " +
                    "Expected Camera_Position or CameraPosition."
                );
                return;
            }

            if (!columns.ContainsKey(cameraRotationColumn))
            {
                Debug.LogError(
                    "Missing camera rotation column. " +
                    "Expected Camera_Rotation or CameraRotation."
                );
                return;
            }

            // Initialize collections for configured objects.
            foreach (ReplayObject obj in replayObjects)
            {
                if (obj == null ||
                    string.IsNullOrWhiteSpace(obj.csvObjectName))
                    continue;

                if (!replayData.ContainsKey(obj.csvObjectName))
                {
                    replayData.Add(
                        obj.csvObjectName,
                        new List<ReplayFrame>()
                    );
                }
            }

            int loadedRows = 0;

            // Parse CSV records.
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                string[] values = ParseCSVLine(lines[i]);

                if (values.Length < headers.Length)
                    continue;

                string objectName = GetValue(
                    values,
                    columns,
                    "ObjectInFocus"
                );

                // Ignore objects not configured in Inspector.
                if (!replayData.ContainsKey(objectName))
                    continue;

                try
                {
                    ReplayFrame frame = new ReplayFrame();

                    // Timestamp
                    DateTime timestamp = DateTime.Parse(
                        GetValue(
                            values,
                            columns,
                            "Timestamp"
                        ),
                        CultureInfo.InvariantCulture
                    );

                    frame.time = timestamp.Ticks /
                        (double)TimeSpan.TicksPerSecond;

                    // Object position
                    frame.position = ParseVector3(
                        GetValue(
                            values,
                            columns,
                            "GlobalPosition"
                        )
                    );

                    // Object rotation
                    frame.rotation = Quaternion.Euler(
                        ParseVector3(
                            GetValue(
                                values,
                                columns,
                                "GlobalRotation"
                            )
                        )
                    );

                    // Camera position
                    frame.cameraPosition = ParseVector3(
                        GetValue(
                            values,
                            columns,
                            cameraPositionColumn
                        )
                    );

                    // Camera rotation
                    frame.cameraRotation = Quaternion.Euler(
                        ParseVector3(
                            GetValue(
                                values,
                                columns,
                                cameraRotationColumn
                            )
                        )
                    );

                    replayData[objectName].Add(frame);

                    loadedRows++;
                }
                catch (Exception e)
                {
                    Debug.LogWarning(
                        "Skipping CSV row " + (i + 1) +
                        ": " + e.Message
                    );
                }
            }

            // Sort frames and determine availability.
            foreach (ReplayObject obj in replayObjects)
            {
                if (obj == null ||
                    string.IsNullOrWhiteSpace(obj.csvObjectName))
                    continue;

                if (!replayData.ContainsKey(obj.csvObjectName))
                    continue;

                List<ReplayFrame> objectFrames =
                    replayData[obj.csvObjectName];

                objectFrames.Sort(
                    (a, b) => a.time.CompareTo(b.time)
                );

                obj.hasData = objectFrames.Count > 0;

                Debug.Log(
                    obj.csvObjectName + ": " +
                    objectFrames.Count + " frames loaded."
                );
            }

            loadedFilePath = path;
            hasLoadedData = true;

            Debug.Log(
                "Loaded: " + Path.GetFileName(path) +
                " | " + loadedRows + " records."
            );

            // Select currently selected dropdown option.
            if (objectDropdown != null)
            {
                OnObjectDropdownChanged(objectDropdown.value);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("CSV loading failed: " + e);
        }
    }

    // ==================================================
    // DROPDOWN SELECTION
    // ==================================================

    public void OnObjectDropdownChanged(int index)
    {
        StopReplay();

        // Do not clear the original dictionary data.
        frames = new List<ReplayFrame>();

        selectedObject = null;

        SetOnlySelectedObjectActive(null);

        if (!hasLoadedData)
            return;

        if (index < 0 || index >= replayObjects.Count)
        {
            Debug.LogWarning("Invalid dropdown selection.");
            return;
        }

        selectedObject = replayObjects[index];

        if (selectedObject == null)
        {
            Debug.LogWarning("Replay object reference is null.");
            return;
        }

        if (selectedObject.targetObject == null)
        {
            Debug.LogWarning(
                "No GameObject assigned for " +
                selectedObject.csvObjectName
            );
            return;
        }

        // Activate selected object and deactivate all others.
        SetOnlySelectedObjectActive(selectedObject);

        if (!selectedObject.hasData ||
            !replayData.ContainsKey(selectedObject.csvObjectName) ||
            replayData[selectedObject.csvObjectName].Count == 0)
        {
            Debug.LogWarning(
                "No replay data available for " +
                selectedObject.csvObjectName
            );
            return;
        }

        frames = replayData[selectedObject.csvObjectName];

        playbackTime = 0;
        currentFrameIndex = 0;

        // Apply initial object AND camera transforms.
        ApplyFrame(frames[0]);

        Debug.Log(
            selectedObject.csvObjectName +
            " | " + frames.Count + " records available."
        );
    }

    // ==================================================
    // REPLAY SELECTED OBJECT
    // ==================================================

    public void ReplaySelectedObject()
    {
        if (objectDropdown == null)
        {
            Debug.LogError("TMP Dropdown not assigned.");
            return;
        }

        if (!hasLoadedData ||
            string.IsNullOrEmpty(loadedFilePath))
        {
            Debug.LogWarning("Browse and load a CSV first.");
            return;
        }

        // Re-evaluate the current selection.
        OnObjectDropdownChanged(objectDropdown.value);

        if (selectedObject == null ||
            !selectedObject.hasData ||
            frames.Count == 0)
        {
            return;
        }

        PlayReplay();
    }

    // ==================================================
    // PLAYBACK CONTROLS
    // ==================================================

    public void PlayReplay()
    {
        if (!hasLoadedData || frames.Count < 2)
        {
            Debug.LogWarning("Insufficient replay records.");
            return;
        }

        isPlaying = true;

        Debug.Log(
            "Playing: " + selectedObject.csvObjectName
        );
    }

    public void PauseReplay()
    {
        isPlaying = false;
        Debug.Log("Replay paused.");
    }

    public void StopReplay()
    {
        isPlaying = false;
        playbackTime = 0;
        currentFrameIndex = 0;
    }

    public void RestartReplay()
    {
        if (!hasLoadedData || frames.Count == 0)
            return;

        StopReplay();

        // Restore initial object AND camera transforms.
        ApplyFrame(frames[0]);

        PlayReplay();
    }

    public void SetPlaybackSpeed(float speed)
    {
        playbackSpeed = Mathf.Max(0.1f, speed);
    }

    // ==================================================
    // REPLAY UPDATE
    // ==================================================

    private void Update()
    {
        if (!isPlaying || !hasLoadedData ||
            frames == null || frames.Count < 2)
            return;

        playbackTime += Time.deltaTime * playbackSpeed;

        double targetTime =
            frames[0].time + playbackTime;

        // End of replay.
        if (targetTime >= frames[frames.Count - 1].time)
        {
            ApplyFrame(frames[frames.Count - 1]);

            isPlaying = false;

            Debug.Log("Replay completed.");

            return;
        }

        // Find the two surrounding frames.
        while (currentFrameIndex < frames.Count - 2 &&
               frames[currentFrameIndex + 1].time < targetTime)
        {
            currentFrameIndex++;
        }

        ReplayFrame a = frames[currentFrameIndex];
        ReplayFrame b = frames[currentFrameIndex + 1];

        double interval = b.time - a.time;

        float t = interval <= 0
            ? 0f
            : (float)((targetTime - a.time) / interval);

        t = Mathf.Clamp01(t);

        ApplyInterpolatedFrame(a, b, t);
    }

    // ==================================================
    // APPLY COMPLETE FRAME
    // Object position + rotation
    // Camera position + rotation
    // ==================================================

    private void ApplyFrame(ReplayFrame frame)
    {
        // 1. Object position
        // 2. Object rotation
        if (selectedObject != null &&
            selectedObject.targetObject != null)
        {
            Transform target = selectedObject.targetObject;

            target.position = frame.position;
            target.rotation = frame.rotation;
        }

        // 3. Camera position
        // 4. Camera rotation
        if (replayCamera != null)
        {
            replayCamera.position = frame.cameraPosition;
            replayCamera.rotation = frame.cameraRotation;
        }
    }

    // ==================================================
    // APPLY INTERPOLATED FRAME
    // ==================================================

    private void ApplyInterpolatedFrame(
        ReplayFrame a,
        ReplayFrame b,
        float t)
    {
        // Object position interpolation.
        // Object rotation interpolation.
        if (selectedObject != null &&
            selectedObject.targetObject != null)
        {
            Transform target = selectedObject.targetObject;

            target.position = Vector3.Lerp(
                a.position,
                b.position,
                t
            );

            target.rotation = Quaternion.Slerp(
                a.rotation,
                b.rotation,
                t
            );
        }

        // Camera position interpolation.
        // Camera rotation interpolation.
        if (replayCamera != null)
        {
            replayCamera.position = Vector3.Lerp(
                a.cameraPosition,
                b.cameraPosition,
                t
            );

            replayCamera.rotation = Quaternion.Slerp(
                a.cameraRotation,
                b.cameraRotation,
                t
            );
        }
    }

    // ==================================================
    // CSV PARSING HELPERS
    // ==================================================

    private string GetValue(
        string[] values,
        Dictionary<string, int> columns,
        string column)
    {
        return values[columns[column]].Trim();
    }

    private Vector3 ParseVector3(string value)
    {
        string[] components = value.Split(';');

        if (components.Length != 3)
        {
            throw new FormatException(
                "Invalid Vector3: " + value
            );
        }

        return new Vector3(
            float.Parse(
                components[0],
                CultureInfo.InvariantCulture
            ),
            float.Parse(
                components[1],
                CultureInfo.InvariantCulture
            ),
            float.Parse(
                components[2],
                CultureInfo.InvariantCulture
            )
        );
    }

    private string[] ParseCSVLine(string line)
    {
        List<string> result = new List<string>();

        bool insideQuotes = false;
        string current = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (insideQuotes &&
                    i + 1 < line.Length &&
                    line[i + 1] == '"')
                {
                    current += '"';
                    i++;
                }
                else
                {
                    insideQuotes = !insideQuotes;
                }
            }
            else if (c == ',' && !insideQuotes)
            {
                result.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }

        result.Add(current);

        return result.ToArray();
    }
}
