using System.Reflection;

namespace GDi.Abstract.SignalR.DTO
{
    public class ExampleDTO
    {
        public string MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }

        public override string ToString()
        {
            return MethodBase.GetCurrentMethod().DeclaringType + " [" +
               "MyProperty1=" + MyProperty1 +
               "; MyProperty2=" + MyProperty2 +
               "]";
        }
    }
}
