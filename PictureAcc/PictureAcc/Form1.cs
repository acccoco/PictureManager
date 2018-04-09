using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Linq;


namespace PictureAcc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Utils.InitSource();

            InitBoxes(flowLayoutPanel1, Utils.flowPictureSize);

            PieceControler.Load();
            TagsControler.Load();
            TreeControler.Load();

            DisplayLists(listBox2, TagsControler.GetAllTags());
            ShowTree(TreeControler.tree, treeView2);

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(RecursionNode(Utils.sourcePath));
        }
        /// <summary>
        /// dir treeview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            DrawFlowView(Utils.GetSomeFiles(e.Node.Tag.ToString(), Utils.searchPattern, SearchOption.TopDirectoryOnly));
        }
        /// <summary>
        /// pictureBox in flowView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureAccBox_DoubleClick(object sender, EventArgs e)
        {
            pictureBox1.CreateGraphics().Clear(Form1.DefaultBackColor);
            Draw((sender as PictureBox).Tag.ToString(), pictureBox1);

            pictureBox1.Tag = (sender as PictureBox).Tag;

            List<string> tags = PieceControler.GetPiece((sender as PictureBox).Tag.ToString()).Tags;
            DisplayLists(listBox1, tags);

            textBox1.Focus();
        }
        /// <summary>
        /// view big picture.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 form2 = new Form2(pictureBox1.Tag.ToString());
            form2.Show();
            form2.TopMost = true;
        }
        /// <summary>
        /// clear date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            PieceControler.ClearPieces();
            TagsControler.Synchronize();
            TreeControler.CleatData();
            DisplayLists(listBox2, TagsControler.GetAllTags());
            ShowTree(TreeControler.tree, treeView2);
        }
        /// <summary>
        /// in single picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int _index = listBox1.IndexFromPoint(e.Location);
            if (_index != -1)
            {
                DrawFlowView(TagsControler.GetPathes(listBox1.Items[_index].ToString()));
            }
        }
        /// <summary>
        /// find all pictures who have the specific tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int _index = listBox2.IndexFromPoint(e.Location);
            if(_index != -1)
            {
                DrawFlowView(TagsControler.GetPathes(listBox2.Items[_index].ToString()));
            }
        }
        /// <summary>
        /// tree treeview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if((string)e.Node.Tag == "tag")
            {
                DrawFlowView(TagsControler.GetPathes(e.Node.Text));
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == System.Convert.ToChar(13))
            {
                button3_Click(sender, e);
                e.Handled = true;
            }
        }


        #region Tag related
        /// <summary>
        /// add tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                return;
            }
            else
            {
                PieceControler.GetPiece(pictureBox1.Tag.ToString()).AddTag(textBox1.Text);
                Refresher();
            }
        }
        /// <summary>
        /// remove tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItems.Count == 0)
            {
                return;
            }
            else
            {
                PieceControler.GetPiece(pictureBox1.Tag.ToString()).RemoveTag(listBox1.SelectedItems[0].ToString());
                Refresher();

            }
        }
        /// <summary>
        /// change tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItems.Count == 0||textBox1.Text == "")
            {
                return;
            }
            else
            {
                PieceControler.GetPiece(pictureBox1.Tag.ToString()).RemoveTag(listBox1.SelectedItems[0].ToString());
                PieceControler.GetPiece(pictureBox1.Tag.ToString()).AddTag(textBox1.Text);
                Refresher();
            }
        }
        #endregion

        #region tree operate
        /// <summary>
        /// add tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text != "")
            {
                // ensure data consistency
                TreeControler.AddTree(new Tree(textBox2.Text));
                //ensure view consistency
                ShowTree(TreeControler.tree, treeView2);
                textBox2.Text = "";
            }
        }
        /// <summary>
        /// add tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            if((string)treeView2.SelectedNode.Tag == "tree" && textBox2.Text != "")
            {
                Tree _tree = TreeControler.GetTree(treeView2.SelectedNode.Text);
                _tree.AddTag(textBox2.Text);
                TreeControler.AddTree(_tree);
                textBox2.Text = "";
            }
            ShowTree(TreeControler.tree, treeView2);
        }
        /// <summary>
        /// delete tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            if((string)treeView2.SelectedNode.Tag == "tree")
            {
                TreeControler.RemoveTree(treeView2.SelectedNode.Text);
            }
            ShowTree(TreeControler.tree, treeView2);
        }
        /// <summary>
        /// delete tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            if((string)treeView2.SelectedNode.Tag == "tag")
            {
                Tree _tree = TreeControler.GetTree(treeView2.SelectedNode.Parent.Text);
                _tree.RemoveTag(treeView2.SelectedNode.Text);
                TreeControler.AddTree(_tree);
            }
            ShowTree(TreeControler.tree, treeView2);
        }
        #endregion


        #region helper method
        /// <summary>
        /// draw pictures in flowView,add tags,release sources,clear canvas,clear event,
        /// </summary>
        /// <param name="pathes"></param>
        private void DrawFlowView(List<string> pathes)
        {
            flowLayoutPanel1.AutoScrollPosition = new Point(0, 0);
            ClearBoxes();
            foreach (var box in boxes)
            {
                box.DoubleClick -= new EventHandler(pictureAccBox_DoubleClick);
            }
            DisposeBitmaps();

            for (int i = 0; i < pathes.Count(); i++)
            {
                bms[i] = Draw(pathes[i], Utils.flowPictureSize);
                boxes[i].Image = bms[i];
                boxes[i].Tag = pathes[i];
                boxes[i].DoubleClick += new EventHandler(pictureAccBox_DoubleClick); 
            }
        }
        /// <summary>
        /// refresh information related to tags of single picture.
        /// </summary>
        private void Refresher()
        {
            //ensure data consistency
            List<string> tags = PieceControler.GetPiece(pictureBox1.Tag.ToString()).Tags;
            DisplayLists(listBox1, tags);
            textBox1.Text = "";

            DisplayLists(listBox2, TagsControler.GetAllTags());
            ShowTree(TreeControler.tree, treeView2);
        }
        /// <summary>
        /// display strings in listbox
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="tags"></param>
        private void DisplayLists(ListBox lb, List<string> tags)
        {
            lb.Items.Clear();

            foreach (var tag in tags)
            {
                lb.Items.Add(tag);
               // lb.Items.
            }
        }
        /// <summary>
        /// show tree xdocument in specific treeview
        /// </summary>
        /// <param name="_tree"></param>
        /// <param name="_treeView"></param>
        private void ShowTree(XDocument _tree,TreeView _treeView)
        {
            _treeView.Nodes.Clear();

            foreach(var _piece in _tree.Element("acc").Elements())
            {
                TreeNode _treeNode = new TreeNode(_piece.Element("tree").FirstNode.ToString());
                _treeNode.Tag = "tree";
                foreach(var _tag in _piece.Elements("tag"))
                {
                    TreeNode _tagNode = new TreeNode(_tag.FirstNode.ToString());
                    _tagNode.Tag = "tag";
                    _treeNode.Nodes.Add(_tagNode);
                }
                _treeView.Nodes.Add(_treeNode);
            }
            
        }
        /// <summary>
        /// creat a tree of directories and attach path to the note.tag.
        /// </summary>
        /// <param name="currentPath"></param>
        /// <returns></returns>
        private TreeNode RecursionNode(string currentPath)
        {
            TreeNode currentNode = new TreeNode();
            currentNode.Text = currentPath.Substring(currentPath.LastIndexOf(@"\") + 1);
            //添加标签
            currentNode.Tag = currentPath;

            string[] dirs = Directory.GetDirectories(currentPath);
            for (int i = 0; i < dirs.Length; i++)
            {
                string childPath = dirs[i];
                TreeNode childNode = new TreeNode();
                childNode = RecursionNode(childPath);
                currentNode.Nodes.Add(childNode);
            }
            return currentNode;
        }
        /// <summary>
        /// return bitmap of the file with specific soucePath in specific size.
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private Bitmap Draw(string sourcePath, int size)
        {
            Bitmap bm = new Bitmap(size, size);
            Image image = Image.FromFile(sourcePath);

            Graphics gr = Graphics.FromImage(bm);
            if (image.Width >= image.Height)
            {
                int relHei = image.Height * size / image.Width;
                gr.DrawImage(image, new Rectangle(0, (size - relHei) / 2, size, relHei));
            }
            else
            {
                int relWi = image.Width * size / image.Height;
                gr.DrawImage(image, new Rectangle((size - relWi) / 2, 0, relWi, size));
            }

            image.Dispose();
            gr.Dispose();

            return bm;
        }
        /// <summary>
        /// draw image in picturebox
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="box"></param>
        private void Draw(string sourcePath, PictureBox box)
        {
            Image image = Image.FromFile(sourcePath);

            Graphics gr = box.CreateGraphics();
            if (image.Width * box.Height >= image.Height * box.Width)
            {
                int relHei = image.Height * box.Width / image.Width;
                gr.DrawImage(image, new Rectangle(0, (box.Height - relHei) / 2, box.Width, relHei));
            }
            else
            {
                int relWi = image.Width * box.Height / image.Height;
                gr.DrawImage(image, new Rectangle((box.Width - relWi) / 2, 0, relWi, box.Height));
            }

            image.Dispose();
            gr.Dispose();
        }
        #endregion

        
    }
}
