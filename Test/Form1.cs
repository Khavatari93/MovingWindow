using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private BackgroundWorker worker;
        string newLine = Environment.NewLine;
        public Form1()
        {
            InitializeComponent();
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.RunWorkerAsync();
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(10);
                Point mousePosition = Control.MousePosition;
                Point relativePosition = new Point();
                Invoke((MethodInvoker)(() =>
                {
                    relativePosition = programStop.PointToClient(mousePosition);
                }));


                worker.ReportProgress(0, relativePosition);
                //Action safeWrite = delegate { txtBoxMousePosition.Text = "X: " + MousePosition.X.ToString() + newLine +  "Y: " + MousePosition.Y.ToString(); };
                //txtBoxMousePosition.Invoke(safeWrite);
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Point relativePosition = (Point)e.UserState;
            
        }
    }
}
