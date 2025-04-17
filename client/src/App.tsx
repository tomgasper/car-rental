import CarReservationForm from "./components/CarReservation/CarReservationForm"
import { CarRentalReservationProvider } from "@/contexts/CarRentalReservationProvider"
    
function App() {
  return (
    <CarRentalReservationProvider>
      <CarReservationForm />
    </CarRentalReservationProvider>
  )
}

export default App
