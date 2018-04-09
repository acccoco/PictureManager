using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace PictureAcc
{
    class TreeControler
    {
        public const string xmlPath = Utils.TreePath;
        public static XDocument tree;

        public static void Load()
        {
            if (!File.Exists(xmlPath))
            {
                FirstWrite();
            }
            else
            {
                tree = XDocument.Load(xmlPath);
            }
        }
        public static void Save()
        {
            tree.Save(xmlPath);
        }

        public static void AddTree(Tree _tree)
        {
            XElement _xe = SelectElement(_tree.TreeName);
            if(_xe != null)
            {
                _xe.Remove();
            }
            tree.Element("acc").Add(_tree.TransToXml());
            tree.Save(xmlPath);
        }
        public static void RemoveTree(string _treeName)
        {
            XElement _xe = SelectElement(_treeName);
            if(_xe != null)
            {
                _xe.Remove();
                tree.Save(xmlPath);
            }
        }
        public static Tree GetTree(string _treeName)
        {
            XElement _xe = SelectElement(_treeName);
            if(_xe == null)
            {
                return new Tree(_treeName);
            }
            else
            {
                return new Tree(_xe);
            }
        }
        /// <summary>
        /// remove tag in all trees.
        /// </summary>
        public static void RemoveTag(string _tag)
        {
            foreach(var _piece in tree.Element("acc").Elements())
            {
                if(SelectTag(_piece, _tag) != null)
                {
                    SelectTag(_piece, _tag).Remove();
                }
            }
            tree.Save(xmlPath);
        }
        public static void CleatData()
        {
            foreach(var _piece in tree.Element("acc").Elements())
            {
                foreach(var _tag in _piece.Elements("tag"))
                {
                    XElement _xe = TagsControler.SelectElement(_tag.FirstNode.ToString());
                    if(_xe == null)
                    {
                        _tag.Remove();
                    }
                }
            }
            tree.Save(xmlPath);
        }




        #region helper method
        /// <summary>
        /// assign tree and save to xml.
        /// </summary>
        private static void FirstWrite()
        {
            tree = new XDocument(new XElement("acc"));
            tree.Save(xmlPath);
        }
        private static XElement SelectElement(string _treeName)
        {
            var _target =
                from piece in tree.Element("acc").Elements()
                where piece.Element("tree").FirstNode.ToString() == _treeName
                select piece;
            if(_target.Count() == 0)
            {
                return null;
            }
            else
            {
                return _target.First();
            }
        }
        private static XElement SelectTag(XElement piece,string _tag)
        {
            var _target =
                from _taag in piece.Elements("tag")
                where _taag.FirstNode.ToString() == _tag
                select _taag;
            if (_target.Count() == 0)
            {
                return null;
            }
            else
            {
                return _target.First();
            }

        }
        #endregion
    }
}
