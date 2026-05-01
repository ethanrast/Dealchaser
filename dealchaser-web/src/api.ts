import type { DealIdea, DealRequest } from "./types";

// IMPORTANT: backend URL. Use your real port from dotnet run.
const API_BASE_URL = "";

export async function fetchDeals(request: DealRequest): Promise<DealIdea[]> {
  const response = await fetch(`${API_BASE_URL}/api/deals`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error(`API error: ${response.status}`);
  }

  return response.json();
}
