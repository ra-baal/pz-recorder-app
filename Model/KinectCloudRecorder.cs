using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Recorder.Model
{
    public class KinectCloudRecorder
    {
        #region Data.

        //protected int _framerate = 1; // Frames per a second.
        protected string _dirprefix = "Real3DFilm"; // Prefix of main directory of a film.
        protected string _dirname; // Name of main directory of a film. It's set in StartRecording.
        protected string _fileprefix = "3dframe"; // Prefix of frame file names.
        protected List<string> _fileNames = new List<string>(); // Names of recorded frame files.

        public RecorderStates State { get; protected set; }

        #endregion

        #region Public section.

        public KinectCloudRecorder()
        {
            this.State = RecorderStates.Ready;
        }

        public virtual async void Start()
        {
            this.State = RecorderStates.Recording;
            _dirname = _dirprefix + " " + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            Directory.CreateDirectory(_dirname);
            int result = await Task.Run(_saveFramesWhileRecordingAsync);
        }

        public void Stop()
        {
            if (State == RecorderStates.Recording)
            {
                this.State = RecorderStates.Ready; // And then _saveFramesWhileRecordingAsync ends.

                Thread.Sleep(2000); // Waiting for the loop to complete in _saveFramesWhileRecordingAsync.

                using (StreamWriter sw = new StreamWriter($@"{_dirname}/settings.vrfilm"))
                {
                    sw.WriteLine("pcd");
                    sw.WriteLine(_fileNames.Count);
                    _fileNames.ForEach(sw.WriteLine);
                }

                _fileNames = new List<string>();

            }
        }

        #endregion

        #region Protected section.

        protected Task<int> _saveFramesWhileRecordingAsync()
        {
            int index = 0;

            Utilities.Functions.setKinect();

            while (this.State == RecorderStates.Recording)
            {
                string filename = $@"{_fileprefix}{index}.pcd";
                string filepath = $@"{_dirname}/{filename}";
                Utilities.Functions.recordAndSaveCloud(filepath);
                _fileNames.Add(filename);

                //Thread.Sleep(1000 / _framerate);

                index++;
            }

            return Task.FromResult(0);
        }

        #endregion

        ~KinectCloudRecorder()
        {
            this.Stop();
        }

    }


}
