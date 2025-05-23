import type { ReservationFormData, } from "../types"

export const calculateDays = (startDate?: Date, endDate?: Date): number => {
  if (!startDate || !endDate) return 0
  const diffTime = Math.abs(endDate.getTime() - startDate.getTime())
  const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24))
  return diffDays || 1 // Minimum 1 day
}

export const isInitialFormValid = (formData: ReservationFormData): boolean => {
  return !!(
    formData.startDate &&
    formData.endDate &&
    formData.pickupLocation &&
    formData.returnLocation &&
    formData.carModel
  )
}

export const isPersonalInfoValid = (formData: ReservationFormData): boolean => {
  return !!(formData.firstName && formData.lastName && formData.email && formData.phoneNumber)
}

