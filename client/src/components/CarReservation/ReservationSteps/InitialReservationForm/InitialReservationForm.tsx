import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import type { ReservationFormData } from "@/lib/types"
import { isInitialFormValid } from "@/lib/utils/reservation"
import { ChevronRight } from "lucide-react"
import { CarSelection } from "./CarSelection"
import { DateSelector } from "./DateSelector"
import { LocationSelector } from "./LocationSelector"

interface InitialReservationFormProps {
  formData: ReservationFormData
  onInputChange: (field: keyof ReservationFormData, value: any) => void
  onCheckAvailability: () => void
  isLoading: boolean
}

export function InitialReservationForm({ formData, onInputChange, onCheckAvailability, isLoading }: InitialReservationFormProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Reservation Details</CardTitle>
        <CardDescription>Enter your rental details to check availability</CardDescription>
      </CardHeader>
      <CardContent className="space-y-6">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          {/* Date Selection */}
          <DateSelector
            id="startDate"
            label="Pickup Date"
            date={formData.startDate}
            onSelect={(date) => onInputChange("startDate", date)}
            minDate={new Date()}
          />

          <DateSelector
            id="endDate"
            label="Return Date"
            date={formData.endDate}
            onSelect={(date) => onInputChange("endDate", date)}
            minDate={formData.startDate}
          />

          <LocationSelector
            id="pickupLocation"
            label="Pickup Location"
            value={formData.pickupLocation}
            onChange={(value) => onInputChange("pickupLocation", value)}
          />

          <LocationSelector
            id="returnLocation"
            label="Return Location"
            value={formData.returnLocation}
            onChange={(value) => onInputChange("returnLocation", value)}
          />
        </div>

        <CarSelection value={formData.carModel} onChange={(value) => onInputChange("carModel", value)} />
      </CardContent>
      <CardFooter>
        <Button
          onClick={onCheckAvailability}
          disabled={!isInitialFormValid(formData) || isLoading}
          className="ml-auto"
          size="lg"
        >
          {isLoading ? "Checking..." : "Check Availability"}
          <ChevronRight className="ml-2 h-4 w-4" />
        </Button>
      </CardFooter>
    </Card>
  )
}
