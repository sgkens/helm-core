// Pure text helpers for palette insertion. Kept DOM-free so duplicate-bind and cursor behavior can
// be verified without loading the WebAssembly renderer or a browser.
export const BIND_SUGGESTIONS = {
  batt: 'group("Batteries", agg: avg)',
  bomb: 'block("Warhead")',
  cargo: 'group("Cargo Containers", agg: sum)',
  controller: 'block("Flight Controller")',
  gear: 'group("Landing Gear", agg: min)',
  gyro: 'group("Gyroscopes", agg: avg)',
  h2: 'group("Hydrogen Tanks", agg: avg)',
  h2_tank: 'group("Hydrogen Tanks", agg: avg)',
  lights: 'group("Lights", agg: avg)',
  notes: 'block("Status LCD")',
  o2_tank: 'group("Oxygen Tanks", agg: avg)',
  power: 'group("Power Producers", agg: avg)',
  pwr: 'group("Batteries", agg: avg)',
  radar: 'block("Target Camera")',
  rudder: 'block("Rudder Hinge")',
  shield: 'dynamic("helm-toolbox.shield", default: 1)',
  thrusters: 'group("Thrusters", agg: avg)',
  turret: 'group("Turrets", agg: sum)',
};

export function snippetAliases(snippet) {
  return [...snippet.matchAll(/\b([A-Za-z_][\w]*)\.[A-Za-z_][\w:-]*/g)].map((match) => match[1]);
}

function bindingInsertOffset(dsl) {
  const binds = [...dsl.matchAll(/^@bind\s+[^\r\n]*(?:\r?\n|$)/gm)];
  if (binds.length) {
    const last = binds[binds.length - 1];
    return { offset: last.index + last[0].length, hasBinds: true };
  }
  const firstStructure = dsl.match(/^@(screen|template|banner)\b/m);
  return { offset: firstStructure ? firstStructure.index : 0, hasBinds: false };
}

export function applyPaletteInsertion(dsl, snippet, start, end, suggestions = {}) {
  const declared = new Set([...dsl.matchAll(/^@bind\s+([A-Za-z_]\w*)\s*=/gm)].map((match) => match[1].toLowerCase()));
  const aliases = [...new Set(snippetAliases(snippet))];
  const missing = aliases.filter((alias) => !declared.has(alias.toLowerCase()));

  if (missing.length) {
    const location = bindingInsertOffset(dsl);
    const newline = dsl.includes("\r\n") ? "\r\n" : "\n";
    const declarations = missing.map((alias) => {
      const key = alias.toLowerCase();
      const source = suggestions[alias] || suggestions[key] || BIND_SUGGESTIONS[key] || `dynamic("helm-toolbox.${alias}", default: 50)`;
      return `@bind ${alias} = ${source}`;
    }).join(newline) + (location.hasBinds ? newline : newline + newline);
    dsl = dsl.slice(0, location.offset) + declarations + dsl.slice(location.offset);
    if (location.offset <= start) start += declarations.length;
    if (location.offset <= end) end += declarations.length;
  }

  const text = dsl.slice(0, start) + snippet + dsl.slice(end);
  return { text, selection: start + snippet.length, addedBinds: missing };
}
