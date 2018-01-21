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
using Android.Support.V4.Print;
using Android.Print;
using Android.Webkit;
using System.IO;
using Android.Graphics;

namespace CSIMobile.Class.Common.Print
{
    //参考http://blog.csdn.net/dengpeng_/article/details/60869509
    class CSIPrinter : CSIBaseObject
    {
        public static void PrintPhoto(Activity BaseActivity, string Filepath, string DocName = "")
        {
            try
            {
                PrintHelper photoPrinter = new PrintHelper(BaseActivity)
                {
                    ScaleMode = PrintHelper.ScaleModeFit
                };
                Bitmap bitmap = BitmapFactory.DecodeFile(Filepath);
                photoPrinter.PrintBitmap(DocName, bitmap);
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }

        public static void PrintQRCode(Activity BaseActivity, string QRContent, string DocName = "")
        {
            try
            {
                PrintHelper photoPrinter = new PrintHelper(BaseActivity)
                {
                    ScaleMode = PrintHelper.ScaleModeFit
                };
                Bitmap bitmap = CSISanner.GenerateQRCode(QRContent);
                photoPrinter.PrintBitmap(DocName, bitmap);
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            }

        public static PrintJob PrintHTML(Activity BaseActivity, string HTMLContent, string DocName = "")
        {
            try
            {
                // Create a WebView object specifically for printing
                WebView webView = new WebView(BaseActivity);
                webView.SetWebViewClient(new WebViewClient());

                // Generate an HTML document on the fly:
                webView.LoadDataWithBaseURL(null, HTMLContent, "text/HTML", "UTF-8", null);
                String htmlDocument = HTMLContent;

                // Get a print adapter instance
                PrintDocumentAdapter printDocumentAdapter = webView.CreatePrintDocumentAdapter(DocName);

                // Get a PrintManager instance
                PrintManager printManager = (PrintManager)BaseActivity.GetSystemService(Context.PrintService);

                // Keep a reference to WebView object until you pass the PrintDocumentAdapter to the PrintManager
                PrintJob printJob = printManager.Print(DocName, printDocumentAdapter, new PrintAttributes.Builder().Build());
                return printJob;
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }
    }
}