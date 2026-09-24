// Prism grammar for the HELM DSL (Custom Data configuration language).
// Used by @11ty/eleventy-plugin-syntaxhighlight through eleventy.config.js.
function registerHelmGrammar(Prism) {
  Prism.languages.helm = {
    comment: { pattern: /^[ \t]*#.*$/m, greedy: true },
    string: { pattern: /"(?:[^"\\\r\n]|\\.)*"/, greedy: true },
    placeholder: { pattern: /<[A-Za-z_][^<>\r\n]*>/, alias: "variable" },
    directive: { pattern: /@[A-Za-z_]\w*/, alias: "keyword" },
    property: /\b[A-Za-z_][\w-]*(?=:)/,
    function: /\b[A-Za-z_]\w*(?=\()/,
    color: { pattern: /#[0-9A-Fa-f]{3,8}\b/, alias: "number" },
    variable: /\b[A-Za-z_]\w*\.[A-Za-z_]\w*\b/,
    number: /\b\d+(?:\.\d+)?\b/,
    boolean: /\b(?:true|false|on|off)\b/,
    punctuation: /[(),=:]/,
  };
  Prism.languages.hlm = Prism.languages.helm;
}

// CommonJS is used by Eleventy at build time; the browser global is used by WebUtil's
// live editor. Keeping one grammar prevents the docs examples and editor drifting apart.
if (typeof module !== "undefined" && module.exports) module.exports = registerHelmGrammar;
if (typeof window !== "undefined") {
  window.registerHelmGrammar = registerHelmGrammar;
  if (window.Prism) registerHelmGrammar(window.Prism);
}
