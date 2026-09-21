// SeShim.cs — minimal stand-in for the Space Engineers ingame API, used ONLY to
// compile Program.cs headlessly so its sprite output can be dumped as JSON.
// Only members Program.cs touches are declared; behavior is not game-accurate
// beyond the math types used for layout (Vector2, Color).
using System;
using System.Collections.Generic;

namespace VRageMath
{
    public struct Vector2
    {
        public float X, Y;
        public Vector2(float x, float y) { X = x; Y = y; }
        public Vector2(float v) { X = v; Y = v; }
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, float s) => new Vector2(a.X * s, a.Y * s);
        public static Vector2 operator /(Vector2 a, float s) => new Vector2(a.X / s, a.Y / s);
        public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);
        public override bool Equals(object o) => o is Vector2 v && this == v;
        public override int GetHashCode() => X.GetHashCode() ^ (Y.GetHashCode() * 397);
    }

    public struct Color
    {
        public byte R, G, B, A;
        public static Color White => new Color(255, 255, 255);
        public static Color Black => new Color(0, 0, 0);
        public static Color Red => new Color(255, 0, 0);
        public static Color Transparent => new Color(0, 0, 0, 0);
        public Color(int r, int g, int b) { R = (byte)r; G = (byte)g; B = (byte)b; A = 255; }
        public Color(int r, int g, int b, int a) { R = (byte)r; G = (byte)g; B = (byte)b; A = (byte)a; }
        public Color(byte r, byte g, byte b, byte a) { R = r; G = g; B = b; A = a; }
        public static Color Lerp(Color a, Color b, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            return new Color((int)Math.Round(a.R + (b.R - a.R) * t), (int)Math.Round(a.G + (b.G - a.G) * t),
                             (int)Math.Round(a.B + (b.B - a.B) * t), (int)Math.Round(a.A + (b.A - a.A) * t));
        }
        public static Color operator *(Color c, float s) =>
            new Color((int)Math.Min(255, c.R * s), (int)Math.Min(255, c.G * s), (int)Math.Min(255, c.B * s), c.A);
        public static bool operator ==(Color a, Color b) => a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        public static bool operator !=(Color a, Color b) => !(a == b);
        public override bool Equals(object o) => o is Color c && this == c;
        public override int GetHashCode() => (R << 24) ^ (G << 16) ^ (B << 8) ^ A;
    }
}

namespace VRage.Game.GUI.TextPanel
{
    public enum SpriteType { TEXTURE, TEXT, CLIP_RECT }
    public enum TextAlignment { LEFT, RIGHT, CENTER }
    public enum ContentType { NONE, TEXT_AND_IMAGE, SCRIPT }

    public struct MySprite
    {
        public SpriteType Type;
        public string Data;
        public VRageMath.Vector2? Position;
        public VRageMath.Vector2? Size;
        public VRageMath.Color? Color;
        public string FontId;
        public TextAlignment Alignment;
        public float RotationOrScale;
        public static MySprite CreateText(string text, string font, VRageMath.Color color, float scale = 1f, TextAlignment alignment = TextAlignment.CENTER)
            => new MySprite { Type = SpriteType.TEXT, Data = text, FontId = font, Color = color, RotationOrScale = scale, Alignment = alignment };
    }

    public class MySpriteDrawFrame : IDisposable
    {
        public readonly List<MySprite> Sprites = new List<MySprite>();
        public void Add(MySprite s) { Sprites.Add(s); }
        public void AddRange(IEnumerable<MySprite> s) { Sprites.AddRange(s); }
        public virtual void Dispose() { }
    }
}

namespace Sandbox.ModAPI.Ingame
{
    using VRage.Game.GUI.TextPanel;
    using VRageMath;

    [Flags] public enum UpdateType { None = 0, Terminal = 1, Trigger = 2, Mod = 4, Script = 8, Update1 = 16, Update10 = 32, Update100 = 64, Once = 128, IGC = 256 }
    [Flags] public enum UpdateFrequency { None = 0, Once = 1, Update1 = 2, Update10 = 4, Update100 = 8 }

    public enum MyShipConnectorStatus { Unconnected, Connectable, Connected }
    public enum LandingGearMode { Unlocked, ReadyToLock, Locked }
    public enum MyAssemblerMode { Assembly, Disassembly }

