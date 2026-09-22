/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./_includes/**/*.njk",
    "./content/**/*.md",
    "./assets/js/**/*.js",
    "./*.njk",
    "../webutil/index.html",
    "../webutil/js/*.mjs"
  ],
  theme: {
    extend: {
      fontFamily: {
        mono: ['"JetBrains Mono"', '"Cascadia Code"', 'Consolas', 'monospace']
      }
    },
  },
  plugins: [
    require("@tailwindcss/typography"),
    require("daisyui")
  ],
  daisyui: {
    themes: [
      "dark",
      "synthwave",
      "cyberpunk",
      "night",
      "dim",
      "dracula",
      "forest",
      "black",
      "corporate"
    ],
    darkTheme: "night"
  }
};
