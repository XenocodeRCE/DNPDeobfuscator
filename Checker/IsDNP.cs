using System.Linq;
using dnlib.DotNet;

namespace DNPD.Checker
{
    class IsDNP
    {
        public static bool Check(ModuleDefMD module)
        {
            return module.Resources.Any(res => res.Name.ToLower().Contains("dotnetpatcher")) || module.Types.Any(type => type.Name.ToLower().Contains("dotnetpatcher"));
        }
    }
}
