// Docs site behavior: theme, header search, table of contents, page tools, mermaid.
(function () {
  "use strict";

  var BASE = ((document.querySelector('meta[name="helm-base"]') || {}).content || "/").replace(/\/$/, "");
  var THEME_KEY = "helm_docs_theme";
  var TOC_KEY = "helm_docs_toc";

  function store(get, key, value) {
    try {
      if (get) return localStorage.getItem(key);
      localStorage.setItem(key, value);
    } catch (e) {}
    return null;
  }

  // ---- Theme ----------------------------------------------------------
  window.setDocTheme = function (theme) {
    document.documentElement.setAttribute("data-theme", theme);
    store(false, THEME_KEY, theme);
    if (window.helmApplySyntaxTheme) window.helmApplySyntaxTheme();
    renderMermaid();
  };

  window.setSyntaxTheme = function (name) {
    store(false, "helm_syntax_theme", name);
    if (window.helmApplySyntaxTheme) window.helmApplySyntaxTheme();
  };

  // ---- Header search --------------------------------------------------
  function initSearch() {
    var input = document.getElementById("docs-search");
    var results = document.getElementById("docs-search-results");
    var dataEl = document.getElementById("docs-search-data");
    if (!input || !results || !dataEl) return;

    var pages = JSON.parse(dataEl.textContent);
    var selected = -1;
    var shown = [];

    function render(list) {
      shown = list;
      selected = list.length ? 0 : -1;
      results.innerHTML = "";
      if (!list.length) {
        var empty = document.createElement("div");
        empty.className = "p-3 text-xs text-base-content/60";
        empty.textContent = "No matching pages";
        results.appendChild(empty);
      }
      list.forEach(function (p, i) {
        var a = document.createElement("a");
        a.href = BASE + p.url;
        a.setAttribute("role", "option");
        a.setAttribute("aria-selected", i === 0 ? "true" : "false");
        a.className = "block px-3 py-2 border-b border-base-300 last:border-0";
        var t = document.createElement("div");
        t.className = "text-xs font-bold font-mono";
        t.textContent = p.title;
        var d = document.createElement("div");
        d.className = "text-[11px] text-base-content/60";
        d.textContent = p.section + " - " + p.description;
        a.appendChild(t);
        a.appendChild(d);
        results.appendChild(a);
      });
      results.hidden = false;
    }

    function search(q) {
      q = q.toLowerCase().trim();
      if (!q) { results.hidden = true; return; }
      var words = q.split(/\s+/);
      var hits = pages.filter(function (p) {
        var hay = (p.title + " " + p.section + " " + p.description).toLowerCase();
        return words.every(function (w) { return hay.indexOf(w) !== -1; });
      });
      render(hits.slice(0, 8));
    }

    function mark(i) {
      var items = results.querySelectorAll("a");
      items.forEach(function (el, n) { el.setAttribute("aria-selected", n === i ? "true" : "false"); });
      selected = i;
    }

    input.addEventListener("input", function () { search(input.value); });
    input.addEventListener("focus", function () { if (input.value) search(input.value); });
    input.addEventListener("keydown", function (e) {
      if (e.key === "ArrowDown" && shown.length) { e.preventDefault(); mark((selected + 1) % shown.length); }
      else if (e.key === "ArrowUp" && shown.length) { e.preventDefault(); mark((selected - 1 + shown.length) % shown.length); }
      else if (e.key === "Enter" && shown[selected]) { window.location.href = BASE + shown[selected].url; }
      else if (e.key === "Escape") { results.hidden = true; input.blur(); }
    });
    document.addEventListener("click", function (e) {
      if (!results.contains(e.target) && e.target !== input) results.hidden = true;
    });
    document.addEventListener("keydown", function (e) {
      var tag = (document.activeElement && document.activeElement.tagName) || "";
      if (e.key === "/" && tag !== "INPUT" && tag !== "TEXTAREA") { e.preventDefault(); input.focus(); }
    });
  }

  // ---- Table of contents ----------------------------------------------
  function slugify(text) {
    return text.toLowerCase().replace(/[^a-z0-9]+/g, "-").replace(/^-+|-+$/g, "") || "section";
  }

  function initToc() {
    var prose = document.querySelector(".doc-body");
    var lists = document.querySelectorAll(".toc-list");
    if (!prose || !lists.length) return;

    var heads = Array.prototype.slice.call(prose.querySelectorAll("h2, h3"));
    var used = {};
    heads.forEach(function (h) {
      if (!h.id) {
        var base = slugify(h.textContent), id = base, n = 2;
        while (used[id] || document.getElementById(id)) id = base + "-" + n++;
        h.id = id;
      }
      used[h.id] = true;
      var anchor = document.createElement("a");
      anchor.className = "heading-anchor";
      anchor.href = "#" + h.id;
      anchor.setAttribute("aria-label", "Link to this section");
      anchor.textContent = "#";
      h.appendChild(anchor);
    });

    if (heads.length < 2) {
      document.querySelectorAll(".toc-block, .toc-mobile").forEach(function (el) { el.remove(); });
      return;
    }

    lists.forEach(function (list) {
      heads.forEach(function (h) {
        var a = document.createElement("a");
        a.href = "#" + h.id;
        a.className = h.tagName === "H3" ? "toc-h3" : "toc-h2";
        a.dataset.target = h.id;
        a.textContent = h.textContent.replace(/#$/, "");
        list.appendChild(a);
      });
    });

    // Scroll spy: the last heading above the top third of the viewport is active.
    var links = document.querySelectorAll(".toc-list a");
    var current = null;
    function setActive(id) {
      if (id === current) return;
      current = id;
      links.forEach(function (a) {
        var on = a.dataset.target === id;
        a.classList.toggle("active", on);
        if (on) {
          var box = a.closest(".toc-scroll");
          if (box && box.offsetParent !== null) {
            var top = a.offsetTop - box.offsetTop, bottom = top + a.offsetHeight;
            if (top < box.scrollTop) box.scrollTop = top - 8;
            else if (bottom > box.scrollTop + box.clientHeight) box.scrollTop = bottom - box.clientHeight + 8;
          }
        }
      });
    }
    var ticking = false;
    function onScroll() {
      if (ticking) return;
      ticking = true;
      requestAnimationFrame(function () {
        ticking = false;
        var limit = window.innerHeight / 3, id = heads[0].id;
        for (var i = 0; i < heads.length; i++) {
          if (heads[i].getBoundingClientRect().top <= limit) id = heads[i].id; else break;
        }
        setActive(id);
      });
    }
    window.addEventListener("scroll", onScroll, { passive: true });
    onScroll();

    // Toggle
    function setToc(state) {
      document.body.setAttribute("data-toc", state);
      store(false, TOC_KEY, state);
    }
    document.querySelectorAll("[data-toc-toggle]").forEach(function (btn) {
      btn.addEventListener("click", function () {
        setToc(document.body.getAttribute("data-toc") === "closed" ? "open" : "closed");
      });
    });
    setToc(store(true, TOC_KEY) === "closed" ? "closed" : "open");
  }

  // ---- Page tools -----------------------------------------------------
  function initTools() {
    document.querySelectorAll("[data-print]").forEach(function (b) {
      b.addEventListener("click", function () { window.print(); });
    });
  }

  // ---- Code block copy buttons -----------------------------------------
  function copyText(text) {
    if (navigator.clipboard && window.isSecureContext) return navigator.clipboard.writeText(text);
    return new Promise(function (resolve, reject) {
      var ta = document.createElement("textarea");
      ta.value = text; ta.style.position = "fixed"; ta.style.opacity = "0";
      document.body.appendChild(ta); ta.select();
      try { document.execCommand("copy") ? resolve() : reject(); } catch (e) { reject(e); }
      document.body.removeChild(ta);
    });
  }
  function initCopy() {
    document.querySelectorAll(".doc-body pre[class*='language-']").forEach(function (pre) {
      if (pre.parentElement.classList.contains("code-block")) return;
      var wrap = document.createElement("div");
      wrap.className = "code-block";
      pre.parentNode.insertBefore(wrap, pre);
      wrap.appendChild(pre);
      var btn = document.createElement("button");
      btn.type = "button";
      btn.className = "code-copy";
      btn.textContent = "Copy";
      btn.setAttribute("aria-label", "Copy code to clipboard");
      btn.addEventListener("click", function () {
        var code = pre.querySelector("code") || pre;
        copyText(code.textContent.replace(/\n$/, "")).then(function () {
          btn.textContent = "Copied"; btn.dataset.state = "copied";
        }, function () {
          btn.textContent = "Failed"; btn.dataset.state = "failed";
        }).then(function () {
          setTimeout(function () { btn.textContent = "Copy"; delete btn.dataset.state; }, 1600);
        });
      });
      wrap.appendChild(btn);
    });
  }

  // ---- Scroll reveal ----------------------------------------------------
  function initReveal() {
    if (!("IntersectionObserver" in window)) return;
    if (window.matchMedia("(prefers-reduced-motion: reduce)").matches) return;
    var targets = document.querySelectorAll(".doc-body > *, .doc-pager > *");
    var io = new IntersectionObserver(function (entries) {
      entries.forEach(function (e) {
        if (e.isIntersecting) { e.target.classList.add("in"); io.unobserve(e.target); }
      });
    }, { rootMargin: "0px 0px -8% 0px", threshold: 0.02 });
    targets.forEach(function (el) { el.classList.add("reveal"); io.observe(el); });
  }

  // ---- Mermaid --------------------------------------------------------
  var mermaidLoading = null;
  function isDarkTheme() {
    var bg = getComputedStyle(document.body).backgroundColor.match(/[\d.]+/g) || [255, 255, 255];
    return (0.299 * bg[0] + 0.587 * bg[1] + 0.114 * bg[2]) < 140;
  }
  function loadMermaid() {
    if (window.mermaid) return Promise.resolve(window.mermaid);
    if (mermaidLoading) return mermaidLoading;
    mermaidLoading = new Promise(function (resolve, reject) {
      var s = document.createElement("script");
      s.src = BASE + "/assets/js/mermaid.min.js";
      s.onload = function () { resolve(window.mermaid); };
      s.onerror = reject;
      document.head.appendChild(s);
    });
    return mermaidLoading;
  }
  function renderMermaid() {
    var nodes = document.querySelectorAll("pre.mermaid");
    if (!nodes.length) return;
    nodes.forEach(function (n) {
      if (!n.dataset.source) n.dataset.source = n.textContent;
      else { n.textContent = n.dataset.source; n.removeAttribute("data-processed"); }
    });
    loadMermaid().then(function (mermaid) {
      mermaid.initialize({ startOnLoad: false, theme: isDarkTheme() ? "dark" : "default", securityLevel: "strict" });
      return mermaid.run({ nodes: nodes });
    }).catch(function (e) { console.error("mermaid failed", e); });
  }

  document.addEventListener("DOMContentLoaded", function () {
    initSearch();
    initToc();
    initTools();
    initCopy();
    initReveal();
    renderMermaid();
  });
})();
