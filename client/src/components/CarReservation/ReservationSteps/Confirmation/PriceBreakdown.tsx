import { Separator } from "@/components/ui/separator"
import { useCarRentalData } from "@/hooks/useCarRentalData"
import type { ReservationFormData } from "@/lib/types"
import { calculateDays } from "@/lib/utils/reservation"

interface PriceBreakdownProps {
  formData: ReservationFormData
}

export function PriceBreakdown({ formData }: PriceBreakdownProps) {
  const { carModels } = useCarRentalData()
  const car = carModels.find((model) => model.id === formData.carModel)
  const days = calculateDays(formData.startDate, formData.endDate)

  return (
    <div className="space-y-2">
      <h3 className="font-medium">Price Breakdown</h3>
      <div className="space-y-1 text-sm">
        <div className="flex justify-between">
          <span>
            {car?.name} (€{(formData.totalPrice / days).toFixed(2)} x {days} days)
          </span>
          <span>€{formData.totalPrice}</span>
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
          <span>€{formData.totalPrice}</span>
        </div>
      </div>
    </div>
  )
}
