using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recorder.Model
{
    public static class RecorderSettings
    {
        public static string _dirprefix = "Real3DFilm"; // Prefix of main directory of a film.
        public static string _fileprefix = "3dframe"; // Prefix of frame file names.
        public static string _path = Directory.GetCurrentDirectory();
    }
}
