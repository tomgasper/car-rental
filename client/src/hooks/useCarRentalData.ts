import { useCarRentalReservationContext } from "@/contexts/CarRentalReservation/CarRentalReservationContext";

export function useCarRentalData() {
  return useCarRentalReservationContext()
}