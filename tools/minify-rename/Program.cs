// MinifyRename: shortens the names of everything HELM declares (locals, parameters, fields, methods, properties and
// nested types) in the minified script. Names that belong to the game API, and the three the game calls
// (Program, Main, Save), are never touched. The renaming is symbol based, so a use is always renamed together
// with its declaration; the result is compiled again and must have no errors.
//
// Usage: dotnet run --project tools/minify-rename -- <body.cs> <SeShim.dll> <out.cs> [--map map.txt]
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

static class MinifyRename
{
    // The wrapper the Programmable Block puts around the pasted text (the same as Build-Release.ps1).
    const string Prefix = "using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Text;" +
                          "using Sandbox.ModAPI.Ingame;using VRage.Game.GUI.TextPanel;using VRage.Game.ModAPI.Ingame;using VRageMath;" +
                          "namespace IngameScript{public partial class Program:MyGridProgram{\n";
    const string Suffix = "\n}}";

    static readonly HashSet<string> Keep = new HashSet<string> { "Main", "Program", "Save" };

    static ISymbol SymbolFor(SyntaxToken tok, SemanticModel m)
    {
        var n = tok.Parent;
        if (n is IdentifierNameSyntax || n is GenericNameSyntax)
        {
            var info = m.GetSymbolInfo(n);
            return info.Symbol ?? info.CandidateSymbols.FirstOrDefault();
        }
        if (n is ConstructorDeclarationSyntax cd) return m.GetDeclaredSymbol(cd)?.ContainingType;
        if (n is VariableDeclaratorSyntax || n is ParameterSyntax || n is MethodDeclarationSyntax || n is PropertyDeclarationSyntax ||
            n is ClassDeclarationSyntax || n is StructDeclarationSyntax || n is EnumDeclarationSyntax || n is EnumMemberDeclarationSyntax ||
            n is ForEachStatementSyntax || n is CatchDeclarationSyntax || n is TypeParameterSyntax || n is InterfaceDeclarationSyntax)
            return m.GetDeclaredSymbol(n);
        return null;
    }

    static bool IsSource(ISymbol s) { return s.Locations.Any(l => l.IsInSource); }

    static bool Implements(ISymbol s)
    {
        if (s.ContainingType == null) return false;
        if (s is IMethodSymbol ms && (ms.IsOverride || ms.ExplicitInterfaceImplementations.Length > 0)) return true;
        if (s is IPropertySymbol ps && (ps.IsOverride || ps.ExplicitInterfaceImplementations.Length > 0)) return true;
        foreach (var i in s.ContainingType.AllInterfaces)
            foreach (var im in i.GetMembers())
                if (SymbolEqualityComparer.Default.Equals(s.ContainingType.FindImplementationForInterfaceMember(im), s)) return true;
        return false;
    }

    static IEnumerable<string> Names()
    {
        const string first = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
        const string rest = first + "0123456789";
        foreach (char c in first) yield return c.ToString();
        foreach (char c in first) foreach (char d in rest) yield return "" + c + d;
        foreach (char c in first) foreach (char d in rest) foreach (char e in rest) yield return "" + c + d + e;
    }

