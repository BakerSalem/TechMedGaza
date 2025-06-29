using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ServerController : MonoBehaviour
{
    [Header("Set this to the folder where server.js is located")]
    public string nodeProjectPath;
    private Process process;

    void Start()
    {
        nodeProjectPath = GetServerPath();

        if (System.IO.Directory.Exists(nodeProjectPath))
        {
            RunNodeServer();
        }
        else
        {
            UnityEngine.Debug.LogError("Node project path not found: " + nodeProjectPath);
        }
    }

    private void OnDestroy()
    {
        KillAllNodeProcesses();
    }

    [ContextMenu("RunNodeServer")]
    public void RunNodeServer()
    {
        string serverExePath = System.IO.Path.Combine(nodeProjectPath, "server.exe");

        if (!System.IO.File.Exists(serverExePath))
        {
            UnityEngine.Debug.LogError("server.exe not found at: " + serverExePath);
            return;
        }

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

    private string GetServerPath()
    {
#if UNITY_EDITOR
        return System.IO.Path.Combine(Application.dataPath, "TechMedGazaPsyServer");
#else
    string buildFolderPath = System.IO.Path.GetDirectoryName(Application.dataPath);
    return System.IO.Path.Combine(buildFolderPath, "TechMedGazaPsyServer");
#endif
    }
}
