internal static partial class Program
{
    // New generator support, independently untested. These records are CIL byte offsets.
    private record FinallyClause(int TryStart,int TryEnd,int HandlerStart,int HandlerEnd);
    private static readonly Dictionary<int,FinallyClause[]> FinallyBodies = [];
    private static void RegisterFinally(int token,IL body,string start,string handler,string end)
    {
        var clause = new FinallyClause(body.OffsetOf(start),body.OffsetOf(handler),body.OffsetOf(handler),body.OffsetOf(end));
        Require(clause.TryStart < clause.TryEnd && clause.HandlerStart < clause.HandlerEnd,"Nonempty finally regions");
        Require(token is 0x06000562 or 0x060005AC or 0x060005B1 or 0x060005B2 or 0x060005B3 or 0x06000276 or 0x060001FF or 0x06000549 or 0x0600060E or 0x060006EA or 0x0600058B or 0x060001A2 or 0x06000573 or 0x06000487 or 0x06000041 or 0x06000184 or 0x06000698 or 0x06000288 or 0x0600014C or 0x06000633 or 0x06000324 or 0x0600060F or 0x060001F0 or 0x060006EB or 0x06000013 or 0x0600004C or 0x060003B8 or 0x06000330 or 0x0600068C or 0x0600058F or 0x06000352 or 0x06000308 or 0x0600048F or 0x06000494 or 0x060000C7 or 0x060005E5 or 0x0600047C or 0x06000478,"Explicit EH target allowlist");
        FinallyBodies.Add(token,[clause]);
    }
    private static int FinallySectionSize(int token) => FinallyBodies.TryGetValue(token,out var clauses) ? checked(4+24*clauses.Length) : 0;
    private static void WriteFinallySection(byte[] image,int at,int token,int codeLength)
    {
        var clauses=FinallyBodies[token];int size=checked(4+24*clauses.Length);
        Require(size<0x1000000,"24-bit fat EH data size");image[at]=0x41;
        image[at+1]=(byte)size;image[at+2]=(byte)(size>>8);image[at+3]=(byte)(size>>16);
        for(int i=0;i<clauses.Length;i++)
        {
            var c=clauses[i];Require(c.TryStart>=0 && c.TryEnd<=codeLength && c.HandlerStart>=c.TryEnd && c.HandlerEnd<=codeLength,"Bounded EH clause");
            int row=at+4+24*i;Write32(image,row,2); // COR_ILEXCEPTION_CLAUSE_FINALLY
            Write32(image,row+4,c.TryStart);Write32(image,row+8,c.TryEnd-c.TryStart);
            Write32(image,row+12,c.HandlerStart);Write32(image,row+16,c.HandlerEnd-c.HandlerStart);Write32(image,row+20,0);
        }
    }
}
