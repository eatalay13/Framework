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
            var serial = _serialManager.GenerateSerial(new Entities.Dtos.GenerateLicenseDto
            {
                Domain = tbxDomainName.Text,
                BrandName = tbxFirma.Text,
                IsTimeExpiredLicense = checkBox1.Checked,
                TimeExpire = checkBox1.Checked ? dateTimePicker1.Value : DateTime.Now
            });

            tbxSerial.Text = serial;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Visible = checkBox1.Checked;
            label3.Visible = checkBox1.Checked;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
