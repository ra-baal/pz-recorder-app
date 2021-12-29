using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
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
        Recording
    }

    class KinectRecorder
    {
        #region Data.

        protected KinectSensor sensor;
        protected byte[] colorPixels;

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
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (this.sensor != null)
            {
                this.State = RecorderStates.Ready;

                // Turn on the color stream to receive color frames
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                // Allocate space to put the pixels we'll receive
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];

                // This is the bitmap we'll display on-screen
                this.ColorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.ColorFrameReady += this._sensorColorFrameReady;
                //this.sensor.DepthFrameReady // głębia
            }

            if (this.sensor == null)
            {
                this.State = RecorderStates.NoSensor;
            }

        }

        //public bool IsReadyOrRecording
        //{
        //    get
        //    {
        //        return this.State == States.Ready || this.State == States.Recording;
        //    }
        //}

        /// <summary>
        /// Start the sensor!
        /// </summary>
        public void Start()
        {
            try
            {
                this.sensor.Start();
                this.State = RecorderStates.Recording;
            }
            catch (IOException e)
            {
                this.sensor = null;
                this.State = RecorderStates.NoSensor;
                throw new IOException("Kinect recording start failed.", e);
            }
        }

        /// <summary>
        /// Stop the sensor!
        /// </summary>
        public void Stop()
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
                this.State = RecorderStates.Ready;
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
                    colorFrame.CopyPixelDataTo(this.colorPixels);

                    // Write the pixel data into our bitmap
                    this.ColorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.ColorBitmap.PixelWidth, this.ColorBitmap.PixelHeight),
                        this.colorPixels,
                        this.ColorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }

            foreach (var action in ActionsOnColorFrameReady)
            {
                action.Invoke();
            }

        }

        #endregion

        ~KinectRecorder()
        {
            this.Stop();
        }

    }
}
