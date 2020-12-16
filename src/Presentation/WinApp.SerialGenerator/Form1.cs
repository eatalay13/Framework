using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApp.SerialGenerator.LisansManager;

namespace WinApp.SerialGenerator
{
    public partial class SerialGenerateForm : Form
    {
        private readonly SerialManager _serialManager;

        public SerialGenerateForm(SerialManager serialManager)
        {
            _serialManager = serialManager;
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var serial = _serialManager.GenerateSerial(tbxDomainName.Text, tbxFirma.Text);

            tbxSerial.Text = serial;
        }
    }
}
