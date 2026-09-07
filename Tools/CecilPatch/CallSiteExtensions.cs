using Mono.Cecil;

internal static class CecilCallSiteExtensions
{
    // Mono.Cecil 0.11.6 has no ModuleDefinition.ImportReference(CallSite) overload.
    // Rebuild the signature explicitly. The current GodsPVZ mobile patch methods do
    // not use calli, but supporting it keeps the body cloner correct for future
    // whitelist additions.
    public static CallSite ImportReference(this ModuleDefinition module, CallSite source)
    {
        var target = new CallSite(module.ImportReference(source.ReturnType))
        {
            HasThis = source.HasThis,
            ExplicitThis = source.ExplicitThis,
            CallingConvention = source.CallingConvention,
        };

        foreach (var parameter in source.Parameters)
            target.Parameters.Add(new ParameterDefinition(module.ImportReference(parameter.ParameterType)));

        return target;
    }
}
