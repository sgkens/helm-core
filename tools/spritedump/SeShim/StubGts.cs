namespace Sandbox.ModAPI.Ingame
{
    public class StubGts : IMyGridTerminalSystem
    {
        public dynamic GetBlockGroupWithName(params object[] a) { return null; }
        public dynamic GetBlocks(params object[] a) { return null; }
        public dynamic GetBlockWithName(params object[] a) { return null; }
    }
}
