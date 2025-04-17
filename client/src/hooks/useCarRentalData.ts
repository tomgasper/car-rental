import { useState, useEffect } from "react"
import { LocationService } from "@/services/api/location"
import { CarService } from "@/services/api/car"
import type { Location, CarModel } from "@/lib/types"

export function useCarRentalData() {
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

  return { locations, carModels, isLoading, error }
}