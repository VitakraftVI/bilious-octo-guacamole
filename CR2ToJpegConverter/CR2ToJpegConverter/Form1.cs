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
using System.Threading;


namespace CR2ToJpegConverter
{
    public partial class Form1 : Form
    {
        LinkedList<string> fileList;

        public Form1()
        {
            InitializeComponent();
            fileList = new LinkedList<string>();     
        }

        private void fillListView(string[] files)
        {
            foreach (string file in files)
            {
                listViewPic.Items.Add(new ListViewItem(file));
                fileList.AddLast(file);
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
            
            if (!backgroundWorker1.IsBusy)
            {
                this.folderBrowserDialog.ShowDialog();
                backgroundWorker1.RunWorkerAsync();
            }      
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.listViewPic.Items.Clear();
            this.progressBar1.Value = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation)
            {
                backgroundWorker1.CancelAsync();
            }
        }

        private void backgroundWorker1_doWork(object sender, EventArgs e)
        {
            int count = this.fileList.Count;
            int i;
            for (i = 0; i < count; i++)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    break;
                }

                Converter.convertOne(fileList.ElementAt(i), folderBrowserDialog.SelectedPath);
                int percentage = (i + 1) * 100 / count;
                backgroundWorker1.ReportProgress(percentage);
            }
            MessageBox.Show(i + " Files converted.");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_Completed(object sender, EventArgs e)
        {
            listViewPic.Items.Clear();
            fileList.Clear();
            progressBar1.Value = 0;
        }
        

    }

    

    
}
