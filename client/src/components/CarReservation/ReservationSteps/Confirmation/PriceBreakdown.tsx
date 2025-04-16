import { Separator } from "@/components/ui/separator"
import type { ReservationFormData } from "@/lib/types"
import { calculateDays, getSelectedCar } from "@/lib/utils/reservation"

interface PriceBreakdownProps {
  formData: ReservationFormData
}

export function PriceBreakdown({ formData }: PriceBreakdownProps) {
  const car = getSelectedCar(formData.carModel)
  const days = calculateDays(formData.startDate, formData.endDate)
  const totalPrice = car ? car.pricePerDay * days : 0

  return (
    <div className="space-y-2">
      <h3 className="font-medium">Price Breakdown</h3>
      <div className="space-y-1 text-sm">
        <div className="flex justify-between">
          <span>
            {car?.name} (€{car?.pricePerDay} x {days} days)
          </span>
          <span>€{car?.pricePerDay ? car.pricePerDay * days : 0}</span>
        </div>
        <div className="flex justify-between">
          <span>Insurance</span>
          <span>Included</span>
        </div>
        <div className="flex justify-between">
          <span>Charging</span>
          <span>Included</span>
        </div>
        <Separator className="my-2" />
        <div className="flex justify-between font-semibold">
          <span>Total</span>
          <span>€{totalPrice}</span>
        </div>
      </div>
    </div>
  )
}
