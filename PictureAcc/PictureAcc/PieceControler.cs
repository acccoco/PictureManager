using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace PictureAcc
{
    public class PieceControler
    {
        public const string xmlPath = Utils.PiecesPath;
        public static XDocument pieces;


        /// <summary>
        /// save pieces as xml
        /// </summary>
        public static void Save()
        {
            pieces.Save(xmlPath);
        }
        /// <summary>
        /// load pieces from xml
        /// </summary>
        public static void Load()
        {
            if (!File.Exists(xmlPath))
            {
                FirstWrite().Save(xmlPath);
                TagsControler.Synchronize();
            }
            pieces = XDocument.Load(xmlPath);
        }
        /// <summary>
        /// clear redundant piece in pieces
        /// </summary>
        public static void ClearPieces()
        {
            foreach(var piece in pieces.Element("acc").Elements())
            {
                if (!File.Exists(piece.Element("path").FirstNode.ToString()))
                {
                    DeletPiece(piece.Element("path").FirstNode.ToString());
                }
            }
        }





        /// <summary>
        /// get piece from pieces.if the piece is not exist,creat new piece with fullpath. 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static Piece GetPiece(string fullPath)
        {
            XElement xe = SelectElement(fullPath);
            if(xe != null)
            {
                return new Piece(xe);
            }
            else
            {
                return new Piece(fullPath);
            }
        }
        /// <summary>
        /// sadd the piece to the pieces and save pieces
        /// </summary>
        /// <param name="piece"></param>
        public static void AddPiece(Piece piece)
        {
            XElement xe = SelectElement(piece.FullPath);
            if (xe == null)
            {
                pieces.Element("acc").Add(piece.TransToXelment());
                pieces.Save(xmlPath);
            }
            else
            {
                xe.Remove();
                pieces.Element("acc").Add(piece.TransToXelment());
                pieces.Save(xmlPath);
            }
        }
        /// <summary>
        /// delet piece from pieces and save pieces
        /// </summary>
        /// <param name="piece"></param>
        public static void DeletPiece(string fullPath)
        {
            XElement xe = SelectElement(fullPath);
            if(xe == null)
            {
                return;
            }
            else
            {
                xe.Remove();
                pieces.Save(xmlPath);
            }
        }
        


        #region helper method
        /// <summary>
        /// find piece(xelment) with fullpath.if the piece is not exist,return null.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private static XElement SelectElement(string fullPath)
        {
            var file =
                from piece in PieceControler.pieces.Element("acc").Elements()
                where piece.Element("path").FirstNode.ToString() == fullPath
                select piece;
            if (file.Count() == 0)
            {
                return null;
            }
            else
            {
                XElement xe = file.First();
                return xe;
            }
        }
        /// <summary>
        /// find piece(xelement) with fullpath.if the piece is not exist,return null.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private static XElement SelectTag(XElement piece, string tag)
        {
            var vv =
                from tagg in piece.Elements("tag")
                where tagg.FirstNode.ToString() == tag
                select tagg;
            if (vv.Count() == 0)
            {
                return null;
            }
            else
            {
                return vv.First();
            }
        }
        /// <summary>
        /// if there is no content in "pieces.xml",write the beginning.
        /// </summary>
        /// <returns></returns>
        private static XDocument FirstWrite()
        {
            XDocument xd = new XDocument(new XElement("acc"));
            return xd;
        }
        #endregion
    }
}
