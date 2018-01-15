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

namespace CSIMobile.Class.Common
{
    class CSISanner : CSIBaseObject
    {

        public static async Task<String> ScanAsync()
        {
            try
            {
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
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return "";
            }
        }
    }
}