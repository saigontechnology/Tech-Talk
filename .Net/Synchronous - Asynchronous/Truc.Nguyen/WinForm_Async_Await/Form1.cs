using System;
using System.Windows.Forms;
using SimpleAsyncAwait.Core.Asynchronous;
using System.Threading;

namespace SimpleAsyncAwait.Winform
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer t;

        CancellationTokenSource cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = false;
            btn_stop.Enabled = true;

            StartTimer();

            //BlockUI.BlockUI1();
            //BlockUI.NonBlockUI1();
            //await BlockUI.NonBlockUI2();
            //await BlockUI.NonBlockUI3();

            //// Create & Execute Tasks
            //IntroTask.TaskInstantiation();
            //await IntroTask.CommonWaysToCreateNewTask();

            // Popular Methods
            //await PopularMethods.ConfigureAwaitState();
            //cancellationTokenSource = new CancellationTokenSource();
            //cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));
            //await PopularMethods.TaskWhenWait(cancellationTokenSource);

            // Best Practices
            //await BestPractices.AsyncWithException();
            //await BestPractices.SumTask(4, 8);

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
            btn_start.Enabled = true;
            btn_stop.Enabled = false;
            t.Enabled = false;
        }

        private void StartTimer()
        {
            t = new System.Windows.Forms.Timer();
            t.Interval = 1;
            t.Tick += new EventHandler(timer1_Tick);
            t.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_time.Text = DateTime.Now.ToString("mm:ss.fff tt");
        }
    }
}
