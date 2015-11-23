using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CR2ToJpegConverter
{
    class dtoListPath
    {
        private LinkedList<string> list;
        private string path;

        public dtoListPath(LinkedList<string> list, string path)
        {
            this.list = list;
            this.path = path;

        }

        public LinkedList<string> getList()
        {
            return list;
        }

        public string getPath()
        {
            return path;
        }

        public void setListViewItemCollection(LinkedList<string> list)
        {
            this.list = list;
        }

        public void setPath(string path)
        {
            this.path = path;
        }
    }
}
