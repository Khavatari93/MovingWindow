using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Test
{
    public partial class Window : Form
    {
        private BackgroundWorker worker;
        string newLine = Environment.NewLine;
        Random Random = new Random(DateTime.Now.Millisecond);
        Rectangle bildschirm = Screen.PrimaryScreen.Bounds;
        public Window()
        {
            InitializeComponent();

            // Erstellen eines neuen Threads indem wir die Mausposition abfragen können ohne den MainUI Thread einzufrieren.
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.RunWorkerAsync();
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
            this.FormClosing += Window_FormClosing;

        }
        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
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

            int windowY = bildschirm.Height - 82;
            int windowX = bildschirm.Width - 136;
            int randomY = Random.Next(windowY);
            int randomX = Random.Next(windowX);

            // Vergleich unserer Mausposition zum Mittelpunkt des Buttons, sollte dieser näher als 30 Pixel sein wird das Fenster zufällig auf dem Bildshcirm verschoben
            if ((Math.Abs(relativePosition.X - 48) < 49) & (Math.Abs(relativePosition.Y - 12) < 13))
            {
                this.Left = randomX;
                this.Top = randomY;
            }
            
        }
    }
}
