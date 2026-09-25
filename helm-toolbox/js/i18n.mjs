const LOCALE_KEY = "helm-toolbox_locale_v1";
const LOCALE_BASE = new URL("../locales/", import.meta.url);

let messages = {};

function preferredLocale(manifest) {
  const available = new Set(manifest.locales.map((locale) => locale.id));
  const query = new URLSearchParams(location.search).get("lang");
  let saved = "";
  try { saved = localStorage.getItem(LOCALE_KEY) || ""; } catch {}
  const browser = navigator.language || "";
  for (const candidate of [query, saved, browser, browser.split("-")[0], manifest.default]) {
    if (candidate && available.has(candidate)) return candidate;
  }
  return manifest.default;
}

async function loadCatalog(locale, fallback) {
  const fallbackMessages = await fetch(new URL(`${fallback}.json`, LOCALE_BASE)).then((response) => response.json());
  if (locale === fallback) return fallbackMessages;
  try {
    const translated = await fetch(new URL(`${locale}.json`, LOCALE_BASE)).then((response) => {
      if (!response.ok) throw new Error(`HTTP ${response.status}`);
      return response.json();
    });
    return { ...fallbackMessages, ...translated };
  } catch {
    return fallbackMessages;
  }
}

export function t(key, replacements = {}) {
  const template = messages[key] || key;
  return template.replace(/\{(\w+)\}/g, (_, name) => replacements[name] ?? `{${name}}`);
}

function applyMessages(root = document) {
  root.querySelectorAll("[data-i18n]").forEach((element) => { element.textContent = t(element.dataset.i18n); });
  root.querySelectorAll("[data-i18n-placeholder]").forEach((element) => {
    element.setAttribute("placeholder", t(element.dataset.i18nPlaceholder));
  });
  root.querySelectorAll("[data-i18n-aria-label]").forEach((element) => {
    element.setAttribute("aria-label", t(element.dataset.i18nAriaLabel));
  });
}

export async function initializeI18n() {
  const manifest = await fetch(new URL("index.json", LOCALE_BASE)).then((response) => response.json());
  const locale = preferredLocale(manifest);
  messages = await loadCatalog(locale, manifest.default);
  document.documentElement.lang = locale;
  applyMessages();

  const picker = document.querySelector("#locale-picker");
  if (picker) {
    picker.replaceChildren(...manifest.locales.map((item) => {
      const option = document.createElement("option");
      option.value = item.id;
      option.textContent = item.nativeLabel;
      return option;
    }));
    picker.value = locale;
    picker.addEventListener("change", () => {
      try { localStorage.setItem(LOCALE_KEY, picker.value); } catch {}
      const url = new URL(location.href);
      url.searchParams.set("lang", picker.value);
      location.assign(url);
    });
  }
  return { locale, manifest };
}
