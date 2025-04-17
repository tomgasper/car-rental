import { Location, CarModel } from "@/lib/types"
import { CarService } from "@/services/api/car"
import { LocationService } from "@/services/api/location"
import { useEffect, useState } from "react"
import { CarRentalReservationContext } from "./CarRentalReservationContext"

export function CarRentalReservationProvider({ children }: { children: React.ReactNode }) {
    const [locations, setLocations] = useState<Location[]>([])
    const [carModels, setCarModels] = useState<CarModel[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
  
    useEffect(() => {
      const fetchData = async () => {
        try {
          const [locationsData, carModelsData] = await Promise.all([
            LocationService.getLocations(),
            CarService.getCarModels(),
          ])
          setLocations(locationsData)
          setCarModels(carModelsData)
        } catch (err) {
          setError(err instanceof Error ? err.message : "Failed to fetch data")
        } finally {
          setIsLoading(false)
        }
      }
  
      fetchData()
    }, [])
  
    return (
      <CarRentalReservationContext.Provider value={{ locations, carModels, isLoading, error }}>
        {children}
      </CarRentalReservationContext.Provider>
    )
  }