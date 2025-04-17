import type { ReservationFormData } from "@/lib/types"
import { ApiError } from "./api-error"

const API_URL = import.meta.env.VITE_API_URL

export class ReservationService {
  static async checkAvailability(formData: ReservationFormData) {
    const queryParams = new URLSearchParams({
      startDate: formData.startDate?.toISOString() || "",
      endDate: formData.endDate?.toISOString() || "",
      carModel: formData.carModel || "",
      pickupLocation: formData.pickupLocation || "",
      returnLocation: formData.returnLocation || "",
    })

    const response = await fetch(`${API_URL}/v1/api/cars/availability?${queryParams.toString()}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    })

    const data = await response.json()
    if (!response.ok) {
        if (data.errors == null) {
            throw new ApiError("Failed to fetch car availability", data.title);
        }
      throw new ApiError("Failed to fetch car availability", data.errors, response.status);
    }

    return data
  }

  static async createReservation(formData: ReservationFormData) {
    const response = await fetch(`${API_URL}/v1/api/reservations`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(formData),
    })

    const data = await response.json()
    if (!response.ok) {
        throw new ApiError("Failed to create reservation", data.errors, response.status);
    }

    return data
  }
}