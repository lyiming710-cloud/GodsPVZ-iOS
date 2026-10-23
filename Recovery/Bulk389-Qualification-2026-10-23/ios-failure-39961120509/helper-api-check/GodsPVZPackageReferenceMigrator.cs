#if UNITY_EDITOR
// Work-copy-only tombstone: retain the source path used by the compilation graph.
// No InitializeOnLoad, static constructor, delayCall, or DLL rewriting is allowed.
public static class GodsPVZPackageReferenceMigrator
{
    public static void RunBatch()
    {
        throw new System.InvalidOperationException("Legacy migration was completed before Bulk389 injection; rerunning it is forbidden.");
    }
}
#endif
