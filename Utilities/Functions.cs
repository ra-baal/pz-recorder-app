﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Utilities
{
    [Obsolete]
    public static class Functions
    {
        private const string _dll = "RecorderDll.dll";

        /// <summary>
        /// Saves PointCloudData obtained from Kinect sensor to file.
        /// </summary>
        /// <param name="output">File name</param>
        /// <param name="binaryFile">Whether output file should be binary</param>
        [DllImport(_dll)]
        public static extern void savePointCloudDataFromKinect(string output, bool binaryFile = false);



        [DllImport(_dll)]
        public static extern void setKinect();

        [DllImport(_dll)]
        public static extern void recordAndSaveCloud(string file);
    }
}
