using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class ImageToImage : MonoBehaviour
{
    public Process process;
    public StreamWriter streamWriter;
    private Thread thread;

    private List<string> liLines = new List<string>();
    private List<string> liErrors = new List<string>();
 

    public void Start()
    {
        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

     
        process.Start();
        process.BeginOutputReadLine();

        streamWriter = process.StandardInput;
        if (streamWriter.BaseStream.CanWrite)
        {
            RunCI();
        }
    }

    public void RunCI()
    {
        UnityEngine.Debug.Log("Writing: " + $"activating env");
        streamWriter.WriteLine($"cd C:/Users/Mirevi/source/repos/CI");
        streamWriter.WriteLine($"cd ci_env/Scripts");
        streamWriter.WriteLine($"activate.bat");
        streamWriter.WriteLine($"cd C:/Users/Mirevi/source/repos/CI");
        streamWriter.WriteLine($"python run_ci.py");
        UnityEngine.Debug.Log("Writing: " + $"CI running...");
    }
}