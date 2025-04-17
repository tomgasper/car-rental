//const API_URL = import.meta.env.VITE_API_URL
import { API_URL } from "./constants"

export class LocationService {
  static async getLocations() {
    const response = await fetch(`${API_URL}/v1/api/locations`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    })

    const data = await response.json()
    if (!response.ok) {
      throw new Error(data.errors?.[0] || "Failed to fetch locations")
    }

    return data
  }
}