export interface DealIdea {
  name: string;
  description: string;
  priceEstimate: string;
  category: string;
  searchKeywords: string;

  amazonUrl?: string;
  bolUrl?: string;
}

export interface DealRequest {
  category: string;
  budgetMin?: number | null;
  budgetMax?: number | null;
  brandPreference: string;
  searchTerm: string;
  region: string;
  numberOfIdeas: number;
}