    public interface IMyTextSurface : IDisposable
    {
        Vector2 SurfaceSize { get; }
        Vector2 TextureSize { get; }
        ContentType ContentType { get; set; }
        string Script { get; set; }
        Color ScriptBackgroundColor { get; set; }
        Color ScriptForegroundColor { get; set; }
        string Font { get; set; }
        float FontSize { get; set; }
        Color FontColor { get; set; }
        Color BackgroundColor { get; set; }
        bool WriteText(string value, bool append = false);
        MySpriteDrawFrame DrawFrame();
        Vector2 MeasureStringInPixels(System.Text.StringBuilder text, string font, float scale);
    }
    public interface IMyTextSurfaceProvider { int SurfaceCount { get; } IMyTextSurface GetSurface(int index); }
    public interface IMyEntity { }
    public partial interface IMyTerminalBlock : IMyEntity { string CustomName { get; set; } string CustomData { get; set; } IMyCubeGrid CubeGrid { get; } }
    public partial interface IMyBatteryBlock : IMyTerminalBlock { }
    public partial interface IMyReactor : IMyTerminalBlock { }
    public partial interface IMySolarPanel : IMyTerminalBlock { }
    public partial interface IMyWindTurbine : IMyTerminalBlock { }
    public partial interface IMyGasTank : IMyTerminalBlock { }
    public partial interface IMyAirVent : IMyTerminalBlock { }
    public partial interface IMyCargoContainer : IMyTerminalBlock { }
    public partial interface IMyJumpDrive : IMyTerminalBlock { }
    public partial interface IMyThrust : IMyTerminalBlock { }
    public partial interface IMyMotorSuspension : IMyTerminalBlock { }
    public partial interface IMyDoor : IMyTerminalBlock { }
    public partial interface IMyShipConnector : IMyTerminalBlock { }
    public partial interface IMyAssembler : IMyTerminalBlock { }
    public partial interface IMyRefinery : IMyTerminalBlock { }
    public partial interface IMyShipDrill : IMyTerminalBlock { }
    public partial interface IMyShipWelder : IMyTerminalBlock { }
    public partial interface IMyShipGrinder : IMyTerminalBlock { }
    public partial interface IMyGasGenerator : IMyTerminalBlock { }
    public partial interface IMyLargeTurretBase : IMyTerminalBlock { }
    public partial interface IMyProjector : IMyTerminalBlock { }
    public partial interface IMyGyro : IMyTerminalBlock { }
    public partial interface IMyGravityGeneratorBase : IMyTerminalBlock { }
    public partial interface IMyLandingGear : IMyTerminalBlock { }
    public partial interface IMyRadioAntenna : IMyTerminalBlock { }
    public partial interface IMyBeacon : IMyTerminalBlock { }
    public partial interface IMyLightingBlock : IMyTerminalBlock { }
    public partial interface IMyPistonBase : IMyTerminalBlock { }
    public partial interface IMyMotorStator : IMyTerminalBlock { }
    public partial interface IMyWarhead : IMyTerminalBlock { }
    public partial interface IMySensorBlock : IMyTerminalBlock { }
    public partial interface IMyCameraBlock : IMyTerminalBlock { }
    public partial interface IMyParachute : IMyTerminalBlock { }
    public partial interface IMyTurretControlBlock : IMyTerminalBlock { }
    public partial interface IMyProductionBlock : IMyTerminalBlock { }
    public partial interface IMyPowerProducer : IMyTerminalBlock { }
    public partial interface IMyFunctionalBlock : IMyTerminalBlock { }
    public partial interface IMyShipToolBase : IMyTerminalBlock { }
    public interface IMyBroadcastListener { bool HasPendingMessage { get; } void SetMessageCallback(string arg = ""); MyIGCMessage AcceptMessage(); }
    public struct MyIGCMessage { public object Data; public string Tag; }
    public interface IMyCubeGrid { }
    public partial interface IMyGridTerminalSystem { }
    public interface IMyIntergridCommunicationSystem { IMyBroadcastListener RegisterBroadcastListener(string tag); }
    public interface IMyGridProgramRuntimeInfo { UpdateFrequency UpdateFrequency { get; set; } double LastRunTimeMs { get; } int CurrentInstructionCount { get; } int MaxInstructionCount { get; } int CurrentCallChainDepth { get; } int MaxCallChainDepth { get; } }

    public abstract class MyGridProgram
    {
        // The game injects these before the constructor runs; the dump harness
        // does the same through the static Bootstrap slot.
        public static IMyTerminalBlock BootstrapMe;
        public IMyTerminalBlock Me = BootstrapMe;
        public string Storage;
        public Action<string> Echo = s => { };
        public IMyGridProgramRuntimeInfo Runtime = new StubRuntime();
        public IMyIntergridCommunicationSystem IGC = new StubIgc();
        public IMyGridTerminalSystem GridTerminalSystem = new StubGts();
    }

    public class StubRuntime : IMyGridProgramRuntimeInfo
    {
        public UpdateFrequency UpdateFrequency { get; set; }
        public double LastRunTimeMs { get { return 0; } }
        public int CurrentInstructionCount { get { return 0; } }
        public int MaxInstructionCount { get { return 50000; } }
        public int CurrentCallChainDepth { get { return 0; } }
        public int MaxCallChainDepth { get { return 1000; } }
    }
    public class StubIgc : IMyIntergridCommunicationSystem
    {
        public IMyBroadcastListener RegisterBroadcastListener(string tag) { return null; }
    }
}
