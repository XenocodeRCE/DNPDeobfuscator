using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OpCodes = dnlib.DotNet.Emit.OpCodes;

namespace DNPD
{
    internal class Helpers
    {
        //Declaration
        public static List<MethodDef> StrDecMeth = new List<MethodDef>();
        public static List<MethodDef> IntDecMeth = new List<MethodDef>();
        public static List<MethodDef> RemainingIntsMeth = new List<MethodDef>();
        public static List<TypeDef> Type2Remove = new List<TypeDef>();
        //Antis
        public static void GetAntitamper(ModuleDefMD module)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {

                    if (!method.HasBody) continue;
                    var local = method.Body.Variables;
                    /*
                        [0] class [mscorlib]System.IO.Stream,
		                [1] string,
		                [2] string,
		                [3] string,
		                [4] class [mscorlib]System.IO.BinaryReader
                    */
                    if (local.Count != 5) continue;
                    if (!local[0].Type.FullName.ToLower().Contains("stream")) continue;
                    if (!local[4].Type.FullName.ToLower().Contains("binaryreader")) continue;
                    Form1.AntitampMethodDef = method;
                    break;
                }
            }
        }

        public static void GetAntidebug(ModuleDefMD module)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    var local = method.Body.Variables;
                    /*
                       		[0] class [mscorlib]System.Threading.Thread,
		                    [1] class [mscorlib]System.Threading.Thread
                    */
                    if (local.Count != 2) continue;
                    if (!local[0].Type.FullName.ToLower().Contains("thread")) continue;
                    if (!local[1].Type.FullName.ToLower().Contains("thread")) continue;
                    Form1.AntidebugMethodDef = method;
                    break;
                }
            }
        }

        public static void GetAntidump(ModuleDefMD module)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;

                    var local = method.Body.Variables;
                    /*
                       				[0] uint32,
		                            [1] uint8*,
		                            [2] uint8*,
		                            [3] uint16,
		                            [4] uint16,
		                            [5] uint8*,
		                            [6] uint8*,
		                            [7] uint8*,
		                            [8] uint8*,
		                            [9] uint8*,
		                            [10] uint8*,
		                            [11] int32,
		                            [12] int32,
		                            [13] int32,
		                            [14] uint8*,
		                            [15] uint16,
		                            [16] int32,
		                            [17] int32,
		                            [18] uint32,
		                            [19] uint32,
		                            [20] uint32[],
		                            [21] uint32[],
		                            [22] uint32[],
		                            [23] int32,
		                            [24] int32,
		                            [25] uint8*,
		                            [26] uint32,
		                            [27] int32,
		                            [28] uint8*,
		                            [29] uint32,
		                            [30] int32,
		                            [31] uint32,
		                            [32] int32,
		                            [33] int32,
		                            [34] int32,
		                            [35] int32,
		                            [36] uint8*,
		                            [37] uint32,
		                            [38] int32,
		                            [39] uint8*,
		                            [40] uint16,
		                            [41] int32,
		                            [42] int32
                    */
                    if (local.Count == 43)
                    {
                        Form1.AntidumpMethodDef = method;

                    }
                    break;
                }
            }
        }




        //String Decryption
        public static void NopCall(ModuleDefMD module, MethodDef calledMethod)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    var instr = method.Body.Instructions;
                    for (var i = 0; i < instr.Count - 1; i++)
                    {
                        var operand = instr[i].Operand;
                        if (operand != null && operand.ToString().ToLower().Contains(calledMethod.Name.ToLower()))
                        {
                            instr[i].OpCode = OpCodes.Nop;
                        }
                    }
                }
            }
        }

        public static string DecryptStr(ModuleDefMD module, int param1, int param2, MethodDef encMethodDef)
        {
            /*
            return Byte2Str(b64(Int2Text(ResourceHandler.GetStr(0), 53)));
            */
            StrDec.SetresName();
            var step1 = ResourceHandler.GetStr(param1);
            var step2 = StrDec.Int2Text(step1, param2);
            var step3 = StrDec.b64(step2);
            var decryptedstring = StrDec.Byte2Str(step3);

            return decryptedstring;
        }

        public static void GetCall(ModuleDefMD module, MethodDef inputmethod, string decryptedstring)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    var instr = method.Body.Instructions;
                    for (var i = 0; i < instr.Count - 1; i++)
                    {
                        if (instr[i].OpCode.Code != Code.Call) continue;
                        if (!instr[i].Operand.ToString().ToLower().Contains(inputmethod.Name.ToLower())) continue;
                        if (instr[i].Operand.ToString().ToLower().Contains("system.windows.forms")) continue;
                        if (instr[i].Operand != inputmethod) continue;
                        instr[i].OpCode = OpCodes.Ldstr;
                        instr[i].Operand = decryptedstring;
                        Form1.DeobedString += 1;
                        if (!StrDecMeth.Contains(inputmethod))
                        {
                            StrDecMeth.Add(inputmethod);
                        }
                    }
                }
            }
        }

        public static void GetStrDecMeth(ModuleDefMD module)
        {
            //Check if it has ResEncryted str dec meth
            if (IsResEncrypted(module))
            {
                foreach (TypeDef type in module.Types)
                {
                    foreach (MethodDef method in type.Methods)
                    {
                        if (method.HasBody)
                        {
                            if (method.Body.Variables.Count == 1)
                            {
                                /*
                                [0] string
                                */
                                if (method.ReturnType.FullName.ToLower().Contains("string"))
                                {
                                    /*
                                    [mscorlib]System.String
                                     */
                                    if (method.IsHideBySig && method.IsStatic && !method.HasParamDefs /*&&
                                        method.Body.Instructions.Count > 5*/)
                                    {

                                        var instr = method.Body.Instructions;

                                        if (instr[0].OpCode == OpCodes.Ldstr)
                                        {
                                            if (instr[1].OpCode.Code.ToString().ToLower().Contains("ldc"))
                                            {
                                                var param1 = instr[0].Operand.ToString();
                                                var param2 = instr[1].GetLdcI4Value();
                                                var decryptedstring =
                                                    StrDec.Byte2Str(StrDec.b64(StrDec.Int2Text(param1, param2)));
                                                GetCall(module, method, decryptedstring);

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (TypeDef type in module.Types)
                {
                    foreach (MethodDef method in type.Methods)
                    {
                        if (method.HasBody)
                        {
                            if (method.Body.Variables.Count == 1)
                            {
                                /*
                                [0] string
                                */
                                if (method.ReturnType.FullName.ToLower().Contains("string"))
                                {
                                    /*
                                    [mscorlib]System.String
                                     */
                                    if (method.IsHideBySig && method.IsStatic && !method.HasParamDefs /*&&
                                        method.Body.Instructions.Count > 7*/)
                                    {
                                        var instr = method.Body.Instructions;

                                        if (instr[0].OpCode == OpCodes.Call)
                                        {
                                            //HideCall is enabled :/ 
                                            var callmethod1 = instr[0].Operand.ToString();
                                            callmethod1 = callmethod1.Replace("System.String ", "");
                                            callmethod1 = callmethod1.Replace("()", "");
                                            string[] xeno = Regex.Split(callmethod1, "::");
                                            //0 -> type
                                            //1 -> method
                                            var a2 = GetStrCallValue1(module, xeno[1]);
                                            if (a2 == null)
                                            {
                                                MessageBox.Show("{Error} Fix HideCall line 264 in Helpers.cs : Cannot fetch String value.");
                                                goto MoveOnStrCallHidden;
                                            }

                                            var callmethod2 = instr[1].Operand.ToString();
                                            callmethod2 = callmethod2.Replace("System.Int32 ", "");
                                            callmethod2 = callmethod2.Replace("()", "");
                                            string[] xeno2 = Regex.Split(callmethod2, "::");
                                            //0 -> type
                                            //1 -> method
                                            var b2 = GetStrCallValue2(module, xeno2[1]);
                                            if (b2 == 0)
                                            {
                                                MessageBox.Show("{Error} Fix HideCall line 353 in Helpers.cs : Cannot fetch Int value.");
                                                goto MoveOnStrCallHidden;
                                            }

                                            try
                                            {
                                                var decryptedstringz = StrDec.Byte2Str(StrDec.b64(StrDec.Int2Text(a2, b2)));
                                                GetCall(module, method, decryptedstringz);
                                                goto MoveOnStrCallHidden;
                                            }
                                            catch (Exception)
                                            {

                                                goto MoveOnStrCallHidden;
                                            }                                            
                                        }

                                        var param1 = instr[0].GetLdcI4Value();
                                        var param2 = instr[2].GetLdcI4Value();

                                        var decryptedstring = DecryptStr(module, param1, param2, method);
                                        GetCall(module, method, decryptedstring);
                                    MoveOnStrCallHidden:
                                        var uselessvar = 2;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public static string GetStrCallValue1(ModuleDefMD module, string inputmethod)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;

                    if (method.Name == inputmethod)
                    {
                        var instr = method.Body.Instructions;
                        var a =instr[0].Operand.ToString();
                        return a;
                    }
                }
            }
            return null;
        }

        public static int GetStrCallValue2(ModuleDefMD module, string inputmethod)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;

                    if (method.Name == inputmethod)
                    {
                        List<Instruction> test = new List<Instruction>();
                        var instrs = method.Body.Instructions;
                        for (var i = 0; i < instrs.Count ; i++)
                        {
                            if (instrs[i].OpCode == OpCodes.Nop)
                            {
                               instrs.RemoveAt(i);
                            }
                        }
                        for (var i = 0; i < instrs.Count; i++)
                        {
                            if (instrs[i].OpCode == OpCodes.Nop)
                            {
                                instrs.RemoveAt(i);
                            }
                        }
                        for (var i = 0; i < instrs.Count; i++)
                        {
                            if (instrs[i].OpCode == OpCodes.Nop)
                            {
                                instrs.RemoveAt(i);
                            }
                        }
                        for (var i = 0; i < instrs.Count; i++)
                        {
                            if (instrs[i].OpCode == OpCodes.Nop)
                            {
                                instrs.RemoveAt(i);
                            }
                        }
                        for (var i = 0; i < instrs.Count; i++)
                        {
                            if (instrs[i].OpCode == OpCodes.Nop)
                            {
                                instrs.RemoveAt(i);
                            }
                        }
                        for (var i = 0; i < instrs.Count; i++)
                        {
                            if (instrs[i].OpCode == OpCodes.Nop)
                            {
                                instrs.RemoveAt(i);
                            }
                        }
                        for (var i = 0; i < instrs.Count; i++)
                        {
                            if (instrs[i].OpCode == OpCodes.Nop)
                            {
                                instrs.RemoveAt(i);
                            }
                        }
                        var instr = method.Body.Instructions;

                        if (instr[0].IsLdcI4())
                        {
                            if (instr[1].OpCode == OpCodes.Ldc_I4)
                            {
                                if (instr[2].OpCode == OpCodes.Sub)
                                {
                                    if (instr[3].OpCode == OpCodes.Ldc_I4)
                                    {
                                        if (instr[4].OpCode == OpCodes.Sub)
                                        {
                                            var op11 = instr[0].GetLdcI4Value();
                                            var op22 = instr[1].GetLdcI4Value();
                                            var op33 = instr[3].GetLdcI4Value();
                                            var resulta = op11 - op22 - op33;
                                            return resulta;
                                        }
                                        if (instr[4].OpCode == OpCodes.Add)
                                        {
                                            var op11 = instr[0].GetLdcI4Value();
                                            var op22 = instr[1].GetLdcI4Value();
                                            var op33 = instr[3].GetLdcI4Value();
                                            var resulta = op11 - op22 + op33;
                                            return resulta;
                                        }
                                        if (instr[4].OpCode == OpCodes.Div)
                                        {
                                            var op11 = instr[0].GetLdcI4Value();
                                            var op22 = instr[1].GetLdcI4Value();
                                            var op33 = instr[3].GetLdcI4Value();
                                            var resulta = op11 - op22 / op33;
                                            return resulta;
                                        }
                                    }
                                    //Math Operation has been spotted ! 
                                    var op1 = instr[0].GetLdcI4Value();
                                    var op2 = instr[1].GetLdcI4Value();
                                    var result = op1 - op2;
                                    return result;
                                }
                                if (instr[2].OpCode == OpCodes.Add)
                                {
                                    if (instr[3].OpCode == OpCodes.Ldc_I4)
                                    {
                                        if (instr[4].OpCode == OpCodes.Sub)
                                        {
                                            var op11 = instr[0].GetLdcI4Value();
                                            var op22 = instr[1].GetLdcI4Value();
                                            var op33 = instr[3].GetLdcI4Value();
                                            var resulta = op11 + op22 - op33;
                                            return resulta;
                                        }
                                        if (instr[4].OpCode == OpCodes.Add)
                                        {
                                            var op11 = instr[0].GetLdcI4Value();
                                            var op22 = instr[1].GetLdcI4Value();
                                            var op33 = instr[3].GetLdcI4Value();
                                            var resulta = op11 + op22 + op33;
                                            return resulta;
                                        }
                                        if (instr[4].OpCode == OpCodes.Div)
                                        {
                                            var op11 = instr[0].GetLdcI4Value();
                                            var op22 = instr[1].GetLdcI4Value();
                                            var op33 = instr[3].GetLdcI4Value();
                                            var resulta = op11 + op22 / op33;
                                            return resulta;
                                        }
                                    }
                                    //Math Operation has been spotted ! 
                                    var op1 = instr[0].GetLdcI4Value();
                                    var op2 = instr[1].GetLdcI4Value();
                                    var result = op1 + op2;
                                    return result;

                                }
                                if (instr[2].OpCode == OpCodes.Div)
                                {
                                    if (instr[3].OpCode == OpCodes.Ldc_I4)
                                    {
                                        if (instr[4].OpCode == OpCodes.Sub)
                                        {
                                            var op11 = instr[0].GetLdcI4Value();
                                            var op22 = instr[1].GetLdcI4Value();
                                            var op33 = instr[3].GetLdcI4Value();
                                            var resulta = op11 - op22 - op33;
                                            return resulta;
                                        }
                                        if (instr[4].OpCode == OpCodes.Add)
                                        {
                                            var op11 = instr[0].GetLdcI4Value();
                                            var op22 = instr[1].GetLdcI4Value();
                                            var op33 = instr[3].GetLdcI4Value();
                                            var resulta = op11 - op22 + op33;
                                            return resulta;
                                        }
                                        if (instr[4].OpCode == OpCodes.Div)
                                        {
                                            var op11 = instr[0].GetLdcI4Value();
                                            var op22 = instr[1].GetLdcI4Value();
                                            var op33 = instr[3].GetLdcI4Value();
                                            var resulta = op11 - op22 / op33;
                                            return resulta;
                                        }
                                    }
                                    //Math Operation has been spotted ! 
                                    var op1 = instr[0].GetLdcI4Value();
                                    var op2 = instr[1].GetLdcI4Value();
                                    var result = op1 / op2;
                                    return result;
                                }
                            }
                            var a = instr[0].GetLdcI4Value();
                            return a;
                        }
                        return 0;          
                    }
                }
            }
            return 0;
        }
     
        public static bool IsResEncrypted(ModuleDefMD module)
        {
            int numberOfMethods = 0;
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (method.HasBody)
                    {
                        if (method.Body.Variables.Count == 1)
                        {
                            /*
                            [0] string
                            */
                            if (method.ReturnType.FullName.ToLower().Contains("string"))
                            {
                                /*
                                [mscorlib]System.String
                                 */
                                if (method.IsHideBySig && method.IsStatic && !method.HasParamDefs &&
                                    method.Body.Instructions.Count > 5)
                                {

                                    var instr = method.Body.Instructions;
                                    if (instr[0].OpCode == OpCodes.Ldstr)
                                    {
                                        if (instr[1].OpCode.Code.ToString().ToLower().Contains("ldc"))
                                        {
                                            numberOfMethods += 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return numberOfMethods > 2;
        }

       

        //Integer Decryption
        public static void ResolveMathPow(ModuleDefMD module)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (method.HasBody)
                    {
                        if (method.Body.Variables.Count == 1)
                        {
                            /*
                            [0] int32
                            */
                            if (method.ReturnType.FullName.ToLower().Contains("int32"))
                            {
                                /*
                                System.Int32 ()
                                 */
                                if (method.IsHideBySig && method.IsStatic && !method.HasParamDefs &&
                                    method.Body.Instructions.Count < 10)
                                {
                                    var instr = method.Body.Instructions;
                                    if (instr[3].OpCode == OpCodes.Call)
                                    {
                                        /*
                                         float64 [mscorlib]System.Math::Pow(float64, float64)
                                         */
                                        if (instr[2].Operand.ToString().ToLower().Contains("pow"))
                                        {

                                            if (instr[0].OpCode == OpCodes.Call)
                                            {
                                                //HideCall is enabled :/ 
                                                var callmethod1 = instr[0].Operand.ToString();
                                                callmethod1 = callmethod1.Replace("System.Double ", "");
                                                callmethod1 = callmethod1.Replace("()", "");
                                                string[] xeno = Regex.Split(callmethod1, "::");
                                                //0 -> type
                                                //1 -> method
                                                var a2 = GetDoubleValue(module, xeno[1]);
                                                if (a2 == 0)
                                                {
                                                    MessageBox.Show("{Error} Fix HideCall line 353 in Helpers.cs : Cannot fetch Double value.");
                                                    goto MoveOnCallHidden;
                                                }

                                                var callmethod2 = instr[1].Operand.ToString();
                                                callmethod2 = callmethod2.Replace("System.Double ", "");
                                                callmethod2 = callmethod2.Replace("()", "");
                                                string[] xeno2 = Regex.Split(callmethod2, "::");
                                                //0 -> type
                                                //1 -> method
                                                var b2 = GetDoubleValue(module, xeno2[1]);
                                                if (b2 == 0)
                                                {
                                                    MessageBox.Show("{Error} Fix HideCall line 353 in Helpers.cs : Cannot fetch Double value.");
                                                    goto MoveOnCallHidden;
                                                }

                                                var decryptedintz = Convert.ToInt32(Math.Pow(a2, b2));
                                                GetCallInt(module, method, decryptedintz);
                                                goto MoveOnCallHidden;
                                            }

                                            var param1 = instr[0].Operand.ToString();
                                            var param2 = instr[1].Operand.ToString();
                                            var a = (float) Convert.ToDouble(param1);
                                            var b = (float) Convert.ToDouble(param2);
                                            var decryptedint = Convert.ToInt32(Math.Pow(a, b));
                                            GetCallInt(module, method, decryptedint);
                                           MoveOnCallHidden :
                                            var uselessvar = 2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static float GetDoubleValue(ModuleDefMD module, string inputmethod)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;

                    if (method.Name == inputmethod)
                    {
                        var instr = method.Body.Instructions;
                        var a = (float)Convert.ToDouble(instr[0].Operand.ToString());
                        return a;
                    }
                }
            }
            return 0;
        }

        public static void GetCallInt(ModuleDefMD module, MethodDef inputmethod, int decryptedint)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    var instr = method.Body.Instructions;
                    for (var i = 0; i < instr.Count - 1; i++)
                    {
                        if (instr[i].OpCode.Code != Code.Call) continue;
                        if (!instr[i].Operand.ToString().ToLower().Contains(inputmethod.Name.ToLower())) continue;
                        if (instr[i].Operand.ToString().ToLower().Contains("system.windows.forms")) continue;
                        if (instr[i].Operand != inputmethod) continue;
                        instr[i].OpCode = OpCodes.Ldc_I4_S;
                        instr[i].Operand = (byte)decryptedint;
                        Form1.DeobedInts += 1;
                        /*
                         
                         Before :
                         instr[i].Operand = {System.Int32 BasicCrackMe.Form1::hNz()}
                         instr[i].OpCode.Code == Code.Call
                         
                         After:
                         instr[i].Operand = 0x15
                         instr[i].OpCode.Code == Code.Ldc_I4_S   
                         
                         */
                        if (!IntDecMeth.Contains(inputmethod))
                        {
                            IntDecMeth.Add(inputmethod);
                        }
                    }
                }
            }
        }

        public static string ToHex(int value)
        {
            return String.Format("0x{0:X}", value);
        }
        
        public static void ResolveLastInt(ModuleDefMD module)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    var instr = method.Body.Instructions;
                    for (var i = 0; i < instr.Count - 1; i++)
                    {
                        if (instr[i].OpCode == OpCodes.Ldstr)
                        {
                            if(instr[i].Operand.ToString().Contains("Unbalanced expression"))
                            {
                                //[0] class [System]System.Collections.Generic.Stack`1<int32>,
		                        //[1] string[],
		                        //[2] int32,
		                        //[3] string,
		                        //[4] string,
		                        //[5] int32
                                if (method.Body.Variables.Count == 6)
                                {
                                    GetCallLastInt(module, method);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void GetCallLastInt(ModuleDefMD module, MethodDef inputmethod)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    var instr = method.Body.Instructions;
                    for (var i = 0; i < instr.Count - 1; i++)
                    {
                        if (instr[i].OpCode.Code != Code.Call) continue;
                        if (!instr[i].Operand.ToString().ToLower().Contains(inputmethod.Name.ToLower())) continue;
                        if (instr[i].Operand.ToString().ToLower().Contains("system.windows.forms")) continue;
                        if (instr[i].Operand != inputmethod) continue;
                        var int2decrypt = instr[0].Operand.ToString();
                        var decryptedint = StrDec.ResolveInt(StrDec.DecryptExpression(int2decrypt));
                        GetCallMethLastInt(module, method, decryptedint);
                        if (!RemainingIntsMeth.Contains(method))
                        {
                            RemainingIntsMeth.Add(method);
                        }
                    }
                }
            }
        }

        public static void GetCallMethLastInt(ModuleDefMD module, MethodDef inputmethod, int decryptedint)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    var instr = method.Body.Instructions;
                    for (var i = 0; i < instr.Count - 1; i++)
                    {
                        if (instr[i].OpCode.Code != Code.Call) continue;
                        if (!instr[i].Operand.ToString().ToLower().Contains(inputmethod.Name.ToLower())) continue;
                        if (instr[i].Operand.ToString().ToLower().Contains("system.windows.forms")) continue;
                        if (instr[i].Operand != inputmethod) continue;
                        instr[i].OpCode = OpCodes.Ldc_I4;
                        instr[i].Operand =decryptedint;
                        Form1.DeobedInts += 1;
                    }
                }
            }
        }

        public static void ExtractIntFromRes(ModuleDefMD module)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (method.HasBody)
                    {
                        if (method.Body.Variables.Count != 2) continue;
                        /*
                    		[0] class [mscorlib]System.Resources.ResourceManager,
		                    [1] string
                            */
                        var vari = method.Body.Variables;
                        if (!vari[0].Type.ToString().ToLower().Contains("resourcemanager")) continue;
                        if (method.ReturnType.FullName.ToLower().Contains("string"))
                        {
                            /*
                                [mscorlib]System.String
                                 */
                            if (method.IsStatic && method.HasParamDefs /*&&
                                        method.Body.Instructions.Count > 5*/)
                            {

                                var instr = method.Body.Instructions;

                                if (instr[0].OpCode == OpCodes.Ldstr)
                                {
                                    if (instr[3].OpCode == OpCodes.Ldstr && instr[3].Operand.ToString() == "GetExecutingAssembly")
                                    {
                                        string resindexname = null;
                                        int decryptionkey = 0;
                                        //"GetExecutingAssembly"

                                        var resourcename = instr[0].Operand.ToString();

                                        RecoverCall(module, method, resourcename);


                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void RecoverCall(ModuleDefMD module, MethodDef method, string resourcename)
        {
            foreach (TypeDef type2 in module.Types)
            {
                foreach (MethodDef method2 in type2.Methods)
                {
                    if (!method2.HasBody) continue;
                    var instr2 = method2.Body.Instructions;
                    for (var i = 0; i < instr2.Count - 1; i++)
                    {
                        /*public static int lywZ()
                        {
	                        return ndxH.WVak(pkvC.bAqA("HGG"), 8203);
                        }*/
                        if (instr2[i].OpCode.Code != Code.Call) continue;
                        if (instr2[i].Operand != method) continue;
                        //
                        var resindexname = instr2[0].Operand.ToString();
                        var decryptionkey = instr2[2].GetLdcI4Value();

                        if (!RemainingIntsMeth.Contains(method2))
                        {
                            RemainingIntsMeth.Add(method2);
                        }
                        RecoverInt(module, method2, resourcename, resindexname, decryptionkey);
                    }
                }
            }
        }

        public static void RecoverInt(ModuleDefMD module, MethodDef method2, string resourcename, string resindexname, int decryptionkey)
        {
            foreach (Resource res in module.Resources)
            {
                if (res.Name.Contains(resourcename))
                {
                    var decryptedint = StrDec.DecryptResData(StrDec.GetStrInRes(resindexname, resourcename), decryptionkey);

                    foreach (TypeDef type3 in module.Types)
                    {
                        foreach (MethodDef method3 in type3.Methods)
                        {
                            if (!method3.HasBody) continue;
                            var instr3 = method3.Body.Instructions;
                            for (var i = 0; i < instr3.Count - 1; i++)
                            {
                                /*public static int lywZ()
                                {
                                  return ndxH.WVak(pkvC.bAqA("HGG"), 8203);
                                }*/
                                if (instr3[i].OpCode.Code != Code.Call) continue;
                                if (instr3[i].Operand != method2) continue;
                                //
                                instr3[i].OpCode = OpCodes.Ldc_I4;
                                instr3[i].Operand = decryptedint;
                                Form1.DeobedInts += 1;
                            }
                        }
                    }
                }
            }
        }


        /*
         * Not needed thanks to CodeCracker's tools.
         */
        public static void SizeOfCalc(ModuleDefMD module)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (method.HasBody)
                    {
                        if (method.Body.Variables.Count == 1)
                        {
                            /*
                                [0] string
                                */
                            if (method.ReturnType.FullName.ToLower().Contains("string"))
                            {
                                /*
                                    [mscorlib]System.String
                                     */
                                if (method.IsHideBySig && method.IsStatic && !method.HasParamDefs /*&&
                                        method.Body.Instructions.Count > 7*/)
                                {
                                    var instr = method.Body.Instructions;
                                    //Check if it uses NumericEncoding
                                    for (var i = 0; i < instr.Count - 1; i++)
                                    {
                                        /*
                                             * Integer = 4
                                             * SByte = 1
                                             * Byte = 1
                                             * Boolean = 1
                                             * Decimal = 16
                                             * Short = 2
                                             * Loong = 8
                                             */

                                        var num = 0;
                                        if (instr[i].Operand == null) continue;
                                        var op = instr[i].Operand.ToString().ToLower();
                                        if (op.Contains("int"))
                                        {
                                            num = 4;
                                        }
                                        else if (op.Contains("sbyte"))
                                        {
                                            num = 1;
                                        }
                                        else if (op.Contains("byte"))
                                        {
                                            num = 1;
                                        }
                                        else if (op.Contains("bool"))
                                        {
                                            num = 1;
                                        }
                                        else if (op.Contains("decimal"))
                                        {
                                            num = 16;
                                        }
                                        else if (op.Contains("short"))
                                        {
                                            num = 2;
                                        }
                                        else if (op.Contains("long"))
                                        {
                                            num = 8;
                                        }


                                        if (num != 0)
                                        {
                                            instr[i].OpCode = OpCodes.Ldc_I4;
                                            instr[i].Operand = num;
                                            instr.Insert(i + 1, new Instruction(OpCodes.Nop));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    



        //Prune
        public static void PruneModule(ModuleDefMD module)
        {
            //Remove String Method decryption
            foreach (var method in StrDecMeth)
            {
                var type = method.DeclaringType;
                try
                {
                    type.Methods.Remove(method);
                    Form1.PrunedMembers += 1;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            //Remove Integer Method calculation
            foreach (var methodz in IntDecMeth)
            {
                var type = methodz.DeclaringType;
                try
                {
                    type.Methods.Remove(methodz);
                    Form1.PrunedMembers += 1;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            //Remove Remaining Integer Method
            foreach (var methodz in RemainingIntsMeth)
            {
                var type = methodz.DeclaringType;
                try
                {
                    type.Methods.Remove(methodz);
                    Form1.PrunedMembers += 1;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            //Remove useless and fake TypeDefs
            foreach (TypeDef type in module.Types)
            { 
                if (type.Name == "ConfusedByAttribute")
                {
                    Type2Remove.Add(type);
                }
                if (type.Name == "DotNetPatcherObfuscatorAttribute" && type.Methods.Count == 1 && type.Methods[0].IsConstructor)
                {
                    type.Name = "DNPDeobfuscated";
                }
                if (type.Name == "dotNetProtector")
                {
                    Type2Remove.Add(type);
                }
                if (type.Name == "ObfuscatedByGoliath")
                {
                    Type2Remove.Add(type);
                }
                if (type.Name == "PoweredByAttribute")
                {
                    Type2Remove.Add(type);
                }             
            }

            foreach (var typez in Type2Remove)
            {
                try
                {
                    module.Types.Remove(typez);
                    Form1.PrunedMembers += 1;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }


    }
}