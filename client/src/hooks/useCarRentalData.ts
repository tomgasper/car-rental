import { useCarRentalReservationContext } from "@/contexts/CarRentalReservationContext";

export function useCarRentalData() {
  return useCarRentalReservationContext()
}