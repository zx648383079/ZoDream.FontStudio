using System.Diagnostics;
using ZoDream.Shared.Font;
using ZoDream.Shared.OpenType.Converters;

namespace ZoDream.Tests
{
    [TestClass]
    public sealed class TypefaceTest
    {
        [TestMethod]
        public void TestConverter()
        {
            var type = typeof(AxisVariationsConverter);
            foreach (var item in type.GetInterfaces())
            {
                if (!item.IsGenericType || item.GenericTypeArguments.Length != 1)
                {
                    continue;
                }
                var target = item.GenericTypeArguments[0].IsAssignableTo(typeof(ITypefaceTable));
                Assert.IsTrue(target);
            }
        }
    }
}
