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

namespace CSIMobile.Class.Common
{
    class CSIPrinter : CSIBaseObject
    {
        public static void doPhotoPrint()
        {
            PrintHelper photoPrinter = new PrintHelper(Application.Context);
            photoPrinter.ScaleMode = PrintHelper.ScaleModeFit;
            Bitmap bitmap = BitmapFactory.DecodeResource(null, 0);
            photoPrinter.PrintBitmap("droids.jpg - test print", bitmap);
        }


        public static void doWebViewPrint() {
            WebView mWebView;
            // Create a WebView object specifically for printing
            WebView webView = new WebView(Application.Context);
            webView.SetWebViewClient(new WebViewClient());

            // Generate an HTML document on the fly:
            String htmlDocument = "<html><body><h1>Test Content</h1><p>Testing, " +
                    "testing, testing...</p></body></html>";
            webView.LoadDataWithBaseURL(null, htmlDocument, "text/HTML", "UTF-8", null);

            // Keep a reference to WebView object until you pass the PrintDocumentAdapter
            // to the PrintManager
            mWebView = webView;
        }

        public static void doPrint()
        {
            try
            {
                // Get a PrintManager instance
                PrintManager printManager = (PrintManager)Application.Context.GetSystemService(Context.PrintService);

                // Set job name, which will be displayed in the print queue
                String jobName = Application.Context.GetString(Resource.String.app_name) + "_Log.txt";

                WebView myWebView = new WebView(Application.Context);
                PrintDocumentAdapter printDocumentAdapter = myWebView.CreatePrintDocumentAdapter(jobName);
                printManager.Print("MyWebPage", printDocumentAdapter, null);
                // Start a print job, passing in a PrintDocumentAdapter implementation
                // to handle the generation of a print document
                //printManager.Print(jobName, new MyPrintDocumentAdapter(getActivity()), null); //


                //PrintDocumentAdapter printDocumentAdapter = myWebView.CreatePrintDocumentAdapter();
                //printManager.Print("MyWebPage", printDocumentAdapter, null);

            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }
    }
}