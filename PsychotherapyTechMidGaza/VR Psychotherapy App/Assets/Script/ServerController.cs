using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ServerController : MonoBehaviour
{

    [Header("Set this to the folder where server.js is located")]
    // public string nodeProjectPath = @"D:\Unity Project\TechMedGazaPsyServer";
    public string nodeProjectPath;
    private Process process;

    void Start()
    {
        nodeProjectPath = System.IO.Path.Combine(Application.dataPath, "TechMedGazaPsyServer");
        RunNodeServer();
    }

    private void OnDestroy()
    {
        KillAllNodeProcesses();
    }

    [ContextMenu("RunNodeServer")]

    public void RunNodeServer()
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            WorkingDirectory = nodeProjectPath,
            Arguments = "/C node server.js",
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        process = new Process
        {
            StartInfo = psi
        };

        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                UnityEngine.Debug.Log("OUT: " + e.Data);
        };
        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                UnityEngine.Debug.LogError("ERR: " + e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        // Don't wait for exit here if it's a long-running server
        // process.WaitForExit(); // Optional
    }

    [ContextMenu("Force Kill All Node Processes")]
    public void KillAllNodeProcesses()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/C taskkill /F /IM node.exe",
            CreateNoWindow = true,
            UseShellExecute = false
        });
    }


}
