// MinifyCheck: proves that a minified C# file is the same program as its source.
// Both files are lexed by Roslyn and their token streams (kind and text, ignoring whitespace and comments)
// must be identical. Joining two tokens into a different one ("+ +" to "++") or dropping a token fails.
//
// Usage: dotnet run --project tools/minify-check -- <source.cs> <minified.cs>
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

static class MinifyCheck
{
    static List<SyntaxToken> Tokens(string path)
    {
        var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(path));
        var list = new List<SyntaxToken>();
        foreach (var t in tree.GetRoot().DescendantTokens())
            if (t.Kind() != SyntaxKind.EndOfFileToken) list.Add(t);
        return list;
    }

    static int Main(string[] args)
    {
        if (args.Length != 2) { Console.Error.WriteLine("usage: minify-check <source.cs> <minified.cs>"); return 2; }
        var a = Tokens(args[0]);
        var b = Tokens(args[1]);
        int n = Math.Min(a.Count, b.Count);
        for (int i = 0; i < n; i++)
        {
            if (a[i].Kind() != b[i].Kind() || a[i].Text != b[i].Text)
            {
                Console.Error.WriteLine("minify-check: token " + i + " differs");
                Console.Error.WriteLine("  source  : " + a[i].Kind() + " '" + a[i].Text + "' near " + a[i].GetLocation().GetLineSpan().StartLinePosition);
                Console.Error.WriteLine("  minified: " + b[i].Kind() + " '" + b[i].Text + "' near " + b[i].GetLocation().GetLineSpan().StartLinePosition);
                return 1;
            }
        }
        if (a.Count != b.Count)
        {
            Console.Error.WriteLine("minify-check: token counts differ (" + a.Count + " vs " + b.Count + ")");
            return 1;
        }
        Console.WriteLine("minify-check: " + a.Count + " tokens identical");
        return 0;
    }
}
