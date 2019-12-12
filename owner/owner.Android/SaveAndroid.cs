using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using owner.Droid;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using System.Threading.Tasks;
using Java.IO;
using System.IO;
using Uri = Android.Net.Uri;
using MimeTypeMap = Android.Webkit.MimeTypeMap;
using OSEnvironment = Android.OS.Environment;

[assembly: Dependency(typeof(SaveAndroid))]
    public class SaveAndroid : ISave
    {
        //Method to save document as a file in Android and view the saved document
        public async Task SaveAndView(string fileName, string contentType, MemoryStream stream)
        {
            string root = null;
            //Get the root path in android device.
            if (OSEnvironment.IsExternalStorageEmulated)
            {
                root = OSEnvironment.ExternalStorageDirectory.ToString();
            }
            else
                root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //Create directory and file 
            Java.IO.File myDir = new Java.IO.File(root + "/Owner");
            myDir.Mkdir();

            Java.IO.File file = new Java.IO.File(myDir, fileName);

            //Remove if the file exists
            if (file.Exists()) file.Delete();

            //Write the stream into the file
            FileOutputStream outs = new FileOutputStream(file);
            outs.Write(stream.ToArray());

            outs.Flush();
            outs.Close();

            //Invoke the created file for viewing
            if (file.Exists())
            {
                Uri path = Uri.FromFile(file);
                string extension = MimeTypeMap.GetFileExtensionFromUrl(Uri.FromFile(file).ToString());
                string mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(path, mimeType);
                Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
            }
        }
    }