import { useState } from "react";
import type { FormEvent } from "react";
import { fetchDeals } from "./api";
import type { DealIdea, DealRequest } from "./types";
import { buildAmazonUrl, buildBolUrl } from "./affiliate";
import "./App.css";

function App() {
  const [form, setForm] = useState<DealRequest>({
    category: "Tech",
    budgetMin: 25,
    budgetMax: 300,
    region: "US",
    numberOfIdeas: 5,
    searchTerm: "",
    brandPreference: "any",
  });

  const [selectedCategory, setSelectedCategory] = useState<string>("Tech");

  const [deals, setDeals] = useState<DealIdea[] | null>(null);
  const [loading, setLoading] = useState(false);

  async function submit(e: FormEvent) {
    e.preventDefault();
    setLoading(true);
    setDeals(null);

    try {
      const result = await fetchDeals(form);
      const enriched = result.map((idea) => {
        const keywords = idea.searchKeywords || idea.name;
        return {
          ...idea,
          amazonUrl: buildAmazonUrl(keywords, form.region),
          bolUrl: buildBolUrl(keywords),
        };
      });

      setDeals(enriched);
    } catch (err) {
      console.error(err);
    }
    setLoading(false);
  }

  return (
    <>
      {/* HEADER */}
      <header className="header">
        <img src="/logo.png" className="logo-img" alt="DealChaser.ai" />

        <div className="title-box">
          <h1 className="site-title">DealChaser.ai</h1>
          <p className="slogan">AI does the chasing, while you do the saving.</p>
        </div>
      </header>

      {/* MAIN CONTENT */}
      <div className="container">
        <h2>Find Black Friday Deals in Seconds</h2>
        <p>Just enter your budget + category and let AI hunt down the best offers.</p>

        <form onSubmit={submit}>
          <label>Category</label>
 <select
  value={selectedCategory}
  onChange={(e) => {
    const value = e.target.value;
    setSelectedCategory(value);

    if (value !== "Other") {
      // For normal categories, keep form.category in sync
      setForm({ ...form, category: value });
    } else {
      // When "Other" is selected, clear category until user types
      setForm({ ...form, category: "" });
    }
  }}
>
  <option value="Tech">Tech</option>
  <option value="Gaming">Gaming</option>
  <option value="Beauty">Beauty</option>
  <option value="Home">Home</option>
  <option value="Fitness">Fitness</option>
  <option value="Other">Other</option>
</select>

{selectedCategory === "Other" && (
  <input
    type="text"
    placeholder="Enter your own category..."
    value={form.category}
    onChange={(e) =>
      setForm({
        ...form,
        category: e.target.value,
      })
    }
    style={{ marginTop: "10px", borderColor: "#00ff57" }}
  />
)}
          <label>Budget Range</label>
          <div className="row-flex">
            <input type="number" placeholder="Min" value={form.budgetMin ?? ""}
              onChange={(e) => setForm({ ...form, budgetMin: parseInt(e.target.value) })} />
            <input type="number" placeholder="Max" value={form.budgetMax ?? ""}
              onChange={(e) => setForm({ ...form, budgetMax: parseInt(e.target.value) })} />
          </div>

          <label>Search Term (optional)</label>
          <input type="text" placeholder="headphones, massage gun, monitor..."
            value={form.searchTerm} onChange={(e) => setForm({ ...form, searchTerm: e.target.value })} />

          <button type="submit" disabled={loading}>
            {loading ? "Searching..." : "Find Deals"}
          </button>
        </form>

        <div className="results">
          {deals && deals.map((idea, i) => (
            <div key={i} className="deal-card">
              <h3>{idea.name}</h3>
              <p>{idea.description}</p>
              <p><b>Price Estimate:</b> {idea.priceEstimate}</p>

              <div className="links">
                {idea.amazonUrl && (
                  <a className="btn" href={idea.amazonUrl} target="_blank">Amazon</a>
                )}
                {idea.bolUrl && (
                  <a className="btn-alt" href={idea.bolUrl} target="_blank">Bol.com (NL)</a>
                )}
              </div>
            </div>
          ))}
        </div>
      </div>
    </>
  );
}

export default App;
