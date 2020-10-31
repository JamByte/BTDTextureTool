using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Animation;

namespace BTDTextureTool
{
    public class ImageHandler
    {
        public const int xstart =200;
        public const int ystart =200;
        public const int xinc =30;
        public const int yinc =30;

        public bool SpiltImage(SpriteInformation si, string file, List<string> output)
        {
            //  try
            // {
            Bitmap b = new Bitmap(file);

                    
            Bitmap bm = new Bitmap(b.Width, b.Height);

            JamBitMap jbm = new JamBitMap(bm);
            JamBitMap jb = new JamBitMap(b);
            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    jbm.SetPixel(x, y, jb.GetPixel(x, y));
                }
            }
            jb.Dispose();
            jbm.Unlock();
                    string[] potato = file.Split('\\');
                    string path = file.Remove(file.Length - (potato[potato.Length - 1].Length), (potato[potato.Length - 1].Length));
            string name = potato[potato.Length - 1].Split('.')[0];
            if(name != si.FrameInformation.Name)
            {
                output.Add("Name of Image does not match the name of the image in the xml");
                output.Add("Files will be saved to the folder with the name of the image");
            }


            path += @"Spilt Textures\" + name;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    for (int a = 0; a < si.FrameInformation.Animation.Count; a++)
                    {
                        string pathanim = path + @"\" + si.FrameInformation.Animation[a].Name;
                        if (!Directory.Exists(pathanim))
                        {
                            Directory.CreateDirectory(pathanim);
                        }
                        for (int i = 0; i < si.FrameInformation.Animation[a].Cell.Count; i++)
                        {
                            Rectangle imagecrop = new Rectangle(si.FrameInformation.Animation[a].Cell[i].X, si.FrameInformation.Animation[a].Cell[i].Y, si.FrameInformation.Animation[a].Cell[i].W, si.FrameInformation.Animation[a].Cell[i].H);
                            Image outputimage;
                            if (imagecrop.Width == 0 || imagecrop.Height == 0)
                            {
                                outputimage = new Bitmap(1, 1);
                            }
                            else if (imagecrop.X + imagecrop.Width > bm.Width || imagecrop.Y + imagecrop.Height > bm.Height || imagecrop.X < 0 || imagecrop.Y < 0)
                            {

                                output.Add(si.FrameInformation.Animation[a].Cell[i].Name + " is out of bounds, filling out of bounds parts with blank pixels.");
                                Bitmap bmap = new Bitmap(si.FrameInformation.Animation[a].Cell[i].W, si.FrameInformation.Animation[a].Cell[i].H);
                                for (int x = 0; x < si.FrameInformation.Animation[a].Cell[i].W; x++)
                                {

                                    for (int y = 0; y < si.FrameInformation.Animation[a].Cell[i].H; y++)
                                    {
                                        int ry = y + si.FrameInformation.Animation[a].Cell[i].Y;
                                        int rx = x + si.FrameInformation.Animation[a].Cell[i].X;
                                        if (ry >= bm.Height || rx >= bm.Width)
                                        {
                                            bmap.SetPixel(x,y,Color.Transparent);
                                        }
                                        else
                                        {
                                            bmap.SetPixel(x, y, bm.GetPixel(rx, ry));
                                        }
                                    }
                                }

                                  outputimage = bmap;
    
                            }
                            else
                            {
                                outputimage = bm.Clone(imagecrop, bm.PixelFormat);
                            }
                            //outputimage = bm.Clone(imagecrop, image.PixelFormat);
                            outputimage.Save(pathanim + @"\" + si.FrameInformation.Animation[a].Cell[i].Name + ".png");
                        }
                    }

                    for (int i = 0; i < si.FrameInformation.Cell.Count; i++)
                    {
                        Rectangle imagecrop = new Rectangle(si.FrameInformation.Cell[i].X, si.FrameInformation.Cell[i].Y, si.FrameInformation.Cell[i].W, si.FrameInformation.Cell[i].H);
                        Image outputimage;
                        if (imagecrop.Width == 0 || imagecrop.Height == 0)
                        {
                            outputimage = new Bitmap(1,1);
                        }
                        else if (imagecrop.X+imagecrop.Width>bm.Width|| imagecrop.Y + imagecrop.Height > bm.Height||imagecrop.X<0||imagecrop.Y<0)
                        {

                            output.Add(si.FrameInformation.Cell[i].Name + " is out of bounds, filling out of bounds parts with blank pixels.");
                            Bitmap bmap = new Bitmap(si.FrameInformation.Cell[i].W, si.FrameInformation.Cell[i].H);
                            for (int x = 0; x < si.FrameInformation.Cell[i].W; x++)
                            {

                                for (int y = 0; y < si.FrameInformation.Cell[i].H; y++)
                                {
                                    int ry = y + si.FrameInformation.Cell[i].Y;
                                    int rx = x + si.FrameInformation.Cell[i].X;
                                    if (ry >= bm.Height || rx >= bm.Width)
                                    {
                                        bmap.SetPixel(x, y, Color.Transparent);
                                    }
                                    else
                                    {
                                        bmap.SetPixel(x, y, bm.GetPixel(rx, ry));
                                    }
                                }
                            }

                            outputimage = bmap;

                }
                        else 
                        {
                            outputimage = bm.Clone(imagecrop, bm.PixelFormat); 
                        }

                        outputimage.Save(path + @"\" + si.FrameInformation.Cell[i].Name + ".png");
                    }
            bm.Dispose();
                return true;
           // }
            //catch
            //{
                return false;
            //}

        }
        public List<string> MakeSpriteSheet(string filepath, SpriteInformation oldxml)
        {
            string[] anims = Directory.GetDirectories(filepath);
            string[] imgs = Directory.GetFiles(filepath);
            SpriteInformation si = new SpriteInformation();
            si.FrameInformation = new FrameInformation();
            si.FrameInformation.Animation = new List<Animation>();
            si.FrameInformation.Cell = new List<Cell>();
            List<tempimage> allimages = new List<tempimage>() ;
            for (int i = 0; i < anims.Length; i++)
            {
                string[] ww = anims[i].Split('\\');
                string animname = ww[ww.Length - 1].Replace("\\", "");
                si.FrameInformation.Animation.Add(new Animation {Name = animname});
                si.FrameInformation.Animation[i].Cell = new List<Cell>();
                

                string[] animimg = Directory.GetFiles(filepath+@"\"+animname+@"\");
                for (int j = 0; j < animimg.Length; j++)
                {
                    string[] wwe = animimg[j].Split('\\');
                    string imgname = wwe[wwe.Length - 1].Replace("\\", "");
                    Bitmap b = new Bitmap(animimg[j]);


                    Bitmap bm = new Bitmap(b.Width, b.Height);

                    JamBitMap jbm = new JamBitMap(bm);
                    JamBitMap jb = new JamBitMap(b);
                    for (int y = 0; y < b.Height; y++)
                    {
                        for (int x = 0; x < b.Width; x++)
                        {
                            jbm.SetPixel(x, y, jb.GetPixel(x, y));
                        }
                    }
                    jb.Dispose();
                    jbm.Unlock();

                    allimages.Add(new tempimage { img =bm, animname =  animname, imgname = imgname });
                    Cell cell = new Cell { Ay =0, Ah = allimages[allimages.Count - 1].img.Height , Aw = allimages[allimages.Count - 1].img.Width , Ax = 0, Name = imgname.Replace(".png",""), H = allimages[allimages.Count - 1].img.Height, W = allimages[allimages.Count - 1].img.Width };
                    si.FrameInformation.Animation[i].Cell.Add(cell);
                }
            }
            for (int i = 0; i < imgs.Length; i++)
            {
                Bitmap b = new Bitmap(imgs[i]);


                Bitmap bm = new Bitmap(b.Width, b.Height);

                JamBitMap jbm = new JamBitMap(bm);
                JamBitMap jb = new JamBitMap(b);
                for (int y = 0; y < b.Height; y++)
                {
                    for (int x = 0; x < b.Width; x++)
                    {
                        jbm.SetPixel(x, y, jb.GetPixel(x, y));
                    }
                }
                jb.Dispose();
                jbm.Unlock();
                allimages.Add(new tempimage { img = bm, animname = null });
                
                string[] wwe = imgs[i].Split('\\');
                string imgname = wwe[wwe.Length - 1].Replace("\\", "");
                Cell cell = new Cell { Ay = 0, Ah = allimages[allimages.Count - 1].img.Height , Aw = allimages[allimages.Count - 1].img.Width , Ax = 0, Name = imgname.Replace(".png", ""), H = allimages[allimages.Count-1].img.Height, W = allimages[allimages.Count - 1].img.Width };
                si.FrameInformation.Cell.Add(cell);
            }
            List<tempimage> allimages2 = new List<tempimage>(allimages.ToArray());
            allimages.Sort((first, second) => {return (second.img.Width * second.img.Height)-(first.img.Width * first.img.Height) ; });
            List<string> Consolelog = new List<string>();
            Consolelog.Add(allimages.Count + " Images Found");

            List<List<ushort>> gird = new List<List<ushort>>(xstart);
            for (int i = 0; i < ystart; i++)
            {
                ushort[] e = new ushort[xstart];
                List<ushort> po = new List<ushort>(e);
                
                gird.Add(po);
            }
            ushort xcount = xstart;
            ushort ycount = ystart;
            for (int i = 0; i < allimages.Count; i++)
            {
                ushort xsize = (ushort)((ushort)allimages[i].img.Width+4);//2 for padding and 2 for duplicate pixels
                ushort ysize = (ushort)((ushort)allimages[i].img.Height+4);//^

                
                bool spacefound = false;
                int loopstopper = 100;//incase of inf loops
                while (!spacefound && loopstopper > 0)
                {
                    for (int y = 0; y < ycount; y++)
                    {
                        for (int x = 0; x < xcount; x++)
                        {
                            if (x + xsize >= xcount || y + ysize >= ycount)
                            {
                                x = ushort.MaxValue;//if out of bounds grow the image 
                            }
                            else
                            {
                                if (gird[y][x] == 0)//if no other image has already taken this spot
                                {
                                    bool otherimg = false;
                                    for (int xx = 0; xx < xsize; xx++)
                                    {
                                        if(gird[y][x+xx] != 0 || gird[y+ysize][x+xx] != 0)
                                        {
                                            otherimg = true;
                                            break;
                                        }
                                    }
                                    for (int yy = 0; yy < ysize; yy++)
                                    {

                                        if (gird[y+yy][x] != 0 || gird[y+yy][x + xsize] != 0)
                                        {
                                            otherimg = true;
                                            break;
                                        }
                                    }//it just works 
                                    if (!otherimg)
                                    {
                                        for (int yy = 0; yy < ysize; yy++)
                                        {
                                            for (int xx = 0; xx < xsize; xx++)
                                            {
                                                gird[y + yy][x + xx] = (ushort)(i + 1);
                                                
                                            }
                                        }
                                        spacefound = true;
                                        tempimage eeeeeeeee = allimages[i];
                                        eeeeeeeee.x = x;
                                        eeeeeeeee.y = y;
                                        x = ushort.MaxValue;
                                        y = ushort.MaxValue;
                                        allimages[i] = eeeeeeeee;
                                    }
                                }
                                else
                                {
                                    x += allimages[gird[y][x] - 1].img.Width+3;
                                }
                            }
                        }
                    }
                    if(!spacefound)
                    {
                        ushort[] eee = new ushort[xinc];
                        for (int c = 0; c < ycount; c++)
                        {
                            gird[c].AddRange(eee);

                        }
                        xcount += xinc;
                        ycount += yinc;
                        ushort[] e = new ushort[xcount];
                        for (int c = 0; c < yinc; c++)
                        {
                            
                            gird.Add(new List<ushort>(e));
                        }
                    }
                }

            }
            Bitmap img = new Bitmap(xcount, ycount);
            for (int x = 0; x < xcount; x++)
            {
                for (int y = 0; y < ycount; y++)
                {
                    if (gird[y][x] == 0)
                    {
                        img.SetPixel(x, y, Color.Transparent);
                    }
                    else
                    {
                        Color color;
                        int index = gird[y][x];
                        tempimage tempimage = allimages[index - 1];
                        int rx = (x) - (tempimage.x+2);
                        int ry = (y) - (tempimage.y+2);
                        if (rx == -2 || rx == tempimage.img.Width + 1 || ry == -2 || ry == tempimage.img.Height + 1)
                        {
                            if (rx <= 0) { rx = 0; }
                            if (rx >= tempimage.img.Width - 1) { rx = tempimage.img.Width - 1; }
                            if (ry <= 0) { ry = 0; }
                            if (ry >= tempimage.img.Height - 1) { ry = tempimage.img.Height - 1; }
                            Color c = tempimage.img.GetPixel(rx, ry);
                            Color cc = Color.FromArgb(0,c.R,c.G,c.B);
                            img.SetPixel(x, y, cc);
                        }
                        else
                        {

                            if (rx <= 0) { rx = 0; }
                            if (rx >= tempimage.img.Width - 1) { rx = tempimage.img.Width - 1; }
                            if (ry <= 0) { ry = 0; }
                            if (ry >= tempimage.img.Height - 1) { ry = tempimage.img.Height - 1; }
                            img.SetPixel(x, y, tempimage.img.GetPixel(rx, ry));
                        }
                    }

                }
            }
            //Consolelog.Add(gird.Count.ToString());
            string[] pathspilt = filepath.Split('\\');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < pathspilt.Length-3; i++)
            {
                sb.Append(pathspilt[i]);
                sb.Append('\\');
            }
            sb.Append(pathspilt[pathspilt.Length - 3]);
            sb.Append('\\');
            sb.Append(pathspilt[pathspilt.Length - 1]);
            string newpath = sb.ToString();
           // Consolelog.Add(newpath);
            img.Save(sb.ToString()+".png");
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>(); ;
            for (int i = 0; i < oldxml.FrameInformation.Animation.Count; i++)
            {
                for (int j = 0; j < oldxml.FrameInformation.Animation[i].Cell.Count; j++)
                {
                    try
                    {
                        cells.Add(oldxml.FrameInformation.Animation[i].Cell[j].Name, oldxml.FrameInformation.Animation[i].Cell[j]);
                    }
                    catch
                    {
                        Consolelog.Add(oldxml.FrameInformation.Animation[i].Cell[j].Name + " is listed multiple times in " + pathspilt[pathspilt.Length - 1] + "_jam.xml. Only the first entry has been added");
                    }
                }
            }
            for (int i = 0; i < oldxml.FrameInformation.Cell.Count; i++)
            {
                try
                {
                    cells.Add(oldxml.FrameInformation.Cell[i].Name, oldxml.FrameInformation.Cell[i]);
                }
                catch
                {
                    Consolelog.Add(oldxml.FrameInformation.Cell[i].Name + " is listed multiple times in "+ pathspilt[pathspilt.Length - 1] + "_jam.xml. Only the first entry has been added");
                }
            }
            //xml 
            int s = 0;
            for (int j = 0; j < si.FrameInformation.Animation.Count; j++)
            {
                for (int i = 0; i < si.FrameInformation.Animation[j].Cell.Count; i++)
                {
                    tempimage img2 = allimages2[s];
                    s++;
                    bool exists = cells.ContainsKey(si.FrameInformation.Animation[j].Cell[i].Name);
                    if (exists)
                    {
                        Cell diccell = cells[si.FrameInformation.Animation[j].Cell[i].Name];
                        si.FrameInformation.Animation[j].Cell[i].Ax = diccell.Ax;
                        si.FrameInformation.Animation[j].Cell[i].Ay = diccell.Ay;
                        si.FrameInformation.Animation[j].Cell[i].Aw = diccell.Aw;
                        si.FrameInformation.Animation[j].Cell[i].Ah = diccell.Ah;
                    }
                    si.FrameInformation.Animation[j].Cell[i].X = img2.x+1;
                    si.FrameInformation.Animation[j].Cell[i].Y = img2.y+1;
                }

            }
            for (int i = 0; i < si.FrameInformation.Cell.Count; i++)
            {
                tempimage img2 = allimages2[s];
                s++;
                bool exists = cells.ContainsKey(si.FrameInformation.Cell[i].Name);
                if (exists)
                {
                    Cell diccell = cells[si.FrameInformation.Cell[i].Name];
                    si.FrameInformation.Cell[i].Ax = diccell.Ax;
                    si.FrameInformation.Cell[i].Ay = diccell.Ay;
                    si.FrameInformation.Cell[i].Aw = diccell.Aw;
                    si.FrameInformation.Cell[i].Ah = diccell.Ah;
                }
                si.FrameInformation.Cell[i].X = img2.x+2;
                si.FrameInformation.Cell[i].Y = img2.y+2;

            }
           
            si.FrameInformation.Name = pathspilt[pathspilt.Length - 1];
            si.FrameInformation.Texh = img.Height;
            si.FrameInformation.Texw = img.Width;
            si.FrameInformation.Type = "png";
            XMLHandler xh = new XMLHandler();
            xh.ExportXML(sb.ToString()+".xml", si);
            Consolelog.Add("Saved image to " + sb.ToString() + ".png");
            return Consolelog;
        }
    }
    public class tempimage
    {
        public string animname;
        public string imgname;
        public int x;
        public int y;
        public Bitmap img;
    }
}
public unsafe class JamBitMap
{
    public Bitmap bitmap;
    public BitmapData bitmapdata;
    public Byte* pointer;
    public int bytesPerPixel;
    public int Width;
    public int Height;
    public JamBitMap(Bitmap b)
    {
        bitmap = b;
        Width = b.Width;
        Height = b.Height;
        Lock();
    }
    public Color GetPixel(int x, int y)
    {
        byte* currentLine = pointer + (y * bitmapdata.Stride);
        int i = bytesPerPixel * x;
        return Color.FromArgb(currentLine[i + 3], currentLine[i + 2], currentLine[i + 1], currentLine[i]);
    }
    public void SetPixel(int x, int y, Color c)
    {
        byte* currentLine = pointer + (y * bitmapdata.Stride);
        int i = bytesPerPixel * x;
        currentLine[i + 3] = c.A;
        currentLine[i] = c.B;
        currentLine[i + 1] = c.G;
        currentLine[i + 2] = c.R;
    }
    public void Lock()
    {
        bitmapdata = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
        bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
        pointer = (byte*)bitmapdata.Scan0;


    }
    public void Unlock()
    {
        bitmap.UnlockBits(bitmapdata);
    }
    public void Dispose()
    {
        bitmap.Dispose();
    }
}