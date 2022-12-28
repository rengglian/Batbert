using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Batbert.Helper
{
    public static class FFmpegWrapper
    {
        public static string GetVersion()
        {
            var ffmpeg_path = App.Config.GetSection("ffmpeg:ExecPath").Value;
            var versionString = CmdHelper.Execute(Path.Combine(ffmpeg_path, "ffmpeg.exe"), "-version");
            return versionString[..versionString.IndexOf(Environment.NewLine)];
        }

        public static string CreateMp3(IEnumerable<string> inputFiles, string outputFile)
        {
            var inputFilesArg = "-i \"" + string.Join("\" -i \"", inputFiles) + "\"";
            var outputFileArg = $"-filter_complex concat=n={inputFiles.Count()}:v=0:a=1 -c:a mp3 -vn {outputFile}";
            var ffmpeg_path = App.Config.GetSection("ffmpeg:ExecPath").Value;
            var versionString = CmdHelper.Execute(Path.Combine(ffmpeg_path, "ffmpeg.exe"), $"{inputFilesArg} {outputFileArg}");
            return $"{Path.Combine(ffmpeg_path, "ffmpeg.exe")} {inputFilesArg} {outputFileArg}";
        }
    }
}
