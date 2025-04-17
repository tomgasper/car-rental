import { format } from "date-fns"
import { CalendarIcon, MapPin } from "lucide-react"
import type { ReservationFormData } from "@/lib/types"
import { useCarRentalData } from "@/hooks/useCarRentalData"

interface ReservationSummaryProps {
  formData: ReservationFormData
}

export function ReservationSummary({ formData }: ReservationSummaryProps) {
  const { carModels, locations } = useCarRentalData()
  const car = carModels.find((model) => model.id === formData.carModel)
  const pickupLocationName = locations.find((loc) => loc.id === formData.pickupLocation)?.name
  const returnLocationName = locations.find((loc) => loc.id === formData.returnLocation)?.name

  return (
    <div className="flex flex-col md:flex-row gap-6 items-center">
      <div className="w-full md:w-1/3">
        {car && <img src={car.image || "/placeholder.svg"} alt={car.name} className="w-full rounded-md" />}
      </div>
      <div className="w-full md:w-2/3 space-y-4">
        <div>
          <h3 className="text-xl font-semibold">{car?.name}</h3>
          <p className="text-base text-muted-foreground">{car?.description}</p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="flex items-center gap-2">
            <CalendarIcon className="h-5 w-5 text-muted-foreground" />
            <div>
              <div className="text-sm text-muted-foreground">Pickup Date</div>
              <div>{formData.startDate && format(formData.startDate, "PPP")}</div>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <CalendarIcon className="h-5 w-5 text-muted-foreground" />
            <div>
              <div className="text-sm text-muted-foreground">Return Date</div>
              <div>{formData.endDate && format(formData.endDate, "PPP")}</div>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <MapPin className="h-5 w-5 text-sm text-muted-foreground" />
            <div>
              <div className="text-muted-foreground">Pickup Location</div>
              <div>{pickupLocationName}</div>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <MapPin className="h-5 w-5 text-muted-foreground" />
            <div>
              <div className="text-sm text-muted-foreground">Return Location</div>
              <div>{returnLocationName}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
