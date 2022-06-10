using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Utilities
{
    public unsafe class TestClass
    {
        #region DllImport.

        private const string _dll = "KinectPCLLib.dll";

        [DllImport(_dll)]
        private static extern void* newTestClass_0();

        [DllImport(_dll)]
        private static extern void* newTestClass_1(int data);

        [DllImport(_dll)]
        private static extern void setData(void* objptr, int data);

        [DllImport(_dll)]
        private static extern int getData(void* objptr);

        [DllImport(_dll)]
        private static extern double sophisticatedCalculations(void* objptr, double x);

        [DllImport(_dll)]
        private static extern void deleteTestClass(void* objptr);

        #endregion

        #region Handler methods.

        private unsafe void* _objptr; // Object pointer.

        public TestClass()
        {
            _objptr = newTestClass_0();
        }

        public TestClass(int data)
        {
            _objptr = newTestClass_1(data);
        }

        public void SetData(int data)
        {
            setData(_objptr, data);
        }

        public int GetData()
        {
            return getData(_objptr);
        }

        public double SophisticatedCalculations(double x)
        {
            return sophisticatedCalculations(_objptr, x);
        }

        ~TestClass()
        {
            deleteTestClass(_objptr);
        }

        #endregion

    }

}
