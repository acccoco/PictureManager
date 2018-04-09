using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PictureAcc
{
    public static class Utils
    {
        //environment variable
        public static string[] searchPattern = new string[] { "*.jpg", "*.png", "*.gif", "*.jpeg", "*.bmp" };
        public static string sourcePath = @"D:\PictureAcc";
        public const string sourceXml = @".\Data\Source.xml";

        public const string PiecesPath = @".\Data\Pieces.xml";
        public const string TaagsPath = @".\Data\Tags.xml";
        public const string TreePath = @".\Data\Tree.xml";

        public const int bitmaps_count = 256;
        public const int pictureboxes_count = 256;

        public const int flowPictureSize = 100;


        public static void InitSource()
        {
            if (!File.Exists(sourceXml))
            {
                new XDocument(new XElement("acc")).Save(sourceXml);
            }

            XDocument _xd = XDocument.Load(sourceXml);
            XElement _xe = _xd.Element("acc");
            if(_xe.Elements().Count() == 0)
            {
                FolderBrowserDialog path = new FolderBrowserDialog();
                path.ShowDialog();
                sourcePath = path.SelectedPath;

                _xe.Add(new XElement("sourcePath",sourcePath));
                _xd.Save(sourceXml);
            }
            else
            {
                sourcePath = _xe.Elements().First().FirstNode.ToString();
            }
        }
        /// <summary>
        /// return some kind of files by specific suffix names
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static List<string> GetSomeFiles(string dirPath,string[] searchPattern,SearchOption searchOption)
        {
            List<string> files = new List<string>();
            for (int i = 0; i < searchPattern.Length; i++)
            {
                foreach (var file in Directory.GetFiles(dirPath, searchPattern[i],searchOption))
                {
                    files.Add(file);
                }
            }

            return files;
        }
    }
}
