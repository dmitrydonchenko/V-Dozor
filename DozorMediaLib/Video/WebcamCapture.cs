using Emgu.CV;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace DozorMediaLib.Video
{
    public class WebcamCapture : IDisposable
    {
        private ImageViewer imageViewer;
        private Capture capture;

        public WebcamCapture()
        {
            imageViewer = new ImageViewer();
            capture = new Capture(1);
        }

        public Bitmap Capture()
        {            
            return capture.QueryFrame().ToBitmap(); 
        }

        public static byte[] ImageToByteArray(Bitmap bitmap)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
        }

        public static Bitmap ByteArrayToImage(byte [] bytes)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(bytes))
            {
                bmp = new Bitmap(ms);
            }
            return bmp;
        }

        public void Dispose()
        {
            if(imageViewer != null)
            {
                imageViewer.Dispose();
            }
            if(capture != null)
            {
                capture.Dispose();
            }
        }
    }
}