    static int Main(string[] args)
    {
        if (args.Length < 3) { Console.Error.WriteLine("usage: minify-rename <body.cs> <SeShim.dll> <out.cs> [--map map.txt]"); return 2; }
        string body = File.ReadAllText(args[0]).TrimStart('﻿');
        string text = Prefix + body + Suffix;
        var tree = CSharpSyntaxTree.ParseText(text, new CSharpParseOptions(LanguageVersion.CSharp6));
        var refs = new List<MetadataReference>();
        foreach (var p in ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator)) refs.Add(MetadataReference.CreateFromFile(p));
        refs.Add(MetadataReference.CreateFromFile(args[1]));
        var comp = CSharpCompilation.Create("helm", new[] { tree }, refs, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var errs = comp.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
        if (errs.Count > 0) { foreach (var e in errs.Take(10)) Console.Error.WriteLine("input: " + e); return 1; }

        var model = comp.GetSemanticModel(tree);
        var root = tree.GetRoot();
        var occ = new List<KeyValuePair<SyntaxToken, ISymbol>>();
        var external = new HashSet<string>();
        foreach (var tok in root.DescendantTokens())
        {
            if (!tok.IsKind(SyntaxKind.IdentifierToken)) continue;
            if (tok.Text == "var") continue;
            if (tok.SpanStart < Prefix.Length) continue; // the wrapper is not part of the script
            var s = SymbolFor(tok, model);
            if (s != null) s = s.OriginalDefinition;
            bool ours = s != null && IsSource(s) && !(s is INamespaceSymbol) && !s.IsImplicitlyDeclared && !Keep.Contains(s.Name) && !Implements(s)
                        && !(s is IMethodSymbol m0 && m0.MethodKind != MethodKind.Ordinary) && !(s is ITypeSymbol t0 && t0.Name == "Program")
                        && !(s is IFieldSymbol f0 && f0.ContainingType.TypeKind == TypeKind.Enum && false);
            if (ours) occ.Add(new KeyValuePair<SyntaxToken, ISymbol>(tok, s));
            else external.Add(tok.Text);
        }

        // Most used first, so they get the shortest names.
        var count = new Dictionary<ISymbol, int>(SymbolEqualityComparer.Default);
        foreach (var o in occ) { int c; count.TryGetValue(o.Value, out c); count[o.Value] = c + 1; }
        var order = count.OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key.Name, StringComparer.Ordinal)
                         .ThenBy(kv => kv.Key.Locations[0].SourceSpan.Start).Select(kv => kv.Key).ToList();
        var taken = new HashSet<string>(external);
        var reserved = SyntaxFacts.GetKeywordKinds().Select(k => SyntaxFacts.GetText(k)).Concat(SyntaxFacts.GetContextualKeywordKinds().Select(k => SyntaxFacts.GetText(k)));
        foreach (var r in reserved) taken.Add(r);
        var newName = new Dictionary<ISymbol, string>(SymbolEqualityComparer.Default);
        using (var names = Names().GetEnumerator())
        {
            foreach (var s in order)
            {
                string n;
                do { names.MoveNext(); n = names.Current; } while (taken.Contains(n));
                newName[s] = n;
                taken.Add(n);
            }
        }

        var sb = new StringBuilder(text);
        foreach (var o in occ.OrderByDescending(x => x.Key.SpanStart))
        {
            sb.Remove(o.Key.SpanStart, o.Key.Span.Length);
            sb.Insert(o.Key.SpanStart, newName[o.Value]);
        }
        string result = sb.ToString();

        // The renamed program must compile with no errors and the same warnings as the input.
        var tree2 = CSharpSyntaxTree.ParseText(result, new CSharpParseOptions(LanguageVersion.CSharp6));
        var comp2 = CSharpCompilation.Create("helm2", new[] { tree2 }, refs, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var errs2 = comp2.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
        if (errs2.Count > 0) { File.WriteAllText(args[2] + ".failed", result); foreach (var e in errs2.Take(10)) Console.Error.WriteLine("renamed: " + e); return 1; }

        string outBody = result.Substring(Prefix.Length, result.Length - Prefix.Length - Suffix.Length);
        File.WriteAllText(args[2], outBody, new UTF8Encoding(true));
        int mapIdx = Array.IndexOf(args, "--map");
        if (mapIdx >= 0)
            File.WriteAllLines(args[mapIdx + 1], order.Select(s => s.Kind + " " + (s.ContainingType != null ? s.ContainingType.Name + "." : "") + s.Name + " -> " + newName[s] + "  (" + count[s] + ")"));
        Console.WriteLine("minify-rename: " + order.Count + " names shortened, " + occ.Count + " uses; " + body.Length + " -> " + outBody.Length + " characters (" + (body.Length - outBody.Length) + " saved)");
        return 0;
    }
}
