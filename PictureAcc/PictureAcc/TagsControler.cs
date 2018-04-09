using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Windows.Forms;

namespace PictureAcc
{
    public class TagsControler
    {
        public const string xmlPath = Utils.TaagsPath;
        public static XDocument taags;



        /// <summary>
        /// load taags from xml document.
        /// </summary>
        public static void Load()
        {
            if (File.Exists(xmlPath))
            {
                taags = XDocument.Load(xmlPath);
            }
            else
            {
                Synchronize();
            }
        }
        /// <summary>
        /// save taags as xml document.
        /// </summary>
        public static void Save()
        {
            taags.Save(xmlPath);
        }
        /// <summary>
        /// synchronize taags with pieces and save.
        /// </summary>
        public static void Synchronize()
        {
            //get all tags and its pathes
            List<string> tempTags = new List<string>();
            List<XElement> tempPieces = new List<XElement>();
            foreach(var piece in PieceControler.pieces.Element("acc").Elements())
            {
                string path_string = piece.Element("path").FirstNode.ToString();
                foreach(var tag in piece.Elements("tag"))
                {
                    string tag_string = tag.FirstNode.ToString();
                    if (!tempTags.Contains(tag_string))
                    {
                        tempTags.Add(tag_string);
                        tempPieces.Add(new XElement("piece", new XElement("tag", tag_string), new XElement("path", path_string)));
                    }
                    else
                    {
                        var targetPiece =
                            from tempPiece in tempPieces
                            where tempPiece.Element("tag").FirstNode.ToString() == tag_string
                            select tempPiece;
                        targetPiece.First().Add(new XElement("path", path_string));
                    }
                }
            }
            //creat new taags
            XDocument xd = new XDocument(new XElement("acc"));
            foreach(var tempPiece in tempPieces)
            {
                xd.Element("acc").Add(tempPiece);
            }
            taags = xd;
            //save
            taags.Save(xmlPath);
        }



        /// <summary>
        /// return all tags existed.
        /// </summary>
        public static List<string> GetAllTags()
        {
            List<string> tags = new List<string>();
            foreach(var piece in taags.Element("acc").Elements())
            {
                string tag = piece.Element("tag").FirstNode.ToString();
                if (!tags.Contains(tag))
                {
                    tags.Add(tag);
                }
            }
            return tags;
        }
        /// <summary>
        /// add tag in taags and save.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="tag"></param>
        public static void AddTag(string fullPath,string tag)
        {
            XElement xe = SelectElement(tag);
            if(xe != null)
            {
                if(SelectPath(xe,fullPath) == null)
                {
                    xe.Add(new XElement("path", fullPath));
                }
            }
            else
            {
                taags.Element("acc").Add(new XElement("piece", new XElement("tag", tag), new XElement("path", fullPath)));
            }
            taags.Save(xmlPath);
        }
        /// <summary>
        /// remove tag in taags and save.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="tag"></param>
        public static void RemoveTag(string fullPath,string tag)
        {
            XElement xe = SelectElement(tag);
            if(xe != null)
            {
                if(SelectPath(xe,fullPath) != null)
                {
                    SelectPath(xe, fullPath).Remove();
                }
                if(xe.Elements("path").Count() == 0)
                {
                    xe.Remove();
                    TreeControler.RemoveTag(tag);
                }
            }
            taags.Save(xmlPath);
        }
        /// <summary>
        /// return pathes which have specific tag.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetPathes(string _tag)
        {
            List<string> _pathes = new List<string>();
            XElement _piece = SelectElement(_tag);
            if(_piece != null)
            {
                foreach(var _path in _piece.Elements("path"))
                {
                    _pathes.Add(_path.FirstNode.ToString());
                }
            }
            return _pathes;
        }




        #region helper method
        /// <summary>
        /// get piece with tag in taags.if tag exists,return null.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static XElement SelectElement(string tag)
        {
            var file =
                from piece in taags.Element("acc").Elements()
                where piece.Element("tag").FirstNode.ToString() == tag
                select piece;
            if(file.Count() == 0)
            {
                return null;
            }
            else
            {
                XElement xe = file.First();
                return xe;
            }
        }
        private static XElement SelectPath(XElement piece, string path)
        {
            var vv =
                from pathh in piece.Elements("path")
                where pathh.FirstNode.ToString() == path
                select pathh;
            if(vv.Count() == 0)
            {
                return null;
            }
            else
            {
                return vv.First();
            }
        }
        #endregion
    }
}
