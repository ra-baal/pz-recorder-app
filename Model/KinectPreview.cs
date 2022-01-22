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

    public class KinectPreview
    {
        #region Data.

        protected KinectSensor _sensor;
        protected byte[] _colorPixels;

        public List<Action> ActionsOnPreviewFrameReady { get; set; } = new List<Action>();
        public WriteableBitmap ColorBitmap { get; private set; }
        public RecorderStates State { get; private set; }

        #endregion

        #region Public section.

        public KinectPreview()
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
                this._sensor.ColorFrameReady += this._sensorPreviewFrameReady;
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
        public void Start()
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
        /// Stops preview.
        /// </summary>
        public void Stop()
        {
            if (null != this._sensor)
            {
                this._sensor.Stop();
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
        protected void _sensorPreviewFrameReady(object sender, ColorImageFrameReadyEventArgs e)
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

            foreach (var action in ActionsOnPreviewFrameReady)
            {
                action.Invoke();
            }

        }
        
        #endregion

        ~KinectPreview()
        {
            this.Stop();
        }

    }
}
