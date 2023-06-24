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
        Random Random = new Random(DateTime.Now.Millisecond);
        public Form1()
        {
            InitializeComponent();

            // Erstellen eines neuen Threads indem wir die Mausposition abfragen können ohne den MainUI Thread einzufrieren.
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
                
                // Einholen der relativen Position der Maus zu dem Button in 10ms abständen
                Invoke((MethodInvoker)(() =>
                {
                    relativePosition = programStop.PointToClient(mousePosition);
                }));

                worker.ReportProgress(0, relativePosition);
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Wir holen die Mausposition relativ zu unserem Button aus unseren anderen Thread
            Point relativePosition = (Point)e.UserState;

            textTest.Text = relativePosition.ToString();

            int ScreenY = Random.Next(0, Screen.PrimaryScreen.Bounds.Width);
            int ScreenX = Random.Next(0, Screen.PrimaryScreen.Bounds.Height);

            // Vergleich unserer Mausposition zum Mittelpunkt des Buttons, sollte dieser näher als 30 Pixel sein wird das Fenster zufällig auf dem Bildshcirm verschoben
            if (relativePosition.X - 48 < 48 & relativePosition.Y - 12 < 12)
            {
                //this.Top = ScreenX;
                //this.Left = ScreenY;
            }
            else if (relativePosition.X + 48 < -48 & relativePosition.Y + 12 < -12)
            {
                //this.Top = ScreenX;
                //this.Left = ScreenY;
            }
            
        }
    }
}
