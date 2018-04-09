using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PictureAcc
{
   public class Piece
    {
        private string fullPath; 
        public string FullPath
        {
            get
            {
                return fullPath;
            }
        }
        private List<string> tags = new List<string>();
        public List<string> Tags
        {
            get { return tags; }
        }
        /// <summary>
        /// instantiate piece by xlement and save it
        /// </summary>
        /// <param name="piece">xelment needs at least 1 path and more than 0 tags.</param>
        public Piece(XElement piece)
        {
            fullPath = piece.Element("path").FirstNode.ToString();
            foreach(var tag in piece.Elements("tag"))
            {
                tags.Add(tag.FirstNode.ToString());
            }
        }
        /// <summary>
        /// new and save
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="tags"></param>
        public Piece(string fullPath,params string[] tags)
        {
            this.fullPath = fullPath;
            foreach(var tag in tags)
            {
                this.tags.Add(tag);
            }
        }





        /// <summary>
        /// add tags for current piece and save it
        /// </summary>
        /// <param name="tag"></param>
        public void AddTag(string tag)
        {
            if (tags.Contains(tag))
            {
                return;
            }
            else
            {
                tags.Add(tag);
                TagsControler.AddTag(fullPath, tag);
                Save();
            }
        }
        /// <summary>
        /// remove tag from the piece
        /// </summary>
        /// <param name="tag"></param>
        public void RemoveTag(string tag)
        {
            if (tags.Contains(tag))
            {
                tags.Remove(tag);
                if(tags.Count() == 0)
                {
                    PieceControler.DeletPiece(this.fullPath);
                }
                else
                {
                    Save();
                }
                TagsControler.RemoveTag(fullPath, tag);
            }
            else
            {
                return;
            }
        }




        /// <summary>
        /// tran piece to xlement in pieces
        /// </summary>
        /// <returns></returns>
        public XElement TransToXelment()
        {
            XElement xe = new XElement("piece",
                new XElement("path", fullPath));
            foreach(var tag in tags)
            {
                xe.Add(new XElement("tag", tag));
            }
            return xe;
        }




        /// <summary>
        /// this function just depends on the storage function of pieceControler.
        /// </summary>
        private void Save()
        {
            PieceControler.AddPiece(this);
        }
    }
}
