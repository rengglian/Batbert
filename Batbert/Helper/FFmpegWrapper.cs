using System;
using System.IO;

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
    }
}
