public class TestClass
{
    public string TestPublicField = "123";
    private int testPrivateField;
    protected bool testProtectedField;
    internal int testInternalField;

    public static string TestPublicStaticField = "123";
    private static int testPrivateStaticField;
    protected static bool testProtectedStaticField;
    
    public int TestPublicMethod(int a, int b) => a + b;
    private string TestPrivateMethod(string a, bool b) => a;

    private class NestedClass
    {
        public string TestNestedClassField = string.Empty;
        public int testNestedClassMethod(int a, int b) => a + b;
    }
}
