const AMAZON_TAG = "dealchaserai-20";          
const BOL_PARTNER_ID = "YOUR_BOL_PARTNER_ID";  // from bol.com partnerprogramma

function toSearchQuery(keywords: string): string {
  return encodeURIComponent(keywords.trim());
}

export function buildAmazonUrl(
  searchKeywords: string,
  region?: string
): string {
  const q = toSearchQuery(searchKeywords || "");
  if (!q) return "";

  // Default to US Amazon – biggest Black Friday market
  let base = "https://www.amazon.com";

  if (region === "EU") base = "https://www.amazon.de";
  if (region === "UK") base = "https://www.amazon.co.uk";

  return `${base}/s?k=${q}&tag=${AMAZON_TAG}`;
}

export function buildBolUrl(searchKeywords: string): string {
  const q = toSearchQuery(searchKeywords || "");
  if (!q) return "";
  return `https://www.bol.com/nl/nl/s/?searchtext=${q}&Referrer=${BOL_PARTNER_ID}`;
}
