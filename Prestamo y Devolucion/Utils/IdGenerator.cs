using System.Threading;
namespace BibliotecaAPEC.Utils;
public static class IdGenerator
{
    private static int _id = 0;
    public static int NextId() => Interlocked.Increment(ref _id);
}
