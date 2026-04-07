using Raze.Script.Core.Builders;
using Raze.Script.Core.Builders.Types;

namespace Raze.Script.Core.BuiltInModules;

internal static class MathModule
{
    internal static void Build(ModuleBuilder builder)
    {
        builder
            .HasName("math")
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("PI")
                    .HasType(TypeBuilder.Decimal)
                    .HasValue(3.14159m);
            })
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("E")
                    .HasType(TypeBuilder.Decimal)
                    .HasValue(2.71828m);
            })
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("MAX_INTEGER")
                    .HasType(TypeBuilder.Integer)
                    .HasValue(Int128.MaxValue);
            })
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("MIN_INTEGER")
                    .HasType(TypeBuilder.Integer)
                    .HasValue(Int128.MinValue);
            })
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("MAX_DECIMAL")
                    .HasType(TypeBuilder.Decimal)
                    .HasValue(decimal.MaxValue);
            })
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("MIN_DECIMAL")
                    .HasType(TypeBuilder.Decimal)
                    .HasValue(decimal.MinValue);
            });
    }
}
