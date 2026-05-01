const AMAZON_TAG = "dealchaserai-20";

// Deze komt uit je bestaande werkende Bol-tekstlink (zonder de url=... en name=...)
const BOL_AFFILIATE_PREFIX =
  "https://partner.bol.com/click/click?p=2&t=url&s=1489024&f=TXL&url=";

const BOL_SID = "dealchaser";

export function buildAmazonUrl(
  searchKeywords: string,
  region?: string
): string {
  const q = encodeURIComponent(searchKeywords.trim() || "");
  if (!q) return "";

  let base = "https://www.amazon.com";
  if (region === "EU") base = "https://www.amazon.de";
  if (region === "UK") base = "https://www.amazon.co.uk";

  return `${base}/s?k=${q}&tag=${AMAZON_TAG}`;
}

export function buildBolUrl(searchKeywords: string): string {
  const raw = searchKeywords.trim();
  if (!raw) return "";

  const bolSearchUrl = `https://www.bol.com/nl/nl/s/?searchtext=${encodeURIComponent(
    raw
  )}`;
  const encodedUrl = encodeURIComponent(bolSearchUrl);

  return `${BOL_AFFILIATE_PREFIX}${encodedUrl}&name=${encodeURIComponent(
    BOL_SID
  )}`;
}
