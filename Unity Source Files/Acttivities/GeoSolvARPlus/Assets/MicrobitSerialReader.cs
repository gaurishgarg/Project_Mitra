
using System;
using System.Collections;
using System.IO.Ports;
using UnityEngine;

public class MicrobitSerialReader : MonoBehaviour
{
    [Header("Serial Settings")]
    public int baudRate = 115200;

    public  int pitch;
    public  int heading;
    public  int roll;

    public int sendpitch;
    public int sendheading;
    public int sendroll;

    public SerialPort serialPort;

    bool connected = false;
    bool receivedataok = false;

    public bool confimreceivedata = false;

    public GameObject calibrator;

    public Vector3 initial_microbit_angles;

    public bool initial_angles_received = false;

    void Start()
    {
    }

    public bool ConnectToMicrobit()
    {
        string[] ports = SerialPort.GetPortNames();

        Debug.Log("Available COM ports: " + string.Join(", ", ports));

        foreach (string port in ports)
        {
            SerialPort candidate = null;

            try
            {
                Debug.Log("Checking " + port);

                candidate = new SerialPort(port, baudRate);
                candidate.ReadTimeout = 500;
                candidate.NewLine = "\n";
                candidate.Open();

                // Look for valid micro:bit sensor data.
                float deadline = Time.realtimeSinceStartup + 1.0f;

                while (Time.realtimeSinceStartup < deadline)
                {
                    try
                    {
                        string line = candidate.ReadLine().Trim();

                        if (line == "1")
                        {
                            serialPort = candidate;

                            Debug.Log("Micro:bit detected on " + port);

                            // Tell the micro:bit to start transmitting rotation data.
                            serialPort.WriteLine("9");

                            Debug.Log("Handshake sent. Waiting for rotation data.");
                            connected = true;


                            return connected;
                        }
                    }
                    catch (TimeoutException)
                    {
                        // Wait for the next complete line.
                    }
                }

                candidate.Close();
                candidate.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogWarning(
                    "Could not use " + port + ": " + e.Message);

                if (candidate != null)
                {
                    try
                    {
                        if (candidate.IsOpen)
                            candidate.Close();

                        candidate.Dispose();
                    }
                    catch { }
                }
            }
        }

        
        Debug.LogError(
            "Micro:bit not found. Check the USB connection and sensor output.");
        return connected;
    }

    public void getinitialstate()
    {
        if (connected && receivedataok)
        {
            try
            {
                string line = serialPort.ReadLine().Trim();

                if (TryParseSensorLine(
                    line, out int p, out int h, out int r))
                {
                    pitch = p;
                    heading = h;
                    roll = r;

                    Debug.Log(
                        $"Pitch: {pitch}, Heading: {heading}, Roll: {roll}");



                    initial_microbit_angles = new Vector3(p, h, r);

                    initial_angles_received = true;

             
                }
            }
            catch (TimeoutException)
            {
                // No complete line available yet.
            }
            catch (Exception e)
            {
                Debug.LogError("Serial read error: " + e.Message);
            }
        }
    }

    public void enablerotationdataread()
    {
        confimreceivedata = true;

        Debug.Log("Got initial state. Waiting for continous updates");

        serialPort.WriteLine("3");

        StartCoroutine(ReadRotationData());
    }
  

    IEnumerator ReadRotationData()
    {
        while (true)
        {
            if (!confimreceivedata ||
                serialPort == null ||
                !serialPort.IsOpen)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            try
            {
                string line = serialPort.ReadLine().Trim();

                if (TryParseSensorLine(line, out int p, out int h, out int r))
                {
                    pitch = p; sendpitch = p;
                    heading = h; sendheading = h;
                    roll = r; sendroll = r;

                    Debug.Log($"Pitch: {pitch}, Heading: {heading}, Roll: {roll}");

                }
            }

            catch (TimeoutException)
            {
                // No complete line available yet.
                Debug.LogError("Timeout");
            }
            catch (Exception e)
            {
                Debug.LogError("Serial read error: " + e.Message);
                //yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.100f);
        }
    }

    public bool TryParseSensorLine(
        string line, out int p, out int h, out int r)
    {
        p = h = r = 0;

        string[] fields = line.Split(',');

        if (fields.Length != 3)
            return false;

        if (!int.TryParse(fields[0].Trim(), out p) ||
            !int.TryParse(fields[1].Trim(), out h) ||
            !int.TryParse(fields[2].Trim(), out r))
            return false;

        return p >= -180 && p <= 180 &&
               h >= 0 && h <= 359 &&
               r >= -180 && r <= 180;
    }
    
    public void receive_data()
    {
        if (serialPort != null && serialPort.IsOpen && connected)
        {
            serialPort.WriteLine("2");
            receivedataok = true;

            Debug.Log("Sent command 2. Receiving rotation data.");
        }
        else
        {
            Debug.LogWarning("Micro:bit is not connected.");
        }
    }

    public Vector3 receive_initial_angles()
    {
        return initial_microbit_angles;
    }

    public Vector3 receive_values()
    {
        return new Vector3(sendpitch, sendheading, sendroll);
    }

    void OnDestroy()
    {
        if (serialPort != null)
        {
            try
            {
                if (serialPort.IsOpen)
                    serialPort.Close();

                serialPort.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogWarning(
                    "Error closing serial port: " + e.Message);
            }
        }
    }
}
