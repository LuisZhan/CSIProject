using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZXing;
using ZXing.Mobile;
using System.Threading.Tasks;
using Android.Graphics;
using ZXing.Common;
using System.IO;
using Android.Content.PM;

namespace CSIMobile.Class.Common
{
    class CSISanner : CSIBaseObject
    {

        public static async Task<String> ScanAsync()
        {
            try
            {
                if (!Application.Context.PackageManager.HasSystemFeature(PackageManager.FeatureCamera))
                {
                    return string.Empty;
                }
                if (!IsCameraCanUse())
                {
                    return string.Empty;
                }
                Application app = new Application();
                MobileBarcodeScanner.Initialize(app);
                MobileBarcodeScanningOptions opts = new MobileBarcodeScanningOptions
                {
                    PossibleFormats = new List<BarcodeFormat>
                    {
                        BarcodeFormat.CODE_39,
                        BarcodeFormat.CODE_93,
                        BarcodeFormat.CODE_128,
                        BarcodeFormat.EAN_13,
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.QR_CODE
                    }
                };
                var scanner = new MobileBarcodeScanner();
                var result = await scanner.Scan(opts);
                return result?.Text ?? string.Empty;
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return "";
            }
        }

        public static Bitmap GenerateQRCode(string text, int width = 300, int height = 300)
        {
            try
            {
                BarcodeWriter barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Width = width,
                        Height = height,
                        Margin = 10
                    }
                };

                barcodeWriter.Renderer = new BitmapRenderer();
                Bitmap bitmap = barcodeWriter.Write(text);
                return bitmap;
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        public static MemoryStream ConvertImageStream(string text, int width = 300, int height = 300)
        {
            try
            {
                Bitmap bitmap = GenerateQRCode(text, width, height);
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);  // this is the diff between iOS and Android
                stream.Position = 0;
                return stream;
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        public static bool IsCameraCanUse()
        {
            bool canUse = false;
            Android.Hardware.Camera mCamera = null;

            try
            {
                mCamera = Android.Hardware.Camera.Open(0);
                Android.Hardware.Camera.Parameters mParameters = mCamera.GetParameters();
                mCamera.SetParameters(mParameters);
            }
            catch (Exception e)
            {
                canUse = false;
            }

            if (mCamera != null)
            {
                mCamera.Release();
                canUse = true;
            }

            return canUse;
        }
    }
}