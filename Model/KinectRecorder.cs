using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace Recorder.Model
{
    public enum RecorderStates
    {
        NoSensor,
        Ready,
        Preview,
        Recording
    }

    class KinectRecorder
    {
        #region Data.

        protected KinectSensor _sensor;
        protected byte[] _colorPixels;

        protected int _framerate = 3; // Frames per a second.
        protected string _dirname = "Real3DFilm"; // Main directory of a film.
        protected string _prefix = "3dframe"; // Prefix of frame file names.
        protected List<string> _frameNames = new List<string>(); // Names of recorded frame files.

        public List<Action> ActionsOnColorFrameReady { get; set; } = new List<Action>();

        public WriteableBitmap ColorBitmap { get; private set; }

        public RecorderStates State { get; private set; }

        #endregion

        #region Public section.

        public KinectRecorder()
        {
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this._sensor = potentialSensor;
                    break;
                }
            }

            if (this._sensor != null)
            {
                this.State = RecorderStates.Ready;

                // Turn on the color stream to receive color frames
                this._sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                // Allocate space to put the pixels we'll receive
                this._colorPixels = new byte[this._sensor.ColorStream.FramePixelDataLength];

                // This is the bitmap we'll display on-screen
                this.ColorBitmap = new WriteableBitmap(this._sensor.ColorStream.FrameWidth, this._sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Add an event handler to be called whenever there is new color frame data
                this._sensor.ColorFrameReady += this._sensorColorFrameReady;
                //this.sensor.DepthFrameReady // głębia
            }

            if (this._sensor == null)
            {
                this.State = RecorderStates.NoSensor;
            }

        }

        /// <summary>
        /// Start the sensor!
        /// </summary>
        public void StartPreview()
        {
            try
            {
                this._sensor.Start();
                this.State = RecorderStates.Preview;
            }
            catch (IOException e)
            {
                this._sensor = null;
                this.State = RecorderStates.NoSensor;
                throw new IOException("Kinect recording start failed.", e);
            }
        }

        /// <summary>
        /// Start recording the video!
        /// </summary>
        public virtual async void StartRecording()
        {
            this.State = RecorderStates.Recording;
            Directory.CreateDirectory(_dirname);
            File.AppendAllText($"{_dirname}/settings.vrfilm", "pcd\n");
            var result = await Task.Run(_saveFramesWhileRecordingAsync);
        }

        /// <summary>
        /// Stops preview and recording.
        /// </summary>
        public void Stop()
        {
            if (null != this._sensor)
            {
                if (State == RecorderStates.Recording)
                {
                    using (StreamWriter sw = new StreamWriter($"{_dirname}/settings.vrfilm"))
                    {
                        sw.WriteLine("pcd");
                        sw.WriteLine(_frameNames.Count);
                        _frameNames.ForEach(sw.WriteLine);
                    }

                    _frameNames = new List<string>();
                }

                //this.sensor.Stop();
                this.State = RecorderStates.Ready; // And then _saveFramesWhileRecordingAsync ends.
            }
        }

        #endregion

        #region Protected section.

        /// <summary>
        /// Event handler for Kinect sensor's ColorFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        protected void _sensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {

            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    // Copy the pixel data from the image to a temporary array
                    colorFrame.CopyPixelDataTo(this._colorPixels);

                    // Write the pixel data into our bitmap
                    this.ColorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.ColorBitmap.PixelWidth, this.ColorBitmap.PixelHeight),
                        this._colorPixels,
                        this.ColorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }

            foreach (var action in ActionsOnColorFrameReady)
            {
                action.Invoke();
            }

        }

        protected Task<int> _saveFramesWhileRecordingAsync()
        {
            int index = 0;

            while (this.State == RecorderStates.Recording)
            {
                string filename = $@"{_dirname}/{_prefix}{index}.pcd";
                Utilities.Functions.savePointCloudDataFromKinect(filename);
                _frameNames.Add(filename);
                //File.WriteAllText($@"{name}/{prefix}{index}.txt", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                index++;
                Thread.Sleep(1000 / _framerate);
            }
            return Task.FromResult(0);
        }

        #endregion

        ~KinectRecorder()
        {
            this.Stop();
        }

    }
}
