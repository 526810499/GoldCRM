using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XHD.Common;

namespace XHD.View
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Drawing.Bitmap imgTemp = new Code128()
            {
                DataToEncode = "ZJW123456789",
                HumanReadable = true,
                Checksum = true,
                IsDisplayCheckCode = true,
                //,IsDisplayStartStopSign=true
                //,CodeBarColor=Color.Red

            }.getBitmapImage(Request["r"].CInt(50,false));

            byte[] bytes = imgTemp.Bitmap2Byte();
            Response.ClearContent();
            Response.ContentType = "image/jpg";
            Response.BinaryWrite(bytes);
        }
    }
}