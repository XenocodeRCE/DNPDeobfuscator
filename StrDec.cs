using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Resources;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DNPD
{
    class StrDec
    {

        //return 
        public static void SetresName()
        {
            var module = Form1.module;
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    if (method.Body.Variables.Count != 0)
                    {
                        if (!method.IsStatic || !method.IsConstructor) continue;
                        var instr = method.Body.Instructions;
                        for (var i = 0; i < instr.Count - 1; i++)
                        {
                            if (instr[i].OpCode != OpCodes.Ldstr) continue;
                            ResourceHandler.Resname = instr[i].Operand.ToString();
                            ResourceHandler.ResourceHandlerz();
                        }
                    }
                }
            }
        }

        public static byte[] b64(string bbbb)
        {
            return Convert.FromBase64String(bbbb);
        }

        public static string Byte2Str(byte[] aaaa)
        {
            return Encoding.Unicode.GetString(aaaa);
        }


        public static string Int2Text(string t, int n)
        {
            string text = string.Empty;
            checked
            {
                int num = t.Length - 1;
                for (int i = 0; i <= num; i++)
                {
                    int utf = Convert.ToInt32(t[i]) ^ n;
                    text += char.ConvertFromUtf32(utf);
                }
                return text;
            }
        }

        public static int ResolveInt(string[] operands)
        {
            Stack<int> stack = new Stack<int>();
            checked
            {
                for (int i = 0; i < operands.Length; i++)
                {
                    string text = operands[i];
                    string left = text;
                    if (Operators.CompareString(left, "+", false) == 0)
                    {
                        stack.Push(stack.Pop() + stack.Pop());
                    }
                    else
                    {
                        if (Operators.CompareString(left, "-", false) == 0)
                        {
                            stack.Push(0 - stack.Pop() + stack.Pop());
                        }
                        else
                        {
                            if (Operators.CompareString(left, "*", false) == 0)
                            {
                                stack.Push(stack.Pop() * stack.Pop());
                            }
                            else
                            {
                                if (Operators.CompareString(left, "/", false) == 0)
                                {
                                    int num = stack.Pop();
                                    stack.Push((int)Math.Round((double)stack.Pop() / (double)num));
                                }
                                else
                                {
                                    if (Operators.CompareString(left, "sqrt", false) == 0)
                                    {
                                        stack.Push((int)Math.Round(Math.Sqrt((double)stack.Pop())));
                                    }
                                    else
                                    {
                                        stack.Push(int.Parse(text));
                                    }
                                }
                            }
                        }
                    }
                }
                if (stack.Count != 1)
                {
                    throw new ArgumentException("Unbalanced expression");
                }
                return stack.Pop();
            }
        }

        public static string[] DecryptExpression(string expression)
        {
            return expression.ToLower().Split(new char[]
               {
                   ','
               }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetStrInRes(string resdata, string resname)
        {
            ResourceManager resourceManager = new ResourceManager(resname, Form1.asm);
            string result = (string)resourceManager.GetObject(resdata);
            resourceManager.ReleaseAllResources();
            return result;
        }


        public static int DecryptResData(string TKRP, int iUYA)
        {
            string[] array = TKRP.Split(Strings.ChrW(iUYA));
            checked
            {
                int[] array2 = new int[array.Length - 1 - 1 + 1];
                int arg_3C_0 = 0;
                int num = array2.Length - 1;
                for (int i = arg_3C_0; i <= num; i++)
                {
                    array2[i] = int.Parse(array[i]);
                }
                return array2[array2[array2.Length - 1]];
            }
        }

        public static int DecryptDataNoRes(string string_0, int int_0)
        {
            try
            {
                char[] separator = new char[(-368 / sizeof(bool)) + ((0x200 - sizeof(long)) - ((-56 + sizeof(decimal)) + ((0x240 - sizeof(sbyte)) - 400)))];
                separator[(0x14b / sizeof(byte)) - ((0x35f - sizeof(decimal)) - ((0x200 - sizeof(byte)) + ((-302 - sizeof(long)) + ((-93 - sizeof(int)) + ((0x444 / sizeof(byte)) - ((0x473 + sizeof(byte)) - ((0x1dd - sizeof(long)) - ((0x314 + sizeof(bool)) - ((0x213 - sizeof(long)) + ((0x389 - sizeof(short)) - 0x286))))))))))] = Strings.ChrW(int_0);
                string[] strArray = string_0.Split(separator);
                int[] numArray = new int[((strArray.Length - 1) - 1) + 1];
                int num3 = numArray.Length - 1;
                for (int i = 0; i <= num3; i++)
                {
                    numArray[i] = int.Parse(strArray[i]);
                }
                return numArray[numArray[numArray.Length - 1]];
            }
            catch (Exception)
            {
                return 0;
            }

            
        }
    }



    public class ResourceHandler
    {
        private static Stream _stream;
        public static string Resname;

        public static void ResourceHandlerz()
        {
            if (_stream != null) return;
            var ressource = Form1.asm.GetManifestResourceStream(Resname);
            var buffer = Stream2Byte(ressource);
            _stream = new MemoryStream(buffer);
        }
        public static string GetStr(int p)
        {
            return new BinaryReader(_stream)
            {
                BaseStream =
                {
                    Position = p
                }
            }.ReadString();
        }
        private static byte[] Stream2Byte(Stream d)
        {
            byte[] result;
            try
            {
                result = GetString(new GZipStream(d, CompressionMode.Decompress, false), checked((int)d.Length));
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
        public static byte[] GetString(Stream ds, int c)
        {
            var num = 0;
            checked
            {
                byte[] result;
                try
                {
                    var array = new byte[] {};
                    while (true)
                    {
                        array = (byte[])Utils.CopyArray(array, new byte[num + c + 1]);
                        var num2 = ds.Read(array, num, c);
                        if (num2 == 0)
                        {
                            break;
                        }
                        num += num2;
                    }
                    array = (byte[])Utils.CopyArray(array, new byte[num - 1 + 1]);
                    result = array;
                }
                catch (Exception)
                {
                    result = null;
                }
                return result;
            }
        }


    }
}
