using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Compression
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        string inputName, outputName;
        string genericSuccess = "Operation complete.";

        private void compressButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void inputButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "All Files (*.*)|*.*|Compressed File (.cmp)|*.cmp";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                inputName = dlg.FileName;
                inputFile.Content = inputName;
            }
        }

        private void outputButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "All Files (*.*)|*.*|Compressed File (.cmp)|*.cmp";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                outputName = dlg.FileName;
                outputFile.Content = outputName;
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if ((inputName != null && outputName != null) && (inputName != outputName))
            {
                if (compressButton.IsChecked == true)
                {
                    switch (listBox.SelectedIndex)
                    {
                        case 0:
                            noCompress(inputName, outputName);
                            break;
                    }
                }
                else
                {
                    // Decompress code goes here. Make sure to verify file format
                }

            }
            else
            {
                MessageBox.Show("Error: You must specify input and output files.");
            }
        }

        // noCompress simply copies the data unaltered.
        //  Same function used for compress and decompress.
        public void noCompress(string inFile, string outFile)
        {
            // Initialize files for I/O
            FileStream inStream = File.Open(inFile, FileMode.Open);
            int bufferSize = 128;
            byte[] buffer = new byte[bufferSize];
            FileStream outStream = File.Open(outFile, FileMode.Create);

            // Copy 128 bytes from input to output, until we run out of bytes to copy.
            bool working = true;
            int byteCount;
            while (working == true)
            {
                byteCount = inStream.Read(buffer, 0, 128);
                outStream.Write(buffer, 0, byteCount);

                // if no bytes were read, we have reached EOF and the transfer is finished.
                if (byteCount == 0)
                {
                    working = false;
                }
            }
            inStream.Close();
            outStream.Close();
            MessageBox.Show(genericSuccess);
        }

        // encodes the data in 
        public void huffman(string inFile, string outFile)
        {
            // Initialize file I/O
            FileStream inStream = File.Open(inFile, FileMode.Open);
            int bufferSize = 128;
            byte[] buffer = new byte[bufferSize];
            FileStream outStream = File.Open(outFile, FileMode.Create);

            // We want to read through the bytes and count the frequency of the bytes.
            int[] frequencyAnalysis = new int[256];

        }
    }
}