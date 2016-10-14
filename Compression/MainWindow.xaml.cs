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
                        case 1:
                            huffman(inputName, outputName);
                            break;
                    }
                }
                else
                {
                    switch (listBox.SelectedIndex)
                    {
                        case 0:
                            noCompress(inputName, outputName);
                            break;
                        case 1:
                            huffmanDecode(inputName, outputName);
                            break;
                    }
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

            int byteCount = 1;
            while (byteCount > 0)
            {
                byteCount = inStream.Read(buffer, 0, 128);
                outStream.Write(buffer, 0, byteCount);
            }
            inStream.Close();
            outStream.Close();
            MessageBox.Show(genericSuccess);
        }

        // Modified from https://www.programmingalgorithms.com/algorithm/huffman-compress?lang=C%23
        public void huffman(string inFile, string outFile)
        {
            // Initialize file I/O
            FileStream inStream = File.Open(inFile, FileMode.Open);
            int bufferSize = 128;
            byte[] buffer = new byte[bufferSize];
            //FileStream outStream = File.Open(outFile, FileMode.Create);
            int byteCount = 1;

            // We want to read through the bytes and count the frequency of the bytes.
            int[] frequencyAnalysis = new int[256];
            while (byteCount > 0)
            {
                byteCount = inStream.Read(buffer, 0, 128);
                for (int i = 0; i < byteCount; i++)
                {
                    frequencyAnalysis[buffer[i]]++;
                }
            }
            inStream.Close();

            structures.quicksort.sort(frequencyAnalysis);


            inStream = File.Open(inFile, FileMode.Open);
            byte[] inBytes = new byte[inStream.Length];
            for (int x = 0; x < inStream.Length; x++)
            {
                inStream.Read(inBytes, x, 1);
            }

            uint originalSize = (uint)inBytes.Length;
            byte[] compressedData = new byte[originalSize * (101 / 100) + 320];
            int compressedDataSize = structures.huffman.Compress(inBytes, compressedData, originalSize);

            FileStream outStream = File.Open(outFile, FileMode.Create);
            byte[] originalLength = new byte[8];
            originalLength = BitConverter.GetBytes(inStream.Length);


            //MessageBox.Show(inStream.Length.ToString());
            
                
                outStream.Write(originalLength, 0, 8);
            for (int x = 0; x < compressedDataSize; x++)
            {
                outStream.Write(compressedData, x, 1);
            }
            inStream.Close();
            outStream.Close();
            MessageBox.Show("Operation successful.");

            /*
            string analysisOut = "";
            for (int i = 0; i < 256; i++)
            {
                if (frequencyAnalysis[i] > 0)
                {
                    analysisOut += i.ToString("X2") + ":" + frequencyAnalysis[i] + " ";
                }
            }
            MessageBox.Show(analysisOut);
            */
        }

        // Modified from https://www.programmingalgorithms.com/algorithm/huffman-decompress
        public void huffmanDecode(string inFile, string outFile)
        {
            FileStream inStream = File.Open(inFile, FileMode.Open);
            byte[] originalSizeB = new byte[8];
            inStream.Read(originalSizeB, 0, 8);

            MessageBox.Show(BitConverter.ToString(originalSizeB));
            Int64 originalDataSize = BitConverter.ToInt64(originalSizeB, 0);

            uint compressedDataSize = (uint)inStream.Length - 8;

            byte[] compressedData = new byte[compressedDataSize];
            for(int x = 0; x < compressedDataSize; x++)
            {
                inStream.Read(compressedData, x, 1);
            }
            inStream.Close();
            byte[] decompressedData = new byte[originalDataSize];
            
            MessageBox.Show(originalDataSize.ToString());

            structures.huffmanDecode.Decompress(compressedData, decompressedData, (uint)compressedDataSize, (uint)originalDataSize);

            FileStream outStream = File.Open(outFile, FileMode.Create);
            for (int x = 0; x < originalDataSize; x++)
            {
                outStream.Write(decompressedData, x, 1);
            }
            outStream.Close();

            MessageBox.Show("Operation Successful.");

        }
    }
}