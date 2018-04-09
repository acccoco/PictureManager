using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PictureAcc
{
    class Tree
    {
        private string treeName;
        public string TreeName
        {
            get { return treeName; }
        }
        private List<string> tags = new List<string>();

        public Tree(string _treeName,params string[] _tags)
        {
            treeName = _treeName;
            foreach(var _tag in _tags)
            {
                tags.Add(_tag);
            }
        }
        public Tree(XElement _piece)
        {
            treeName = _piece.Element("tree").FirstNode.ToString();
            foreach(var _tag in _piece.Elements("tag"))
            {
                tags.Add(_tag.FirstNode.ToString());
            }
        }


        public void AddTag(string _tag)
        {
            if (TagsControler.SelectElement(_tag) != null)
            {
                tags.Add(_tag);
                //ensure data consistency
                TreeControler.AddTree(this);
            }
        }
        public void RemoveTag(string _tag)
        {
            if (tags.Contains(_tag))
            {
                tags.Remove(_tag);
                if(tags.Count() == 0)
                {
                    TreeControler.RemoveTree(treeName);
                }
                else
                {
                    TreeControler.AddTree(this);
                }
            }
        }

        public XElement TransToXml()
        {
            XElement xe = new XElement("piece");

            xe.Add(new XElement("tree", treeName));
            foreach(var tag in tags)
            {
                xe.Add(new XElement("tag", tag));
            }
            return xe;
        }
    }
}
