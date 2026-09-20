// The in-game editor supplies System.Linq implicitly; the stand-in needs it stated.
global using System.Linq;

namespace Sandbox.ModAPI.Ingame
{
    public class StubGts : IMyGridTerminalSystem
    {
        public dynamic GetBlockGroupWithName(params object[] a) { return null; }
        public dynamic GetBlocks(params object[] a) { return null; }
        public dynamic GetBlockWithName(params object[] a) { return null; }
    }
}

static class CombinedCheck { static void Main() { } }
