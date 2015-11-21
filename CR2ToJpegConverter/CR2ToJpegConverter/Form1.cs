using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media.Imaging;

namespace CR2ToJpegConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.listViewPic.DragEnter += new DragEventHandler(listViewPic_DragEnter);
            this.listViewPic.DragDrop += new DragEventHandler(listViewPic_DragDrop);

        }

        private void fillListView(string[] files)
        {
            foreach (string file in files)
            {
                listViewPic.Items.Add(new ListViewItem(file));
            }
        }

        private void listViewPic_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void listViewPic_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            fillListView(files);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog.ShowDialog();
            this.listViewPic.Items.Clear();
            var files = Directory.GetFiles(@folderBrowserDialog.SelectedPath, "*.CR2");
            fillListView(files);
        }   

        private void me_file_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (listViewPic.Items.Count.Equals(0))
            {
                return;
            }
            this.folderBrowserDialog.ShowDialog();
            Converter.convert(this.listViewPic.Items, folderBrowserDialog.SelectedPath);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.listViewPic.Items.Clear();
        }
        

    }

    public class Converter
    {

        public static void convert(ListView.ListViewItemCollection list, String path)
        {
            int i = 0;
           // var files = Directory.GetFiles(@"C:\Users\Jens\Documents", "*.CR2");
            foreach (ListViewItem lvi in list)
            {
                String file =lvi.Text;
                var bmpDec = BitmapDecoder.Create(new Uri(file), BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                var bmpEnc = new JpegBitmapEncoder();
                bmpEnc.QualityLevel = 100;
                bmpEnc.Frames.Add(bmpDec.Frames[0]);
                var oldfn = Path.GetFileName(file);
                var newfn = Path.ChangeExtension(oldfn, "JPG");
                using (var ms = File.Create(Path.Combine(@path, newfn), 10000000))
                {
                    bmpEnc.Save(ms);
                }
                Console.WriteLine(newfn);
            }
        }

    }
}
