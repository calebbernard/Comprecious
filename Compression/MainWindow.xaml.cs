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

        // encodes the data in 
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
    }

    public class node
    {
        private node left;
        private node right;
        private int data;

        public node getLeft()
        {
            return left;
        }
        public void setLeft(node addNode)
        {
            left = addNode;
        }
        public node getRight()
        {
            return right;
        }
        public void setRight(node addNode)
        {
            right = addNode;
        }
        public int getData()
        {
            return data;
        }
        public void setData(int newData)
        {
            data = newData;
        }
        public node()
        {
            left = null;
            right = null;
            data = 0;
        }
    }

    public class binaryTree
    {
        private node root;
        private node current;
        public int getCurrent()
        {
            return current.getData();
        }
        public void setCurrent(int data)
        {
            current.setData(data);
        }
        public bool hasLeft()
        {
            if (current.getLeft() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool hasRight()
        {
            if (current.getRight() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void addLeft(node newNode)
        {
            current.setLeft(newNode);
        }
        public void addRight(node newNode)
        {
            current.setRight(newNode);
        }
        public void left()
        {
            current = current.getLeft();
        }
        public void right()
        {
            current = current.getRight();
        }
        public binaryTree()
        {
            root = new node();
        }
    }
    
}