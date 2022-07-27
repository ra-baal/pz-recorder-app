using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recorder.Model
{
    [Obsolete]
    public interface IKinectCloudRecorder
    {
        RecorderStates State { get; }
        void Start();
        void Stop();

        string DirectoryPrefix { get; set; }
        string FilePrefix { get; set; }
        string Path { get; set; }
    }
}
