using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace NecDMS.iOS
{
    public class Application
    {
        // This is the main entry point of the application.

        [assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
        [assembly: ExportRenderer(typeof(Entry), typeof(NecDMS.iOS.CustomEntryRenderer))]
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        
        }


       
    }

}
