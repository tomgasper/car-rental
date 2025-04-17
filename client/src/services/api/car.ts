const API_URL = import.meta.env.VITE_API_URL

export class CarService {
  static async getAvailableCars(params: {
    startDate: Date
    endDate: Date
    carModel: string
    pickupLocation: string
    returnLocation: string
  }) {
    const queryParams = new URLSearchParams({
      startDate: params.startDate.toISOString(),
      endDate: params.endDate.toISOString(),
      carModel: params.carModel,
      pickupLocation: params.pickupLocation,
      returnLocation: params.returnLocation,
    })

    const response = await fetch(`${API_URL}/v1/api/cars/availability?${queryParams.toString()}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    })

    const data = await response.json()
    if (!response.ok) {
      throw new Error(data.errors?.[0] || "Failed to fetch car availability")
    }

    return data
  }

  static async getCarModels() {
    const response = await fetch(`${API_URL}/v1/api/cars`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    })
  
    const data = await response.json()
    if (!response.ok) {
      throw new Error(data.errors?.[0] || "Failed to fetch car models")
    }
  
    return data
  }
}