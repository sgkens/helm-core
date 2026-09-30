// Keep HTML, XML and machine-readable links on the same production URL.
function absoluteUrl(pageUrl, siteUrl) {
  return new URL(String(pageUrl).replace(/^\/+/, ""), siteUrl).href;
}

function markdownUrl(filePathStem) {
  return `/docs/src/${filePathStem.replace(/^\/content\//, "")}.md`;
}

function pageTitle(title, splash = false) {
  return splash ? "HELM Core — Space Engineers Telemetry Display Script" : `${title} | HELM Core Docs`;
}

function structuredData(page, site, title, description, splash, language) {
  const url = absoluteUrl(page.url, site.url);
  const websiteId = site.url + "#website";
  const softwareId = site.url + "#software";
  return JSON.stringify({
    "@context": "https://schema.org",
    "@graph": [
      {
        "@type": "WebSite", "@id": websiteId,
        name: "HELM Core Docs", url: site.url, description: site.description, inLanguage: language,
      },
      {
        "@type": "SoftwareSourceCode", "@id": softwareId,
        name: site.name, description: site.description, url: site.url,
        codeRepository: site.repoUrl, programmingLanguage: "C#", version: site.version,
        license: site.repoUrl + "/-/blob/" + site.branch + "/LICENSE",
      },
      {
        "@type": splash ? "WebPage" : "TechArticle", "@id": url + "#page",
        url, name: pageTitle(title, splash), description: description || site.description,
        inLanguage: language, isPartOf: { "@id": websiteId }, about: { "@id": softwareId },
      },
    ],
  }).replace(/</g, "\\u003c");
}

module.exports = { absoluteUrl, markdownUrl, pageTitle, structuredData };
