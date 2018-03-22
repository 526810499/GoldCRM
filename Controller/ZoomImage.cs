/*
* 
功能：图片缩放裁剪
作者：行道树 QQ:43784751 Home:http://www.55rc.com TEL:18980778883
时间：2009-12-17
* 
*/

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace XHD.Controller
{
    public class ZoomImage
    {
        public static bool SaveCutPic(string pPath, string filePath, int pPartStartPointX, int pPartStartPointY,
            int pPartWidth, int pPartHeight, int pOrigStartPointX, int pOrigStartPointY, int imageWidth, int imageHeight)
        {
            using (Image originalImg = Image.FromFile(pPath))
            {
                try
                {
                    if (originalImg.Width == imageWidth && originalImg.Height == imageHeight)
                    {
                        return SaveCutPic(pPath, filePath, pPartStartPointX, pPartStartPointY, pPartWidth, pPartHeight,pOrigStartPointX, pOrigStartPointY);
                    }

                    Bitmap thumimg = MakeThumbnail(originalImg, imageWidth, imageHeight);

                    var partImg = new Bitmap(pPartWidth, pPartHeight);

                    Graphics graphics = Graphics.FromImage(partImg);
                    var destRect = new Rectangle(new Point(pPartStartPointX, pPartStartPointY),new Size(pPartWidth, pPartHeight)); //目标位置
                    var origRect = new Rectangle(new Point(pOrigStartPointX, pOrigStartPointY),new Size(pPartWidth, pPartHeight)); //原图位置（默认从原图中截取的图片大小等于目标图片的大小）

                    ///文字水印  
                    Graphics G = Graphics.FromImage(partImg);
                    //Font f = new Font("Lucida Grande", 6);
                    //Brush b = new SolidBrush(Color.Gray);
                    G.Clear(Color.White);
                    // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
                    G.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    // 指定高质量、低速度呈现。 
                    G.SmoothingMode = SmoothingMode.HighQuality;

                    graphics.DrawImage(thumimg, destRect, origRect, GraphicsUnit.Pixel);
                    //G.DrawString("Xuanye", f, b, 0, 0);
                    G.Dispose();

                    originalImg.Dispose();
                    if (File.Exists(filePath))
                    {
                        File.SetAttributes(filePath, FileAttributes.Normal);
                        File.Delete(filePath);
                    }
                    partImg.Save(filePath, ImageFormat.Jpeg);

                    partImg.Dispose();
                    thumimg.Dispose();
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }

        public static Bitmap MakeThumbnail(Image fromImg, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            int ow = fromImg.Width;
            int oh = fromImg.Height;

            //新建一个画板
            Graphics g = Graphics.FromImage(bmp);

            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.Default;
            //清空画布并以透明背景色填充
            g.Clear(Color.White);

            g.DrawImage(fromImg, new Rectangle(0, 0, width, height),
                new Rectangle(0, 0, ow, oh),
                GraphicsUnit.Pixel);

            return bmp;
        }

        public static bool SaveCutPic(string pPath, string filePath, int pPartStartPointX, int pPartStartPointY,
            int pPartWidth, int pPartHeight, int pOrigStartPointX, int pOrigStartPointY)
        {
            using (Image originalImg = Image.FromFile(pPath))
            {
                try
                {
                    var partImg = new Bitmap(pPartWidth, pPartHeight);
                    Graphics graphics = Graphics.FromImage(partImg);
                    var destRect = new Rectangle(new Point(pPartStartPointX, pPartStartPointY),
                        new Size(pPartWidth, pPartHeight)); //目标位置
                    var origRect = new Rectangle(new Point(pOrigStartPointX, pOrigStartPointY),
                        new Size(pPartWidth, pPartHeight)); //原图位置（默认从原图中截取的图片大小等于目标图片的大小）

                    ///注释 文字水印  
                    Graphics G = Graphics.FromImage(partImg);
                    //Font f = new Font("Lucida Grande", 6);
                    //Brush b = new SolidBrush(Color.Gray);
                    G.Clear(Color.White);
                    // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
                    G.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    // 指定高质量、低速度呈现。 
                    G.SmoothingMode = SmoothingMode.HighQuality;

                    graphics.DrawImage(originalImg, destRect, origRect, GraphicsUnit.Pixel);
                    //G.DrawString("Xuanye", f, b, 0, 0);
                    G.Dispose();

                    originalImg.Dispose();
                    if (File.Exists(filePath))
                    {
                        File.SetAttributes(filePath, FileAttributes.Normal);
                        File.Delete(filePath);
                    }
                    partImg.Save(filePath, ImageFormat.Jpeg);
                    partImg.Dispose();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}