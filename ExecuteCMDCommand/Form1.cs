using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ExecuteCMDCommand
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputString = textBox1.Text;
            if (isValidSyntax(inputString))
            {
                int seconds = inputStringToSeconds(inputString);
                shutdownAfter(seconds);
            }
            else
            {
                MessageBox.Show(SYNTAX_ERROR_MSG);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            abort();
        }

        private const string SECOND = "s";
        private const string MINUTE = "m";
        private const string HOUR = "h";
        private const string SYNTAX_ERROR_MSG = "Wrong syntax";
        private const string GUIDE = "Enter <number><unit> to set the time to shutdown pc\n" +
                                    "number is a positive integer\n" +
                                    "unit is one of 's', 'm' or 'h'\n" +
                                    "example: 30m its mean 30 minutes";

        private string subPart1(string s)
        {
            return s.Substring(0, s.Length - 1);
        }

        private string subPart2(string s)
        {
            return s.Substring(s.Length - 1);
        }

        private bool isValidSyntax(string s)
        {
            string part1 = subPart1(s);
            string part2 = subPart2(s);
            bool isValidNumber = false;
            bool isValidUnit = false;

            try
            {
                int i = int.Parse(part1);
                if (i >= 0)
                {
                    isValidNumber = true;
                }
            }
            catch (FormatException)
            {}

            if (SECOND.Equals(part2) || MINUTE.Equals(part2) || HOUR.Equals(part2))
            {
                isValidUnit = true;
            }

            return isValidNumber && isValidUnit;
        }
        
        private int inputStringToSeconds(string inputString)
        {
            int seconds = 10;
            string part1 = subPart1(inputString);
            string part2 = subPart2(inputString);

            seconds = int.Parse(part1);

            if (MINUTE.Equals(part2))
            {
                seconds *= 60;
            }
            else if (HOUR.Equals(part2))
            {
                seconds *= 60 * 60;
            }

            return seconds;
        }

        private void shutdownAfter(int seconds)
        {
            executeShutdown("-s -t " + seconds);
        }

        private void abort()
        {
            executeShutdown("-a");
        }

        private void executeShutdown(string arguments)
        {
            execute(@"C:\Windows\System32\shutdown.exe", arguments);
        }

        private void execute(string fileName, string arguments)
        {
            startInfo.FileName = fileName;
            startInfo.Arguments = arguments;
            Process.Start(startInfo);
        }

        private ProcessStartInfo startInfo = new ProcessStartInfo();

        private void Form1_Load(object sender, EventArgs e)
        {
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            label1.Text = GUIDE;
        }
    }
}
