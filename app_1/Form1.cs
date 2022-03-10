using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AutoScroll = true;
            SetupCheckBoxes();
        }

        private void SetupCheckBoxes()
        {
            int create = (this.Size.Height - 24) / 24 - 1;
            int rows = (this.Size.Width - 249) / 99;
            int created = 0x0;
            bool limitSetted = false;

            limiterList.ForEach(limiter =>
            {
                this.Controls.Remove(limiter);
            });

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < create; j++)
                {
                    var checkbox = new CheckBox()
                    {
                        Location = new Point(164 + ((i + 1) * 99), j * 23 + 12),
                        Size = new Size(99, 17),
                        Text = $"{created + 1} Basamak"
                    };

                    this.Controls.Add(checkbox);

                    if (limit == created + 1)
                    {
                        checkbox.Checked = true;
                        SetLimit(checkbox, limit);
                        limitSetted = true;
                    };

                    created++;
                }

                if (!limitSetted) SetLimit(new CheckBox());
            }

            limiterList = this.Controls.OfType<CheckBox>().ToList();

            limiterList.ForEach(limiter =>
            {
                limiter.CheckedChanged += limiter_CheckedChanged;
                limiter.MouseDown += limiter_MouseDown;
            });
        }

        private void Form1_ResizeEnd(object sender, EventArgs e) => SetupCheckBoxes();

        private string calc_operator = "";
        private double calc_buffer = 0;
        private int limit = 3;

        private List<CheckBox> limiterList = new List<CheckBox>();

        private void button_keyboard_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= limit)
                return;

            var btn = sender as Button;

            if (btn.Text == ",")
                if (textBox1.Text.Length == 0 || textBox1.Text.Contains(","))
                    return;

            textBox1.Text += btn.Text;
        }

        private void delete_last_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                return;

            textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
        }

        private void operator_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                return;

            calc_operator = (sender as Button).Text;
            calc_buffer = Convert.ToDouble(textBox1.Text);
            textBox1.Text = "";
        }

        private void equals_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                return;

            switch (calc_operator)
            {
                case "+":
                    textBox1.Text = (calc_buffer + Convert.ToDouble(textBox1.Text)).ToString();
                    return;
                case "-":
                    textBox1.Text = (calc_buffer - Convert.ToDouble(textBox1.Text)).ToString();
                    return;
            }

        }

        private CheckBox lastChangedLimiter = new CheckBox();

        private void limiter_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;

            limiterList.ForEach(limiter =>
            {
                if (!checkbox.Equals(limiter) && sender.Equals(lastChangedLimiter))
                    limiter.Checked = false;
            });

            if (checkbox.Checked == false && limiterList.All(limiter => !limiter.Checked))
                SetLimit(new CheckBox());
        }

        private void limiter_MouseDown(object sender, MouseEventArgs e)
        {
            var checkbox = sender as CheckBox;

            limit = int.Parse(checkbox.Text.Split(' ')[0]);
            SetLimit(checkbox, limit);

            if (textBox1.Text.Length > limit)
                textBox1.Text = textBox1.Text.Substring(0, limit);
        }

        private void SetLimit(CheckBox changedLimiter, int limit = 999999)
        {
            this.limit = limit;
            lastChangedLimiter = changedLimiter;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox1.Text.Length > limit)
                textBox1.Text = textBox1.Text.Substring(0, limit);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > limit)
                textBox1.Text = textBox1.Text.Substring(0, limit);
        }
    }
}