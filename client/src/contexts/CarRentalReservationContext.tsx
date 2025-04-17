import { createContext, useContext } from "react"
import type { Location, CarModel } from "@/lib/types"

interface CarRentalReservationContextType {
  locations: Location[]
  carModels: CarModel[]
  isLoading: boolean
  error: string | null
}

export const CarRentalReservationContext = createContext<CarRentalReservationContextType | undefined>(undefined)

export function useCarRentalReservationContext() {
  const context = useContext(CarRentalReservationContext)
  if (context === undefined) {
    throw new Error("useCarRentalReservationContext must be used within a CarRentalReservationProvider")
  }
  return context
}