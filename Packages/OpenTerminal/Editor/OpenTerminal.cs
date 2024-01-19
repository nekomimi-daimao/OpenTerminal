#if UNITY_EDITOR_OSX

using System.Diagnostics;
using System.IO;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Nekomimi.Daimao.Editor
{
    public static class OpenTerminal
    {
        private static Process _process;

        [MenuItem("Assets/Open Terminal")]
        private static void Open()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
            var fullPath = Path.GetFullPath(path);
            DirectoryInfo dir;
            if (Directory.Exists(fullPath))
            {
                dir = new DirectoryInfo(fullPath);
            }
            else if (File.Exists(fullPath))
            {
                var f = new FileInfo(fullPath);
                dir = f.Directory;
            }
            else
            {
                return;
            }

            if (dir == null || !dir.Exists)
            {
                return;
            }

            var openDir = dir.FullName;
            Debug.Log($"{nameof(OpenTerminal)} : {openDir}");

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "/System/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal",
                Arguments = $"cd {openDir}",
                WorkingDirectory = openDir,
                CreateNoWindow = false,
                UseShellExecute = false,
            };

            if (_process != null && !_process.HasExited)
            {
                _process.CloseMainWindow();
                _process.WaitForExit(10000);
            }

            _process = null;
            _process = Process.Start(processStartInfo);
        }
    }
}

#endif
