import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import type { ReservationFormData } from "@/lib/types"
import { format } from "date-fns"
import { Car, CalendarIcon, Check, MapPin } from "lucide-react"
import { useCarRentalData } from "@/hooks/useCarRentalData"

interface CompleteProps {
  formData: ReservationFormData
  reservationId: string
  onReset: () => void
}

export function Complete({ formData, reservationId, onReset }: CompleteProps) {
  const { carModels, locations } = useCarRentalData()
  const car = carModels.find((model) => model.id === formData.carModel)
  const pickupLocationName = locations.find((loc) => loc.id === formData.pickupLocation)?.name
  const returnLocationName = locations.find((loc) => loc.id === formData.returnLocation)?.name

  return (
    <Card>
      <CardHeader className="text-center">
        <div className="mx-auto mb-4 flex h-12 w-12 items-center justify-center rounded-full bg-green-100">
          <Check className="h-6 w-6 text-green-600" />
        </div>
        <CardTitle>Reservation Confirmed!</CardTitle>
        <CardDescription>Your Tesla is reserved and ready for your trip.</CardDescription>
      </CardHeader>
      <CardContent className="space-y-6">
        <div className="rounded-lg bg-muted p-4 text-center">
          <div className="text-sm text-muted-foreground">Reservation ID</div>
          <div className="text-xl font-semibold">{reservationId}</div>
          <div className="text-xs text-muted-foreground mt-1">
            Please save this ID for your records. You'll need it to modify or cancel your reservation.
          </div>
        </div>

        <div className="space-y-4">
          <h3 className="font-medium">Reservation Details</h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="flex items-center gap-2">
              <Car className="h-5 w-5 text-muted-foreground" />
              <div>
                <div className="text-sm text-muted-foreground">Vehicle</div>
                <div>{car?.name}</div>
              </div>
            </div>
            <div className="flex items-center gap-2">
              <CalendarIcon className="h-5 w-5 text-muted-foreground" />
              <div>
                <div className="text-sm text-muted-foreground">Rental Period</div>
                <div>
                  {formData.startDate && format(formData.startDate, "MMM d, yyyy")} -{" "}
                  {formData.endDate && format(formData.endDate, "MMM d, yyyy")}
                </div>
              </div>
            </div>
            <div className="flex items-center gap-2">
              <MapPin className="h-5 w-5 text-muted-foreground" />
              <div>
                <div className="text-sm text-muted-foreground">Pickup</div>
                <div>{pickupLocationName}</div>
              </div>
            </div>
            <div className="flex items-center gap-2">
              <MapPin className="h-5 w-5 text-muted-foreground" />
              <div>
                <div className="text-sm text-muted-foreground">Return</div>
                <div>{returnLocationName}</div>
              </div>
            </div>
          </div>
        </div>

        <div className="rounded-lg border p-4">
          <h3 className="font-medium mb-2">Important Information</h3>
          <ul className="space-y-1 text-sm">
            <li>• Please arrive at the pickup location 30 minutes before your scheduled time.</li>
            <li>• Bring your driver's license, ID, and the credit card used for the reservation.</li>
            <li>• The vehicle will be fully charged upon pickup.</li>
            <li>• For any changes, please contact us at least 24 hours in advance.</li>
          </ul>
        </div>

        <div className="text-center text-sm text-muted-foreground">
          A confirmation email has been sent to {formData.email} with all the details of your reservation.
        </div>
      </CardContent>
      <CardFooter className="justify-center">
        <Button onClick={onReset}>Make Another Reservation</Button>
      </CardFooter>
    </Card>
  )
}
