using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compression
{
    class structures
    {
        public class quicksort
        {
            public static void sort(int[] array)
            {
                sort(array, 0, array.Length - 1);
            }

            public static void sort(int[] array, int lo, int hi)
            {
                if (hi <= lo)
                {
                    return;
                }
                int j = partition(array, lo, hi);
                sort(array, lo, j - 1);
                sort(array, j + 1, hi);
            }
            private static int partition(int[] array, int lo, int hi)
            {
                int i = lo, j = hi + 1;
                int v = array[lo];
                while (true)
                {
                    while (array[++i] < v)
                    {
                        if (i == hi)
                        {
                            break;
                        }
                    }
                    while (v < array[--j])
                    {
                        if (j == lo)
                        {
                            break;
                        }
                    }
                    if (i >= j)
                    {
                        break;
                    }
                    swap(array, i, j);
                }
                swap(array, lo, j);
                return j;
            }
            private static void swap(int[] array, int x, int y)
            {
                int z = array[x];
                array[x] = array[y];
                array[y] = z;
            }

        }


        public class huffmanBad
        {
            private int readIndex;
            private int writeIndex;
            public huffmanBad()
            {
                readIndex = 0;
                writeIndex = 0;
            }
            private node readTrie(byte[] bytestream)
            {
                if (bytestream[readIndex++] == 1)
                {
                    return new node(bytestream[readIndex++], 0, null, null);
                }
                return new node((byte)0, 0, readTrie(bytestream), readTrie(bytestream));
            }

            private void writeTrie(node x, byte[] bytestream)
            {
                if (x.isLeaf())
                {
                    bytestream[writeIndex++] = (byte)255;
                    bytestream[writeIndex++] = x.symbol;
                    return;
                }
                bytestream[writeIndex++] = (byte)0;
                writeTrie(x.left, bytestream);
                writeTrie(x.right, bytestream);

            }

            private class node
            {
                public byte symbol;
                public int freq;
                public node left, right;
                public node(byte symbol, int freq, node left, node right)
                {
                    this.symbol = symbol;
                    this.freq = freq;
                    this.left = left;
                    this.right = right;
                }
                public bool isLeaf()
                {
                    return (left == null && right == null);
                }
                public int compareTo(node that)
                {
                    return this.freq - that.freq;
                }
            }

            private class minPQ
            {
                private node[] pq;
                private int N;
                public minPQ(int maxN)
                {
                    pq = new node[maxN + 1];
                }
                public bool isEmpty()
                {
                    return N == 0;
                }
                public int size()
                {
                    return N;
                }
                public void insert(node v)
                {
                    pq[++N] = v;
                    swim(N);
                }
                public node delMin()
                {
                    node min = pq[1];
                    exch(1, N--);
                    pq[N + 1] = null;
                    sink(1);
                    return min;
                }
                private bool less(int i, int j)
                {
                    return pq[i].compareTo(pq[j]) > 0;
                }
                private void exch(int i, int j)
                {
                    node t = pq[i];
                    pq[i] = pq[j];
                    pq[j] = t;
                }
                private void swim(int k)
                {
                    while (k > 1 && less(k / 2, k))
                    {
                        exch(k / 2, k);
                        k = k / 2;
                    }
                }
                private void sink(int k)
                {
                    while (2 * k <= N)
                    {
                        int j = 2 * k;
                        if (j < N && less(j, j + 1))
                        {
                            j++;
                        }
                        if (!less(k, j))
                        {
                            break;
                        }
                        exch(k, j);
                        k = j;
                    }
                }
            }

            private node buildTrie(int[] freq)
            {
                minPQ pq = new minPQ(freq.Length);
                for (int i = 0; i < freq.Length; i++)
                {
                    if (freq[i] > 0)
                    {
                        pq.insert(new node((byte)i, freq[i], null, null));
                    }
                }
                while (pq.size() > 1)
                {
                    node x = pq.delMin();
                    node y = pq.delMin();
                    node parent = new node((byte)0, (x.freq + y.freq), x, y);
                    pq.insert(parent);
                }
                return pq.delMin();
            }

            public void expand(byte[] bytestreamIn, byte[] bytestreamOut)
            {
                int expandIndex = 0;
                int writeIndex = 0;
                node root = readTrie(bytestreamIn);
                int N = bytestreamIn[expandIndex++];
                for (int i = 0; i < N; i++)
                {
                    node x = root;
                    while (!x.isLeaf())
                    {
                        if (bytestreamIn[expandIndex++] != (byte)0)
                        {
                            x = x.right;
                        }
                        else
                        {
                            x = x.left;
                        }
                    }
                    bytestreamOut[writeIndex++] = x.symbol;
                }
            }
        }

        public class huffman
        {
            private const int MAX_TREE_NODES = 511;

            public class BitStream
            {
                public byte[] BytePointer;
                public uint BitPosition;
                public uint Index;
            }

            public struct Symbol
            {
                public int Sym;
                public uint Count;
                public uint Code;
                public uint Bits;
            }

            public class EncodeNode
            {
                public EncodeNode ChildA;
                public EncodeNode ChildB;
                public int Count;
                public int Symbol;
            }

            private static void initBitstream(ref BitStream stream, byte[] buffer)
            {
                stream.BytePointer = buffer;
                stream.BitPosition = 0;
            }

            private static void writeBits(ref BitStream stream, uint x, uint bits)
            {
                byte[] buffer = stream.BytePointer;
                uint bit = stream.BitPosition;
                uint mask = (uint)(1 << (int)(bits - 1));

                for (uint count = 0; count < bits; ++count)
                {
                    buffer[stream.Index] = (byte)((buffer[stream.Index] & (0xff ^ (1 << (int)(7 - bit)))) + ((Convert.ToBoolean(x & mask) ? 1 : 0) << (int)(7 - bit)));
                    x <<= 1;
                    bit = (bit + 1) & 7;

                    if (!Convert.ToBoolean(bit))
                    {
                        ++stream.Index;
                    }
                }

                stream.BytePointer = buffer;
                stream.BitPosition = bit;
            }

            private static void histogram(byte[] input, Symbol[] sym, uint size)
            {
                int i;
                int index = 0;

                for (i = 0; i < 256; ++i)
                {
                    sym[i].Sym = i;
                    sym[i].Count = 0;
                    sym[i].Code = 0;
                    sym[i].Bits = 0;
                }

                for (i = (int)size; Convert.ToBoolean(i); --i, ++index)
                {
                    sym[input[index]].Count++;
                }
            }

            private static void storeTree(ref EncodeNode node, Symbol[] sym, ref BitStream stream, uint code, uint bits)
            {
                uint symbolIndex;

                if (node.Symbol >= 0)
                {
                    writeBits(ref stream, 1, 1);
                    writeBits(ref stream, (uint)node.Symbol, 8);

                    for (symbolIndex = 0; symbolIndex < 256; ++symbolIndex)
                    {
                        if (sym[symbolIndex].Sym == node.Symbol)
                            break;
                    }

                    sym[symbolIndex].Code = code;
                    sym[symbolIndex].Bits = bits;
                    return;
                }
                else
                {
                    writeBits(ref stream, 0, 1);
                }

                storeTree(ref node.ChildA, sym, ref stream, (code << 1) + 0, bits + 1);
                storeTree(ref node.ChildB, sym, ref stream, (code << 1) + 1, bits + 1);
            }

            private static void makeTree(Symbol[] sym, ref BitStream stream)
            {
                EncodeNode[] nodes = new EncodeNode[MAX_TREE_NODES];

                for (int counter = 0; counter < nodes.Length; ++counter)
                {
                    nodes[counter] = new EncodeNode();
                }

                EncodeNode node1, node2, root;
                uint i, numSymbols = 0, nodesLeft, nextIndex;

                for (i = 0; i < 256; ++i)
                {
                    if (sym[i].Count > 0)
                    {
                        nodes[numSymbols].Symbol = sym[i].Sym;
                        nodes[numSymbols].Count = (int)sym[i].Count;
                        nodes[numSymbols].ChildA = null;
                        nodes[numSymbols].ChildB = null;
                        ++numSymbols;
                    }
                }

                root = null;
                nodesLeft = numSymbols;
                nextIndex = numSymbols;

                while (nodesLeft > 1)
                {
                    node1 = null;
                    node2 = null;

                    for (i = 0; i < nextIndex; ++i)
                    {
                        if (nodes[i].Count > 0)
                        {
                            if (node1 == null || (nodes[i].Count <= node1.Count))
                            {
                                node2 = node1;
                                node1 = nodes[i];
                            }
                            else if (node2 == null || (nodes[i].Count <= node2.Count))
                            {
                                node2 = nodes[i];
                            }
                        }
                    }

                    root = nodes[nextIndex];
                    root.ChildA = node1;
                    root.ChildB = node2;
                    root.Count = node1.Count + node2.Count;
                    root.Symbol = -1;
                    node1.Count = 0;
                    node2.Count = 0;
                    ++nextIndex;
                    --nodesLeft;
                }

                if (root != null)
                {
                    storeTree(ref root, sym, ref stream, 0, 0);
                }
                else
                {
                    root = nodes[0];
                    storeTree(ref root, sym, ref stream, 0, 1);
                }
            }

            public static int Compress(byte[] input, byte[] output, uint inputSize)
            {
                Symbol[] sym = new Symbol[256];
                Symbol temp;
                BitStream stream = new BitStream();
                uint i, totalBytes, swaps, symbol;

                if (inputSize < 1)
                    return 0;

                initBitstream(ref stream, output);
                histogram(input, sym, inputSize);
                makeTree(sym, ref stream);

                do
                {
                    swaps = 0;

                    for (i = 0; i < 255; ++i)
                    {
                        if (sym[i].Sym > sym[i + 1].Sym)
                        {
                            temp = sym[i];
                            sym[i] = sym[i + 1];
                            sym[i + 1] = temp;
                            swaps = 1;
                        }
                    }
                } while (Convert.ToBoolean(swaps));

                for (i = 0; i < inputSize; ++i)
                {
                    symbol = input[i];
                    writeBits(ref stream, sym[symbol].Code, sym[symbol].Bits);
                }

                totalBytes = stream.Index;

                if (stream.BitPosition > 0)
                {
                    ++totalBytes;
                }

                return (int)totalBytes;
            }
        }

        public class huffmanDecode
        {
            private const int MAX_TREE_NODES = 511;

            public class BitStream
            {
                public byte[] BytePointer;
                public uint BitPosition;
                public uint Index;
            }

            public struct Symbol
            {
                public int Sym;
                public uint Count;
                public uint Code;
                public uint Bits;
            }

            public class DecodeNode
            {
                public DecodeNode ChildA;
                public DecodeNode ChildB;
                public int Symbol;
            }

            private static void initBitstream(ref BitStream stream, byte[] buffer)
            {
                stream.BytePointer = buffer;
                stream.BitPosition = 0;
            }

            private static uint readBit(ref BitStream stream)
            {
                byte[] buffer = stream.BytePointer;
                uint bit = stream.BitPosition;

                uint x = (uint)(Convert.ToBoolean((buffer[stream.Index] & (1 << (int)(7 - bit)))) ? 1 : 0);
                bit = (bit + 1) & 7;

                if (!Convert.ToBoolean(bit))
                {
                    ++stream.Index;
                }

                stream.BitPosition = bit;

                return x;
            }

            private static uint read8Bits(ref BitStream stream)
            {
                byte[] buffer = stream.BytePointer;
                uint bit = stream.BitPosition;
                uint x = (uint)((buffer[stream.Index] << (int)bit) | (buffer[stream.Index + 1] >> (int)(8 - bit)));
                ++stream.Index;

                return x;
            }

            private static DecodeNode recoverTree(DecodeNode[] nodes, ref BitStream stream, ref uint nodenum)
            {
                DecodeNode thisNode;

                thisNode = nodes[nodenum];
                nodenum = nodenum + 1;
                thisNode.Symbol = -1;
                thisNode.ChildA = null;
                thisNode.ChildB = null;

                if (Convert.ToBoolean(readBit(ref stream)))
                {
                    thisNode.Symbol = (int)read8Bits(ref stream);
                    return thisNode;
                }

                thisNode.ChildA = recoverTree(nodes, ref stream, ref nodenum);
                thisNode.ChildB = recoverTree(nodes, ref stream, ref nodenum);

                return thisNode;
            }

            public static void Decompress(byte[] input, byte[] output, uint inputSize, uint outputSize)
            {
                DecodeNode[] nodes = new DecodeNode[MAX_TREE_NODES];

                for (int counter = 0; counter < nodes.Length; ++counter)
                {
                    nodes[counter] = new DecodeNode();
                }

                DecodeNode root, node;
                BitStream stream = new BitStream();
                uint i, nodeCount;
                byte[] buffer;

                if (inputSize < 1)
                    return;

                initBitstream(ref stream, input);
                nodeCount = 0;
                root = recoverTree(nodes, ref stream, ref nodeCount);
                buffer = output;

                for (i = 0; i < outputSize; ++i)
                {
                    node = root;

                    while (node.Symbol < 0)
                    {
                        if (Convert.ToBoolean(readBit(ref stream)))
                            node = node.ChildB;
                        else
                            node = node.ChildA;
                    }

                    buffer[i] = (byte)node.Symbol;
                }
            }
        }

        
    }
}